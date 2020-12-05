using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewToDoList.code
{
    [Serializable]
    public class UserSession
    {
        public int id { get; set; }
        public string role { get; set; }
    }
}