using System.ComponentModel.DataAnnotations;

namespace MassMailWeb.Models
{
    public class Email// : IValidatableObject
    {
        [Required]
        [EmailAddress]
        public string From { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ToField { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }
        public string Attachment { get; set; }

        [Display(Name = "Enviar como cópia oculta")]
        public bool BccOrNot { get; set; }

        [Display(Name = "Formatação em HTML")]
        public bool HtmlOrNot { get; set; }
    }
}
