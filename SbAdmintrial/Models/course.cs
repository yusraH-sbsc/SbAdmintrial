using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SbAdmintrial.Models
{
    public class course
    {
        public int courseid { set; get; }

        [Display(Name = "Course Name:")]
        [Required(ErrorMessage = "Field is Required")]
        public string coursename { set; get; }

        [Display(Name = "Course Code:")]
        [Required(ErrorMessage = "Field is Required")]
        public string coursecode { set; get; }

        [Display(Name = "Description:")]
        [Required(ErrorMessage = "Field is Required")]
        public string description { set; get; }

        [Display(Name = "Author id:")]
        [Required(ErrorMessage = "Field is Required")]
        public int authorid{ set; get; }

       

        private DateTime? dateCreated;
        [Display(Name = "Date Created")]
        public DateTime date
        {
            get { return dateCreated ?? DateTime.Now; }
            set { dateCreated = value; }
           
           
        }

            
       // public DateTime date { get; private set; }

        [Display(Name = "Duration:")]
        [Required(ErrorMessage = "Field is Required")]
        public string duration { set; get; }

        [Display(Name = "IsActive:")]
        public string isActive { set; get; }


    }
}