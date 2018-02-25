using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
        [RegularExpression("#(?:[0-9a-fA-F]{3}){1,2}")]
        public string Color { get; set; } // Hex color
    }
}
