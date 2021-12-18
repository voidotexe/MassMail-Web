using Microsoft.AspNetCore.Mvc;
using MassMailWeb.Models;
using MassMailWeb.Services;
using MailKit.Security;

namespace MassMailWeb.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
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
            EmailService.SetMessage(email.From, email.ToField, email.Subject, email.Body, email.BccOrNot, email.HtmlOrNot);
            EmailService.SendEmail(email.Password);

            ViewBag.SentSuccessfully = true;
            
            return View();
        }
    }
}
