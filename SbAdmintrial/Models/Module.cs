using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace SbAdmintrial.Models
{
    public class Module
    {
        [Display(Name = "Course Author:")]
        [Required(ErrorMessage = "Field is Required")]
        public int idmodule { set; get;}

        [Display(Name = "Module Name:")]
        [Required(ErrorMessage = "Field is Required")]
        public string modulename { set; get;}

        [Display(Name = "Module Description")]
        [Required(ErrorMessage = "Field is Required")]
        public string moduledesc { set; get; }

        [Display(Name = "Course id:")]
        [Required(ErrorMessage = "Field is Required")]
        public int courseid { set; get;}
    }
}