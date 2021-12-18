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

            ViewBag.AuthenticationWarn = false;
            ViewBag.SentSuccessfully = false;

            return View(email);
        }

        [HttpPost]
        public IActionResult Index(Email email)
        {
            EmailService.SetMessage(email.From, email.ToField, email.Subject, email.Body, email.BccOrNot, email.HtmlOrNot);

            try
            {
                EmailService.SendEmail(email.Password);
            }
            catch (AuthenticationException)
            {
                ViewBag.AuthenticationWarn = true;
                ViewBag.SentSuccessfully = false;

                return View();
            }
            
            ViewBag.AuthenticationWarn = false;
            ViewBag.SentSuccessfully = true;
            
            return View();
        }
    }
}
