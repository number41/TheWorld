using Microsoft.AspNet.Mvc;
using System.Net;
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
            return Json(repo.GetAllTripsWithStops());
        }

        [HttpPost]
        public JsonResult Post([FromBody] TripViewModel newTrip)
        {
            if (ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.Created;
                return Json(true);
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { Message = "failed", ModelState = ModelState });
        }
    }
}
