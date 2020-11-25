using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Mtama.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [StringLength(50)]
        [Display(Name = "Farmer From Id Number")]
        public string Farmer_Id_Form_No { get; set; }

        [StringLength(50)]
        [Display(Name = "Date of Birth")]
        public string Dob { get; set; }

        [StringLength(50)]
        [Display(Name = "County")]
        public string County { get; set; }

        [StringLength(50)]
        [Display(Name = "SubCounty")]
        public string SubCounty { get; set; }

        [StringLength(50)]
        [Display(Name = "Ward")]
        public string Ward { get; set; }

        [StringLength(50)]
        [Display(Name = "Wallet Address")]
        public string WalletAddress { get; set; }

        [Required]
        [StringLength(50)]
        public string Gender { get; set; }

        [StringLength(50)]
        [Display(Name = "Mpesa Number")]
        public string Mpesa_No { get; set; }

        [StringLength(50)]
        [Display(Name = "Whatsapp Number")]
        public string Whatapp_No { get; set; }

        [StringLength(50)]
        [Display(Name = "Kra Pin")]
        public string KraPin { get; set; }

        [StringLength(50)]
        [Display(Name = "Bank Information")]
        public string Bank_Info { get; set; }

        [StringLength(50)]
        [Display(Name = "Next of Kin")]
        public string Next_of_Kin { get; set; }
        
        [StringLength(50)]
        [Display(Name = "Next of Kin Contact")]
        public string Next_of_Kin_Contact { get; set; }
        
        [StringLength(50)]
        [Display(Name = "Acre Age")]
        public string Acreage { get; set; }

        [Display(Name = "Field Coords")]
        public string Field_Coords { get; set; }

        [Display(Name = "Household Size")]
        public string Household_Size { get; set; }

        [Display(Name = "Economic Activity")]
        public string Economic_Activity { get; set; }

        public string AggregatorID { get; set; }

        [StringLength(50)]
        [Display(Name = "Crop")]
        public string Crop { get; set; }

        public string Fingerprint { get; set; }

        [Display(Name = "Profile Picture")]
        public string ProfilePicture { get; set; }

        [Display(Name = "Scanned ID")]
        public string ScannedID { get; set; }

        [Display(Name = "Comment")]
        public string ScannedIDComment { get; set; }

        [Display(Name = "National ID Number")]
        public string NationalIDNumber { get; set; }

        [Display(Name = "Serial Form Number")]
        public string SerialFormNumber { get; set; }

        [Display(Name = "Input Used")]
        public string InputUsed { get; set; }

        [StringLength(50)]
        [Display(Name = "Planting Date")]
        public string PlantingDate { get; set; }

        [StringLength(50)]
        [Display(Name = "Input Service")]
        public string InputService { get; set; }

        [StringLength(50)]
        [Display(Name = "Yield Quantity")]
        public string YieldQuantity { get; set; }

        [StringLength(50)]
        [Display(Name = "Planting Season")]
        public string PlantingSeason { get; set; }

        [StringLength(50)]
        [Display(Name = "Disability Type")]
        public string DisabilityType { get; set; }

        [StringLength(100)]
        [Display(Name = "Aggregator Company")]
        public string Aggregator_Company { get; set; }

        [StringLength(100)]
        [Display(Name = "supplier Company")]
        public string supplier_Company { get; set; }

        [StringLength(50)]
        [Display(Name = "Company Registration Number")]
        public string CompanyRegistrationNumber { get; set; }

        [StringLength(50)]
        [Display(Name = "Field Pin")]
        public string Field_Pin { get; set; }
        
        [StringLength(50)]
        [Display(Name = "Aggregator Representative Phone Number")]
        public string AggregatorRepresentativePhoneNumber { get; set; }


        [StringLength(50)]
        [Display(Name = "supplier Representative Phone Number")]
        public string supplierRepresentativePhoneNumber { get; set; }


        public string UserRole { get; set; }
        public bool AggregatorLinked { get; set; }




    }
}
