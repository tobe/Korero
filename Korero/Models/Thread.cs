using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Korero.Models
{
    public class Thread
    {
        public int ID { get; set; }

        [Required]
        [StringLength(64)]
        public string Title { get; set; }

        public DateTime DateCreated { get; set; }
        public int Views { get; set; }

        public ApplicationUser Author { get; set; }
        public List<Reply> Replies { get; set; }
        public Tag Tag { get; set; }
    }
}
