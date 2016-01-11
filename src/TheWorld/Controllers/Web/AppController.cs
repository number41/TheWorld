using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using TheWorld.ViewModels;
using TheWorld.Services;
using TheWorld.Configs;
using Microsoft.Extensions.OptionsModel;
using TheWorld.Models;
using TheWorld.Models.Repos;
using Microsoft.AspNet.Authorization;

namespace TheWorld.Controllers.Web
{
    public class AppController : Controller
    {
        private IMailService mailService;
        private IWorldRepository worldRepo;
        AppSettings Options { get; }

        public AppController(IMailService service, IOptions<AppSettings> optionsAccessor, IWorldRepository worldRepo)
        {
            mailService = service;
            Options = optionsAccessor.Value;
            this.worldRepo = worldRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Trips()
        {
            var trips = worldRepo.GetAllTrips();
            return View(trips);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                var email = Options.SiteEmailAddress;
                if (String.IsNullOrWhiteSpace(email))
                {
                    ModelState.AddModelError("", "Error Sending email: server error");
                    return View();
                }
                
                if (mailService.SendMail(email, email, $"Contact Page from {model.Name} ({model.Email})", model.Message))
                {
                    ModelState.Clear();
                    ViewBag.Message = "Message received!";
                }
                
            }
            return View();
        }
    }
}
