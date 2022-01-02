using System;
using System.ComponentModel.DataAnnotations;

namespace MassMailWeb.Models
{
    public class EmailDb
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(320)]
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime When { get; set; }
    }
}
