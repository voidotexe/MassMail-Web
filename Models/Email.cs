using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace MassMailWeb.Models
{
    public class Email
    {
        [Required]
        [EmailAddress]
        [MaxLength(320)]
        public string From { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ToField { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }
        public List<IFormFile> Attachments { get; set; }

        [Display(Name = "Enviar como cópia oculta")]
        public bool BccOrNot { get; set; }

        [Display(Name = "Formatação em HTML")]
        public bool HtmlOrNot { get; set; }
    }
}
