using Microsoft.AspNetCore.Mvc;
using MassMailWeb.Models;
using MassMailWeb.Services;
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
        private string currentDirectory;
        private string wwwRoot;
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
            wwwRoot = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads");
            currentDirectory = Path.Combine(Directory.GetCurrentDirectory(), wwwRoot);

            if (email.Attachment.Count > 0)
            {
                foreach (IFormFile attachment in email.Attachment)
                {
                    filePath = Path.Combine(currentDirectory, attachment.FileName);

                    using (FileStream stream = new FileStream(Path.Combine(wwwRoot, attachment.FileName), FileMode.Create))
                    {
                        attachment.CopyTo(stream);
                    }

                    attachments.Add(Path.Combine(currentDirectory, attachment.FileName));
                }
            }

            EmailService.SetMessage(email.From, email.ToField, email.Subject, email.Body, attachments, email.BccOrNot, email.HtmlOrNot);
            EmailService.SendEmail(email.Password);

            System.IO.File.Delete(filePath); // use of namescape System.IO for not getting confused with ControllerBase.File

            ViewBag.SentSuccessfully = true;
            
            return View();
        }
    }
}
