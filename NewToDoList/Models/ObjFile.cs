using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace NewToDoList.Models
{
    public class ObjFile
    {
        public IEnumerable<HttpPostedFileBase> files { get; set; }

        public string File { get; set; }
        public long Size { get; set; }
        public string Type { get; set; }
        [DisplayName("Nhân viên up")]
        public string nv { get; set; }
    }
}