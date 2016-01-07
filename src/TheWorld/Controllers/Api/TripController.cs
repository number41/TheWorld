using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWorld.Models.Repos;

namespace TheWorld.Controllers.Api
{
    public class TripController : Controller
    {
        private IWorldRepository repo;

        public TripController(IWorldRepository repo)
        {
            this.repo = repo;
        }
        [HttpGet("api/trips")]
        public JsonResult Get()
        {
            return Json(repo.GetAllTripsWithStops());
        }
    }
}
