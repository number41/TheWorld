using AutoMapper;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TheWorld.Models;
using TheWorld.Models.Repos;
using TheWorld.Services;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Api
{
    [Route("api/trips/{tripName}/stops")]
    public class StopController : Controller
    {
        private CoordService coordService;
        private ILogger<StopController> logger;
        private IWorldRepository repo;

        public StopController(IWorldRepository repo, ILogger<StopController> logger, CoordService coordService)
        {
            this.repo = repo;
            this.logger = logger;
            logger.LogInformation("Inside StopController ctor");
            this.coordService = coordService;
        }

        [HttpGet("")]
        public JsonResult Get(string tripName)
        {
            try
            {
                var results = repo.GetTripByName(tripName);

                if (results == null)
                {
                    return Json(null);
                }

                return Json(Mapper.Map<IEnumerable<StopViewModel>>(results.Stops.OrderBy(s => s.Order)));
            }
            catch (Exception e)
            {
                string msg = $"Failed to get stops for trip {tripName}";
                logger.LogError(msg, e);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(msg);
            }
        }

        public async Task<JsonResult> Post(string tripName, [FromBody] StopViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid stop");
            }

            try
            {
                // Map to Entity
                var newStop = Mapper.Map<Stop>(vm);

                // Looking up Geocoordinates
                var coordResult = await coordService.Lookup(newStop.Name);
                if (!coordResult.Success)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json(coordResult.Message);
                }

                newStop.Latitude = coordResult.Latitude;
                newStop.Longitude = coordResult.Longitude;

                // Save to the database
                repo.AddStop(tripName, newStop);

                if (repo.SaveAll())
                {
                    logger.LogInformation("Saved the trip!");
                    Response.StatusCode = (int)HttpStatusCode.Created;
                    return Json(Mapper.Map<StopViewModel>(newStop));
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return Json("Failed to save new stop");
                }
            }
            catch (Exception e)
            {
                string msg = "Failed to save the new stop";
                logger.LogError(msg, e);
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(msg);
            }
        }
    }
}
