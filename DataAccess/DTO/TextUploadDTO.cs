using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class TextUploadDTO
    {
        public string Content { get; set; }
        public bool AutoDelete { get; set; }
        public int UserId { get; set; }
    }
}
