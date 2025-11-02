using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BobaTea.Models
{
    public class User : Controller
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }
}
