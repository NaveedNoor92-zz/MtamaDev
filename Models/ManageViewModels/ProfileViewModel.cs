using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Mtama.Models.ManageViewModels
{
    public class ProfileViewModel
    { 
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [StringLength(50)]
        [Display(Name = "Farmer Form Id Number")]
        public string Farmer_Id_Form_No { get; set; }

        [StringLength(11)]
        [Display(Name = "Kra Pin")]
        public string KraPin { get; set; }

        //[Required]
        //[StringLength(50)]
        //public string Gender { get; set; }

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
        [Display(Name = "Mpesa Number")]
        public string Mpesa_No { get; set; }

        [StringLength(50)]
        [Display(Name = "Whatsapp Number")]
        public string Whatapp_No { get; set; }

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
        [Display(Name = "Wallet Address")]
        public string WalletAddress { get; set; }


        [StringLength(50)]
        [Display(Name = "Acreage")]
        public string Acreage { get; set; }

      
        [Display(Name = "Field Coordinates")]
        public string Field_Coords { get; set; }

        [StringLength(50)]
        [Display(Name = "Crop")]
        public string Crop { get; set; }

        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        public string StatusMessage { get; set; }


        [Display(Name = "Profile Picture")]
        public string ProfilePicture { get; set; }

        public string Fingerprint { get; set; }

        [Display(Name = "Scanned ID")]
        public string ScannedID { get; set; }

        [Display(Name = "Comment")]
        public string ScannedIDComment { get; set; }

        [NotMapped]
        public string mapCoords { get; set; }
        [NotMapped]
        public string sasToken { get; set; }


        [Display(Name = "Serial Form Number")]
        public string SerialFormNumber { get; set; }

        [Display(Name = "Input Used")]
        public string InputUsed { get; set; }


        //New Fields
        [StringLength(50)]
        [Display(Name = "Field Pin")]
        public string Field_pin { get; set; }

        [StringLength(50)]
        [Display(Name = "Input Service")]
        public string Input_Service { get; set; }

        [StringLength(50)]
        [Display(Name = "Planting Date")]
        public string PlantingDate { get; set; }

        [Display(Name = "National ID Number")]
        public string NationalIDNumber { get; set; }

        [Display(Name = "Household Size")]
        public string Household_Size { get; set; }

        [Display(Name = "Economic Activity")]
        public string Economic_Activity { get; set; }

        [Display(Name = "Yield Quantity")]
        public int Yield_Quantity { get; set; }

        [StringLength(50)]
        [Display(Name = "Planting Season")]
        public string Planting_Season { get; set; }

        [StringLength(50)]
        [Display(Name = "Disability Type")]
        public string Disability_Type { get; set; }

        [StringLength(100)]
        [Display(Name = "Aggregator Company")]
        public string Aggregator_Company { get; set; }

        [StringLength(100)]
        [Display(Name = "supplier Company")]
        public string supplier_Company { get; set; }

        [StringLength(50)]
        [Display(Name = "Company Registration Number")]
        public string Company_Registration_Number { get; set; }



        [StringLength(50)]
        [Display(Name = "Aggregator Representative Phone Number")]
        public string AggregatorRepresentativePhoneNumber { get; set; }


        [StringLength(50)]
        [Display(Name = "supplier Representative Phone Number")]
        public string supplierRepresentativePhoneNumber { get; set; }












    }
}
