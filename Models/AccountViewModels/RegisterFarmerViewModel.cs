using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Mtama.Models
{
    public class RegisterFarmerViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "National Id")]
        public string NationalId { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        [Display(Name = "Mobile Number")]
        public string MobileNumber { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [RegularExpression("^(?=.{8,})(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^&+=]).*$",
            ErrorMessage = "Passwords must be at least 8 characters and contain the following:<br/>" +
                            " -Password lenght should be atleast 8. <br/>" +
                            " -Atleast one Number. <br/>" +
                            " -Atleast one Capital Alphabet. <br/>" +
                            " -Atleast one Small Alphabet. <br/>" +
                            " -Atleast one Special Character.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
        //[RegularExpression("^(?=.{8,})(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^&+=]).*$", 
        //    ErrorMessage = "Invalid password format <br/>" +
        //                    " -Password lenght should be atleast 8. <br/>" +
        //                    " -Atleast one Number. <br/>" +
        //                    " -Atleast one Capital Alphabet. <br/>" +
        //                    " -Atleast one Special Character.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
