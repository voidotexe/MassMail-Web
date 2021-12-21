using Microsoft.AspNetCore.Http;

namespace MassMailWeb.Models
{
    public static class Cookies
    {
        public static string From { get; set; }
        public static string To { get; set; }
        public static string Subject { get; set; }
        public static string Body { get; set; }
    }
}
