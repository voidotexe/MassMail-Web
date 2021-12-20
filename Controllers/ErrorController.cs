using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;
using System.Net.Sockets;
using MailKit.Security;

namespace MassMailWeb.Controllers
{
    [Route("/Error")]
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            IExceptionHandlerPathFeature exceptionHandler = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionHandler != null)
            {
                if (exceptionHandler.Error is SocketException)
                {
                    ViewBag.ExceptionTitle = "Socket Exception";
                    ViewBag.ExceptionDescription = "Host desconhecido. Verifique se o email está correto";

                    return View();
                }
                else if (exceptionHandler.Error is AuthenticationException)
                {
                    ViewBag.ExceptionTitle = "Authentication Exception";
                    ViewBag.ExceptionDescription = "E-mail ou senha incorretos.\n\nCaso estejam corretos e esteja usando Gmail, então é necessário ativar o acesso a app menos seguro";

                    return View();
                }

                ViewBag.ExceptionTitle = "Exception";
                ViewBag.ExceptionDescription = "Ocorreu algum erro";

                return View();
            }

            ViewBag.ExceptionTitle = "MassMailUnknownException";
            ViewBag.ExceptionDescription = "Não foi possível identificar o erro";

            return View();
        }
    }
}
