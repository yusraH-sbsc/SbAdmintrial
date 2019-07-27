
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SbAdmintrial.Models
{
    public class Exam
    {


            public int examid { set; get; }
        public int courseid { set; get; }

            [Display(Name = "Course Name:")]
            [Required(ErrorMessage = "Field is Required")]
            public string coursename { set; get; }

            [Display(Name = "Instructions:")]
            [Required(ErrorMessage = "Field is Required")]
            public string examdesc { set; get; }

            [Display(Name = "Date and Time:")]
            [Required(ErrorMessage = "Field is Required")]
            public string date { set; get; }

            [Display(Name = "isActive:")]
            [Required(ErrorMessage = "Field is Required")]
            public int isActive { set; get; }



            



        }
    }
