using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Korero.Models
{
    public class Reply
    {
        public int ID { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        public ApplicationUser Author { get; set; }

        [Required]
        public string Body { get; set; }

        // Manual inverse navigation -- literally the foreign key.
        // This is needed because of a workaround -> https://stackoverflow.com/questions/44519325/entity-framework-core-selectmany-then-include
        // Although, this isn't bad practice tbh
        [ForeignKey("ThreadID")] // The one down there ˇ
        public Thread Thread { get; set; }
        public int ThreadID { get; set; }
    }
}
