using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SbAdmintrial.Models
{
    public class courseXuser
    {
        public int Xuserid { set; get; }

        [Display(Name = "User ID:")]
        [Required(ErrorMessage = "please insert your first name")]
        public int userid { set; get; }

        [Display(Name = "Course ID:")]
        [Required(ErrorMessage = "please insert your first name")]
        public int courseid { set; get; }



        [Display(Name = "HR Names:")]
        [Required(ErrorMessage = "please insert your first name")]
        public string username { set; get; }

        
    }
}