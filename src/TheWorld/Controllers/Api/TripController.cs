using AutoMapper;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using System.Collections.Generic;
using System.Net;
using TheWorld.Models;
using TheWorld.Models.Repos;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Api
{
    [Authorize]
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
            var trips = repo.GetUserTripsWithStops(User.Identity.Name);
            var results = Mapper.Map<IEnumerable<TripViewModel>>(trips);

            return Json(results);
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
            newTrip.UserName = User.Identity.Name;
            repo.AddTrip(newTrip);

            if (repo.SaveAll())
            {
                Response.StatusCode = (int)HttpStatusCode.Created;
                return Json(Mapper.Map<TripViewModel>(newTrip));
            } else
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new { Message = "Failed to save to DB" });
            }
        }
    }
}
