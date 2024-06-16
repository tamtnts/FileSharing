using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Text
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public string TextContent { get; set; }
        public bool AutoDelete { get; set; }
        public int AccessCount { get; set; }
    }
}
