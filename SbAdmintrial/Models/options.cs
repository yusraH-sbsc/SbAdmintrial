using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SbAdmintrial.Models
{
    public class options
    {

        


            [Display(Name = "Course Author:")]
            [Required(ErrorMessage = "Field is Required")]
            public int idoptions { set; get; }

        [Display(Name = "Course Author:")]
        [Required(ErrorMessage = "Field is Required")]
        public int questionid { set; get; }

       

        [Display(Name = "Module Name:")]
            [Required(ErrorMessage = "Field is Required")]
            public string isAnswer { set; get; }

            [Display(Name = "Module Description")]
            [Required(ErrorMessage = "Field is Required")]
            public string optA { set; get; }

            [Display(Name = "Module Description")]
            [Required(ErrorMessage = "Field is Required")]
            public string optB { set; get; }

        [Display(Name = "Module Description")]
        [Required(ErrorMessage = "Field is Required")]
        public string optC { set; get; }

        [Display(Name = "Module Description")]
        [Required(ErrorMessage = "Field is Required")]
        public string optD { set; get; }



    }
    }

