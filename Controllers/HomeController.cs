using Microsoft.AspNetCore.Mvc;
using MassMailWeb.Models;
using MassMailWeb.Helpers;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
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

            Cookies.From ??= HttpContext.Request.Cookies["From"];
            Cookies.To ??= HttpContext.Request.Cookies["To"];
            Cookies.Subject ??= HttpContext.Request.Cookies["Subject"];
            Cookies.Body ??= HttpContext.Request.Cookies["Body"];

            return View(email);
        }

        [HttpPost]
        public IActionResult Index(Email email)
        {
            EmailHelper.SetAttachments(_webHostEnvironment, email.Attachments);
            EmailHelper.SetMessage(email.From, email.ToField, email.Subject, email.Body, email.BccOrNot, email.HtmlOrNot);
            EmailHelper.SendEmail(email.Password);

            ViewBag.SentSuccessfully = true;

            HttpContext.Response.Cookies.Append("From", email.From);
            HttpContext.Response.Cookies.Append("To", email.ToField);
            HttpContext.Response.Cookies.Append("Subject", email.Subject);
            HttpContext.Response.Cookies.Append("Body", email.Body);

            Cookies.From = HttpContext.Request.Cookies["From"];
            Cookies.To = HttpContext.Request.Cookies["To"];
            Cookies.Subject = HttpContext.Request.Cookies["Subject"];
            Cookies.Body = HttpContext.Request.Cookies["Body"];

            return View();
        }
    }
}
