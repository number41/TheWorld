using AutoMapper;
using Microsoft.AspNet.Mvc;
using System.Collections.Generic;
using System.Net;
using TheWorld.Models;
using TheWorld.Models.Repos;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Api
{
    [Route("api/trips")]
    public class TripController : Controller
    {
        private IWorldRepository repo;

        public TripController(IWorldRepository repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public JsonResult Get()
        {
            return Json(Mapper.Map<IEnumerable<TripViewModel>>(repo.GetAllTripsWithStops()));
        }

        [HttpPost]
        public JsonResult Post([FromBody] TripViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = "failed", ModelState = ModelState });
            }

            var newTrip = Mapper.Map<Trip>(vm);
            // Save the newTrip to the DB
            Response.StatusCode = (int)HttpStatusCode.Created;
            return Json(Mapper.Map<TripViewModel>(newTrip));
        }
    }
}
