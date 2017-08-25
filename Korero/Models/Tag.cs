using System;
using System.ComponentModel.DataAnnotations;

namespace Korero.Models
{
    public class Tag
    {
        public int ID { get; set; }
        [Required]
        [StringLength(32)]
        public string Label { get; set; }

        [Required]
        [StringLength(7)]
        public string Color { get; set; } // Hex color
    }
}
