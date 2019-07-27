using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SbAdmintrial.Models
{
    public class Question
    {
       
            [Display(Name = "Course Author:")]
            [Required(ErrorMessage = "Field is Required")]
            public int idQuestion { set; get; }

            [Display(Name = "Module Name:")]
            [Required(ErrorMessage = "Field is Required")]
            public string QuestionNo { set; get; }

            [Display(Name = "Module Description")]
            [Required(ErrorMessage = "Field is Required")]
            public string Qstatement { set; get; }

        [Display(Name = "Module Description")]
        [Required(ErrorMessage = "Field is Required")]
        public string QPoints { set; get; }


        [Display(Name = "Course id:")]
            [Required(ErrorMessage = "Field is Required")]
            public int courseid { set; get; }
        }
    }
