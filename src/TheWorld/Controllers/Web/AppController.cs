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

namespace TheWorld.Controllers.Web
{
    public class AppController : Controller
    {
        private IMailService mailService;
        private WorldContext dbContext;
        AppSettings Options { get; }

        public AppController(IMailService service, IOptions<AppSettings> optionsAccessor, WorldContext dbContext)
        {
            mailService = service;
            Options = optionsAccessor.Value;
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var trips = dbContext.Trips.OrderBy(t => t.Name).ToList();
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
