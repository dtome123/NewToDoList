using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewToDoList.Models
{
    public class LoginModel
    {
        [Required]
        public string id { set; get; }
        public string password { set; get; }
        public bool remember { set; get; }
    }
}