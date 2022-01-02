using Microsoft.AspNetCore.Mvc;
using MassMailWeb.Helpers;

namespace MassMailWeb.Controllers
{
    [Route("/History")]
    public class HistoryController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            DbHelper.Get();

            return View(DbHelper.emailsDb);
        }
    }
}
