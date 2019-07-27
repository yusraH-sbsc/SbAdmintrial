
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SbAdmintrial.Models
{
    public class User
    {
            public int idUser { set; get; }

            [Display(Name = "First Name:")]
            [Required(ErrorMessage = "please insert your first name")]
            public string FirstName { set; get; }

        [Display(Name = "Last Name:")]
        [Required(ErrorMessage = "please insert your Last name")]
        public string LastName { set; get; }

        [Display(Name = "User Name:")]
        [Required(ErrorMessage = "please insert your Last name")]
        public string username { set; get; }

        [Display(Name = "Email:")]
        [Required(ErrorMessage = "please insert your Email")]
        public string Email { set; get; }

        


        [Display(Name = "Role:")]
            [Required(ErrorMessage = "this field is required")]
            public roletype Role { set; get; }

            [Display(Name = "Password:")]
            [Required(ErrorMessage = "This field is required")]
            [DataType(DataType.Password)]

            public string Password { set; get; }
            [Display(Name = "Confirm Password:")]
            [DataType(DataType.Password)]


            [Compare("Password")]
            public string password_confirm { set; get; }
            public enum roletype
            {
            EMPLOYEE,
                HR_MANAGEMENT,
                IT_STAFF               

            }


        }




    }

