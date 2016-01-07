using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWorld.Models;
using TheWorld.Models.Repos;

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
        public JsonResult Post([FromBody] Trip newTrip)
        {
            return Json(true);
        }
    }
}
