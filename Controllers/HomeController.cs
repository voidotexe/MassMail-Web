using Microsoft.AspNetCore.Mvc;
using MassMailWeb.Models;
using MassMailWeb.Services;

namespace MassMailWeb.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            Email email = new Email();

            ViewBag.AuthenticationWarn = false;

            return View(email);
        }

        [HttpPost]
        public IActionResult Index(Email email)
        {
            EmailService.SetMessage(email.From, email.ToField, email.Subject, email.Body, email.BccOrNot, email.HtmlOrNot);

            if (EmailService.SendEmail(email.Password) != "Success")
            {
                ViewBag.AuthenticationWarn = true;

                return View();
            }
            
            ViewBag.AuthenticationWarn = false;
            
            return Content($"E-mail sent successfully!");
        }
    }
}
