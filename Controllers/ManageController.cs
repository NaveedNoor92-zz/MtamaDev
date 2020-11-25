using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mtama.Data;
using Mtama.Models;
using Mtama.Models.HomeViewModels;
using Mtama.Models.ManageViewModels;
using Mtama.Services;

namespace Mtama.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class ManageController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly UrlEncoder _urlEncoder;

        private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";
        private const string RecoveryCodesKey = nameof(RecoveryCodesKey);

        public ManageController(
          ApplicationDbContext context,
          UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager,
          IEmailSender emailSender,
          ILogger<ManageController> logger,
          UrlEncoder urlEncoder)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _urlEncoder = urlEncoder;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [HttpGet]
        [Authorize(Roles = "Super Admin,Admin,Aggregator,Farmer,supplier")]
        public async Task<IActionResult> Profile(string id)
        {
            ApplicationUser  user = null;
            if (id != null)
            {
                user = _context.Users.Where(u => u.Id == id).SingleOrDefault();
                var roles = await _userManager.GetRolesAsync(user);
                ViewBag.UserRole1 = roles[0];
                ViewBag.AdminEdit = true;
            }
            else 
            {
                user = await _userManager.GetUserAsync(User);
                var roles = await _userManager.GetRolesAsync(user);
                ViewBag.UserRole1 = roles[0];

            }

            var mapCoord = "";
            var markers = _context.Markers.Where(u => u.UserId == user.Id).SingleOrDefault();
            if (markers != null)
            {

                ViewBag.Markers = markers.LatLng;
                mapCoord = markers.LatLng;
            }
            else
            {
                ViewBag.Markers = "Empty";
            }
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var model = new ProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                // Farmer_Id_Form_No = user.Farmer_Id_Form_No,
                Dob = user.Dob,
                Username = user.UserName,
                County = user.County,
                SubCounty = user.SubCounty,
                Ward = user.Ward,
                PhoneNumber = user.PhoneNumber,
                Mpesa_No = user.Mpesa_No,
                //  Gender = user.Gender,
                KraPin = user.KraPin,
                Whatapp_No = user.Whatapp_No,
                Bank_Info = user.Bank_Info,
                Next_of_Kin = user.Next_of_Kin,
                Next_of_Kin_Contact = user.Next_of_Kin_Contact,
                Acreage = user.Acreage,
                Field_Coords = mapCoord,
                Crop = user.Crop,
                Email = user.Email,
                IsEmailConfirmed = user.EmailConfirmed,
                WalletAddress = user.WalletAddress,
                StatusMessage = StatusMessage,
                NationalIDNumber = user.NationalIDNumber,
                SerialFormNumber = user.SerialFormNumber,
                InputUsed = user.InputUsed,
                PlantingDate = user.PlantingDate,
                Aggregator_Company = user.Aggregator_Company,
                Company_Registration_Number = user.CompanyRegistrationNumber,
                Field_pin = user.Field_Pin,
                Economic_Activity = user.Economic_Activity,
                Household_Size = user.Household_Size,
                Disability_Type = user.DisabilityType,
                Farmer_Id_Form_No = user.Farmer_Id_Form_No,
                Input_Service = user.InputService,
                supplier_Company = user.supplier_Company

            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Super Admin,Admin,Aggregator,Farmer,supplier")]
        public async Task<IActionResult> Profile(ProfileViewModel model , string id)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ApplicationUser user = null;
            if (id != null)
            {
                user = _context.Users.Where(u => u.Id == id).SingleOrDefault();
            }
            else
            {
                user = await _userManager.GetUserAsync(User);
            }
           
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            //Only allow unique wallet addresses
            if (model.WalletAddress != null)
            {
                var existingWallets = _context.Users.Where(p => p.WalletAddress == model.WalletAddress).ToList();
                if (existingWallets.Count != 0)
                {
                    if (existingWallets.Count > 1)
                    {
                        StatusMessage = "Error: This wallet address is already in use.Please use a different Wallet Address";
                        return RedirectToAction(nameof(Profile));
                        // throw new ApplicationException("This wallet address is already in use. Please use a different Wallet Address");              
                    }
                }
            }
            //if (model.PhoneNumber != null)
            //{
            //    var existingphonenumber = _context.Users.Where(p => p.PhoneNumber == model.PhoneNumber).ToList();
            //    if (existingphonenumber.Count != 0)
            //    {
            //        if (existingphonenumber.Count > 1)
            //        {
            //            StatusMessage = "Error: This Phone Number is already in use. Please use a different Phone Number.";
            //            return RedirectToAction(nameof(Profile));
            //            // throw new ApplicationException("This wallet address is already in use. Please use a different Wallet Address");              
            //        }          
            //    }
            //    var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber);
            //    user.PhoneNumber = model.PhoneNumber;
            //    user.UserName = model.PhoneNumber;

            //}
            //else
            //{
            //    StatusMessage = "Error: Phone Number cannot be empty";
            //    return RedirectToAction(nameof(Profile));
            //}


            #region DataAssignment


            if (model.FirstName != user.FirstName && model.FirstName != null) 
            {
                user.FirstName = model.FirstName;
            }

            if (model.LastName != user.LastName && model.LastName != null)
            {
                user.LastName = model.LastName;
            }

            var email = user.Email;
            if (model.Email != email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, model.Email);
                if (!setEmailResult.Succeeded)
                {
                    StatusMessage = "Error: This Email address is already in use.Please use a different Email Address";
                    return RedirectToAction(nameof(Profile));
                }
            }

            if (model.Username != user.UserName)
            {
                user.UserName = model.Username;
            }

            if (model.PhoneNumber != user.PhoneNumber)
            {
                user.PhoneNumber = model.PhoneNumber;
            } 

            if (model.Farmer_Id_Form_No != user.Farmer_Id_Form_No)
            {
                user.Farmer_Id_Form_No = model.Farmer_Id_Form_No;
            }
            if (user.Dob != model.Dob)
            {
                user.Dob = model.Dob;
            }

            if (model.County != user.County)
            {
                user.County = model.County;
            }

            if (model.SubCounty != user.SubCounty)
            {
                user.SubCounty = model.SubCounty;
            }

            if (model.Ward != user.Ward)
            {
                user.Ward = model.Ward;
            }

            if (model.Mpesa_No != user.Mpesa_No)
            {
                user.Mpesa_No = model.Mpesa_No;
            }

            if (model.Whatapp_No != user.Whatapp_No)
            {
                user.Whatapp_No = model.Whatapp_No;
            }

            if (model.Bank_Info != user.Bank_Info)
            {
                user.Bank_Info = model.Bank_Info;
            }


            if (model.KraPin != user.KraPin)
            {
                user.KraPin = model.KraPin;
            }

            if (model.Next_of_Kin != user.Next_of_Kin)
            {
                user.Next_of_Kin = model.Next_of_Kin;
            }

            if (model.Next_of_Kin_Contact != user.Next_of_Kin_Contact)
            {
                user.Next_of_Kin_Contact = model.Next_of_Kin_Contact;
            }

            if (model.Acreage != user.Acreage)
            {
                user.Acreage = model.Acreage;
            }

            if (model.mapCoords != user.Field_Coords)
            {
                user.Field_Coords = model.mapCoords;
            }

            if (model.Crop != user.Crop)
            {
                user.Crop = model.Crop;
            }

            if (model.WalletAddress != user.WalletAddress)
            {
                user.WalletAddress = model.WalletAddress;
            }

            if (model.NationalIDNumber != user.NationalIDNumber)
            {
                user.NationalIDNumber = model.NationalIDNumber;
            }

            if (model.SerialFormNumber != user.SerialFormNumber)
            {
                user.SerialFormNumber = model.SerialFormNumber;
            }

            if (model.InputUsed != user.InputUsed)
            {
                user.InputUsed = model.InputUsed;
            }

            if (model.PlantingDate != user.PlantingDate)
            {
                user.PlantingDate = model.PlantingDate;
            }


            if (model.Aggregator_Company != user.Aggregator_Company)
            {
                user.Aggregator_Company = model.Aggregator_Company;
            }

            if (model.Company_Registration_Number != user.CompanyRegistrationNumber)
            {
                user.CompanyRegistrationNumber = model.Company_Registration_Number;
            }


            if (model.Field_pin != user.Field_Pin)
            {
                user.Field_Pin = model.Field_pin;
            }

            if (model.Economic_Activity != user.Economic_Activity)
            {
                user.Economic_Activity = model.Economic_Activity;
            }

            if (model.Household_Size != user.Household_Size)
            {
                user.Household_Size = model.Household_Size;
            }

            if (model.Disability_Type != user.DisabilityType)
            {
                user.DisabilityType = model.Disability_Type;
            }


            if (model.Farmer_Id_Form_No != user.Farmer_Id_Form_No)
            {
                user.Farmer_Id_Form_No = model.Farmer_Id_Form_No;
            }
            if (model.Input_Service != user.InputService)
            {
                user.InputService = model.Input_Service;
            }

            if (model.supplier_Company != user.supplier_Company)
            {
                user.supplier_Company = model.supplier_Company;
            }


            

         

            #endregion

            await _userManager.UpdateAsync(user);

            var markers = _context.Markers.Where(u => u.UserId == user.Id).SingleOrDefault();

            if (markers != null)
            {
                markers.LatLng = model.mapCoords;
                await _context.SaveChangesAsync();
            }
            else
            {

                var area = new MarkerModel();
                area.UserId = user.Id;
                area.LatLng = model.mapCoords;
                _context.Add(area);
                _context.SaveChanges();
            }




            StatusMessage = "Your profile has been updated";
            return RedirectToAction(nameof(Profile), new { id = id });
        }



        [HttpGet]
        public async Task<IActionResult> Attachments()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            ViewData["bloburi"] = ConfigurationManager.GetAppSetting("BlobUri");
            ViewData["sas"] = ConfigurationManager.GetAppSetting("SAS");
            ViewData["container"] = ConfigurationManager.GetAppSetting("Container");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var model = new ProfileViewModel
            {
                ProfilePicture = user.ProfilePicture,
                Fingerprint = user.Fingerprint,
                ScannedID = user.ScannedID,
                ScannedIDComment = user.ScannedIDComment,
                StatusMessage = StatusMessage,
                sasToken = ConfigurationManager.GetAppSetting("SAS")
        };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Attachments(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }


            if (model.ProfilePicture != user.ProfilePicture)
            {
                user.ProfilePicture = model.ProfilePicture;
            }

            if (model.Fingerprint != user.Fingerprint)
            {
                user.Fingerprint = model.Fingerprint;
            }

            if (model.ScannedID != user.ScannedID)
            {
                user.ScannedID = model.ScannedID ;
            }

            if (model.ScannedIDComment != user.ScannedIDComment)
            {
                user.ScannedIDComment = model.ScannedIDComment;
            }

            await _userManager.UpdateAsync(user);
            StatusMessage = "Your Data has been Uploaded.";
            return RedirectToAction(nameof(Attachments));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendVerificationEmail(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
            var email = user.Email;
            await _emailSender.SendEmailConfirmationAsync(email, callbackUrl);

            StatusMessage = "Verification email sent. Please check your email.";
            return RedirectToAction(nameof(Profile));
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (!hasPassword)
            {
                return RedirectToAction(nameof(SetPassword));
            }

            var model = new ChangePasswordViewModel { StatusMessage = StatusMessage };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                AddErrors(changePasswordResult);
                return View(model);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            _logger.LogInformation("User changed their password successfully.");
            StatusMessage = "Your password has been changed.";

            return RedirectToAction(nameof(ChangePassword));
        }

        [HttpGet]
        public async Task<IActionResult> SetPassword()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var hasPassword = await _userManager.HasPasswordAsync(user);

            if (hasPassword)
            {
                return RedirectToAction(nameof(ChangePassword));
            }

            var model = new SetPasswordViewModel { StatusMessage = StatusMessage };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var addPasswordResult = await _userManager.AddPasswordAsync(user, model.NewPassword);
            if (!addPasswordResult.Succeeded)
            {
                AddErrors(addPasswordResult);
                return View(model);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            StatusMessage = "Your password has been set.";

            return RedirectToAction(nameof(SetPassword));
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLogins()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var model = new ExternalLoginsViewModel { CurrentLogins = await _userManager.GetLoginsAsync(user) };
            model.OtherLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync())
                .Where(auth => model.CurrentLogins.All(ul => auth.Name != ul.LoginProvider))
                .ToList();
            model.ShowRemoveButton = await _userManager.HasPasswordAsync(user) || model.CurrentLogins.Count > 1;
            model.StatusMessage = StatusMessage;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LinkLogin(string provider)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            // Request a redirect to the external login provider to link a login for the current user
            var redirectUrl = Url.Action(nameof(LinkLoginCallback));
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, _userManager.GetUserId(User));
            return new ChallengeResult(provider, properties);
        }

        [HttpGet]
        public async Task<IActionResult> LinkLoginCallback()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var info = await _signInManager.GetExternalLoginInfoAsync(user.Id);
            if (info == null)
            {
                throw new ApplicationException($"Unexpected error occurred loading external login info for user with ID '{user.Id}'.");
            }

            var result = await _userManager.AddLoginAsync(user, info);
            if (!result.Succeeded)
            {
                throw new ApplicationException($"Unexpected error occurred adding external login for user with ID '{user.Id}'.");
            }

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            StatusMessage = "The external login was added.";
            return RedirectToAction(nameof(ExternalLogins));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveLogin(RemoveLoginViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var result = await _userManager.RemoveLoginAsync(user, model.LoginProvider, model.ProviderKey);
            if (!result.Succeeded)
            {
                throw new ApplicationException($"Unexpected error occurred removing external login for user with ID '{user.Id}'.");
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            StatusMessage = "The external login was removed.";
            return RedirectToAction(nameof(ExternalLogins));
        }

        [HttpGet]
        public async Task<IActionResult> TwoFactorAuthentication()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var model = new TwoFactorAuthenticationViewModel
            {
                HasAuthenticator = await _userManager.GetAuthenticatorKeyAsync(user) != null,
                Is2faEnabled = user.TwoFactorEnabled,
                RecoveryCodesLeft = await _userManager.CountRecoveryCodesAsync(user),
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Disable2faWarning()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!user.TwoFactorEnabled)
            {
                throw new ApplicationException($"Unexpected error occured disabling 2FA for user with ID '{user.Id}'.");
            }

            return View(nameof(Disable2fa));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Disable2fa()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var disable2faResult = await _userManager.SetTwoFactorEnabledAsync(user, false);
            if (!disable2faResult.Succeeded)
            {
                throw new ApplicationException($"Unexpected error occured disabling 2FA for user with ID '{user.Id}'.");
            }

            _logger.LogInformation("User with ID {UserId} has disabled 2fa.", user.Id);
            return RedirectToAction(nameof(TwoFactorAuthentication));
        }

        [HttpGet]
        public async Task<IActionResult> EnableAuthenticator()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var model = new EnableAuthenticatorViewModel();
            await LoadSharedKeyAndQrCodeUriAsync(user, model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnableAuthenticator(EnableAuthenticatorViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadSharedKeyAndQrCodeUriAsync(user, model);
                return View(model);
            }

            // Strip spaces and hypens
            var verificationCode = model.Code.Replace(" ", string.Empty).Replace("-", string.Empty);

            var is2faTokenValid = await _userManager.VerifyTwoFactorTokenAsync(
                user, _userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

            if (!is2faTokenValid)
            {
                ModelState.AddModelError("Code", "Verification code is invalid.");
                await LoadSharedKeyAndQrCodeUriAsync(user, model);
                return View(model);
            }

            await _userManager.SetTwoFactorEnabledAsync(user, true);
            _logger.LogInformation("User with ID {UserId} has enabled 2FA with an authenticator app.", user.Id);
            var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
            TempData[RecoveryCodesKey] = recoveryCodes.ToArray();

            return RedirectToAction(nameof(ShowRecoveryCodes));
        }

        [HttpGet]
        public IActionResult ShowRecoveryCodes()
        {
            var recoveryCodes = (string[])TempData[RecoveryCodesKey];
            if (recoveryCodes == null)
            {
                return RedirectToAction(nameof(TwoFactorAuthentication));
            }

            var model = new ShowRecoveryCodesViewModel { RecoveryCodes = recoveryCodes };
            return View(model);
        }

        [HttpGet]
        public IActionResult ResetAuthenticatorWarning()
        {
            return View(nameof(ResetAuthenticator));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetAuthenticator()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await _userManager.SetTwoFactorEnabledAsync(user, false);
            await _userManager.ResetAuthenticatorKeyAsync(user);
            _logger.LogInformation("User with id '{UserId}' has reset their authentication app key.", user.Id);

            return RedirectToAction(nameof(EnableAuthenticator));
        }

        [HttpGet]
        public async Task<IActionResult> GenerateRecoveryCodesWarning()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!user.TwoFactorEnabled)
            {
                throw new ApplicationException($"Cannot generate recovery codes for user with ID '{user.Id}' because they do not have 2FA enabled.");
            }

            return View(nameof(GenerateRecoveryCodes));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateRecoveryCodes()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!user.TwoFactorEnabled)
            {
                throw new ApplicationException($"Cannot generate recovery codes for user with ID '{user.Id}' as they do not have 2FA enabled.");
            }

            var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
            _logger.LogInformation("User with ID {UserId} has generated new 2FA recovery codes.", user.Id);

            var model = new ShowRecoveryCodesViewModel { RecoveryCodes = recoveryCodes.ToArray() };

            return View(nameof(ShowRecoveryCodes), model);
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private string FormatKey(string unformattedKey)
        {
            var result = new StringBuilder();
            int currentPosition = 0;
            while (currentPosition + 4 < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition, 4)).Append(" ");
                currentPosition += 4;
            }
            if (currentPosition < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition));
            }

            return result.ToString().ToLowerInvariant();
        }

        private string GenerateQrCodeUri(string email, string unformattedKey)
        {
            return string.Format(
                AuthenticatorUriFormat,
                _urlEncoder.Encode("Mtama"),
                _urlEncoder.Encode(email),
                unformattedKey);
        }

        private async Task LoadSharedKeyAndQrCodeUriAsync(ApplicationUser user, EnableAuthenticatorViewModel model)
        {
            var unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            if (string.IsNullOrEmpty(unformattedKey))
            {
                await _userManager.ResetAuthenticatorKeyAsync(user);
                unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            }

            model.SharedKey = FormatKey(unformattedKey);
            model.AuthenticatorUri = GenerateQrCodeUri(user.Email, unformattedKey);
        }

        [HttpGet]
        public async Task<IActionResult> ViewUsers()
        {
            return View(await _context.Users.ToListAsync());
        }



















        #endregion
    }
}
