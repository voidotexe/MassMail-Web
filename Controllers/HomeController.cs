using Microsoft.AspNetCore.Mvc;
using MassMailWeb.Models;
using MassMailWeb.Helpers;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace MassMailWeb.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        IWebHostEnvironment _webHostEnvironment;
        private string filePath;
        private List<string> attachments = new List<string>();

        public HomeController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            Email email = new Email();            

            ViewBag.SentSuccessfully = false;

            return View(email);
        }

        [HttpPost]
        public IActionResult Index(Email email)
        {
            EmailHelper.SetAttachments(_webHostEnvironment, email.Attachments);
            EmailHelper.SetMessage(email.From, email.ToField, email.Subject, email.Body, email.BccOrNot, email.HtmlOrNot);
            EmailHelper.SendEmail(email.Password);

            ViewBag.SentSuccessfully = true;
            
            return View();
        }
    }
}
