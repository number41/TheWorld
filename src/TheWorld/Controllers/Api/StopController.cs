using AutoMapper;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TheWorld.Models.Repos;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Api
{
    [Route("api/trips/{tripName}/stops")]
    public class StopController : Controller
    {
        private ILogger<StopController> logger;
        private IWorldRepository repo;

        public StopController(IWorldRepository repo, ILogger<StopController> logger)
        {
            this.repo = repo;
            this.logger = logger;
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
    }
}
