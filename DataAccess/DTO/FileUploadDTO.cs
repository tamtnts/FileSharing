using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class FileUploadDTO
    {
        public string FileName { get; set; }
        public bool AutoDelete { get; set; }
    }
}
