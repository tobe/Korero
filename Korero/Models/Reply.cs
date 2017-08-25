using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Korero.Models
{
    public class Reply
    {
        public int ID { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        public string Body { get; set; }
    }
}
