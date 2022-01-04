/*
 * By: voidotexe
 * https://www.github.com/voidotexe
 */

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

        [HttpPost]
        public IActionResult Index(object obj)
        {
            DbHelper.Truncate();

            return View();
        }
    }
}
