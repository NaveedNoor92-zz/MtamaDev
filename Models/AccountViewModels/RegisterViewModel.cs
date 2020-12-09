using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Mtama.Models
{
    public class RegisterViewModel
    {

        public int Id { get; set; }


        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; }


        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }


        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }


        [Required]
        [Display(Name = "National ID")]
        public string NationalId { get; set; }


        [Required]
        public string Gender { get; set; }

        [Required]
        [Display(Name = "Mobile Number")]
        public string MobileNumber { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Farmer Id Form No")]
        public string Farmer_Id_Form_No { get; set; }

        [Required]
        [Display(Name = "Disability Type")]
        public string DisabilityType { get; set; }

        [Required]
        [Display(Name = "Supplier Company")]
        public string supplier_Company { get; set; }

        [Required]
        [Display(Name = "Aggregator Company")]
        public string Aggregator_Company { get; set; }

        [Required]
        [Display(Name = "Aggregator ID")]
        public string AggregatorID { get; set; }

        [Required]
        [Display(Name = "Company Registration Number")]
        public string CompanyRegistrationNumber { get; set; }

        [Required]
        [Display(Name = "Aggregator Representative Phone Number")]
        public string AggregatorRepresentativePhoneNumber { get; set; }


        [Required]
        [Display(Name = "Supplier Representative Phone Number")]
        public string supplierRepresentativePhoneNumber { get; set; }






        [Required]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$",
            ErrorMessage = "Passwords must be at least 8 characters and contain the following:<br/>" +
                            " -Password lenght should be atleast 8. <br/>" +
                            " -Atleast one Number. <br/>" +
                            " -Atleast one Capital Alphabet. <br/>" +
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

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
