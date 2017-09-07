using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Korero.Models.ApiDtos
{
    public class NewThreadDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public int TagId { get; set; }
    }
}