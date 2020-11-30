using Mtama.Data;
using Mtama.Models;
using Mtama.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mtama.Managers
{
    public class ProfileManager
    {
        public static ProfileViewModelVM Profile(ApplicationDbContext _context, ApplicationUser1ModelVM user, string StatusMessage)
        {
            var Model = new ProfileViewModelVM();
            var mapCoord = "";
            var markers = _context.Markers.Where(u => u.UserId == user.applicationUser.Id).SingleOrDefault();
            if (markers != null)
            {

                Model.Markers = markers.LatLng;
                mapCoord = markers.LatLng;
            }
            else
            {
                Model.Markers = "Empty";
            }

            var model = new ProfileViewModel
            {
                FirstName = user.applicationUser.FirstName,
                LastName = user.applicationUser.LastName,
                // Farmer_Id_Form_No = user.Farmer_Id_Form_No,
                Dob = user.applicationUser.Dob,
                Username = user.applicationUser.UserName,
                County = user.applicationUser.County,
                SubCounty = user.applicationUser.SubCounty,
                Ward = user.applicationUser.Ward,
                PhoneNumber = user.applicationUser.PhoneNumber,
                Mpesa_No = user.applicationUser.Mpesa_No,
                //  Gender = user.Gender,
                KraPin = user.applicationUser.KraPin,
                Whatapp_No = user.applicationUser.Whatapp_No,
                Bank_Info = user.applicationUser.Bank_Info,
                Next_of_Kin = user.applicationUser.Next_of_Kin,
                Next_of_Kin_Contact = user.applicationUser.Next_of_Kin_Contact,
                Acreage = user.applicationUser.Acreage,
                Field_Coords = mapCoord,
                Crop = user.applicationUser.Crop,
                Email = user.applicationUser.Email,
                IsEmailConfirmed = user.applicationUser.EmailConfirmed,
                WalletAddress = user.applicationUser.WalletAddress,
                StatusMessage = StatusMessage,
                NationalIDNumber = user.applicationUser.NationalIDNumber,
                SerialFormNumber = user.applicationUser.SerialFormNumber,
                InputUsed = user.applicationUser.InputUsed,
                PlantingDate = user.applicationUser.PlantingDate,
                Aggregator_Company = user.applicationUser.Aggregator_Company,
                Company_Registration_Number = user.applicationUser.CompanyRegistrationNumber,
                Field_pin = user.applicationUser.Field_Pin,
                Economic_Activity = user.applicationUser.Economic_Activity,
                Household_Size = user.applicationUser.Household_Size,
                Disability_Type = user.applicationUser.DisabilityType,
                Farmer_Id_Form_No = user.applicationUser.Farmer_Id_Form_No,
                Input_Service = user.applicationUser.InputService,
                supplier_Company = user.applicationUser.supplier_Company
            };

            Model.profileViewModel = model;

            return Model;

        }


        public static ProfileViewModelVM Attachments(ApplicationUser user, string StatusMessage)
        {
            var Model = new ProfileViewModelVM();

            Model.BlobUri = ConfigurationManager.GetAppSetting("BlobUri");
            Model.SAS = ConfigurationManager.GetAppSetting("SAS");
            Model.Container = ConfigurationManager.GetAppSetting("Container");
            
            Model.profileViewModel = new ProfileViewModel
            {
                ProfilePicture = user.ProfilePicture,
                Fingerprint = user.Fingerprint,
                ScannedID = user.ScannedID,
                ScannedIDComment = user.ScannedIDComment,
                StatusMessage = StatusMessage,
            };


            return Model;

        }




        








    }
}
