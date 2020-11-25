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
using Mtama.Controllers;
using Mtama.Data;
using Mtama.Models;
using Mtama.Models.AccountViewModels;
using Mtama.Models.HomeViewModels;
using Mtama.Models.ManageViewModels;
using Mtama.Services;
using RexMercury.Models.ManageViewModels;


namespace Mtama.Controllers
{
    public class UserController : Controller
    {
        private RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
      
        public UserController(
          ApplicationDbContext context,
          UserManager<ApplicationUser> userManager,
          RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Authorize(Roles = "Super Admin, Admin, Aggregator")]
        public async Task<IActionResult> ViewUsers()
        {
            
            return View("/Views/User/ViewUsers.cshtml", await _context.Users.ToListAsync());
        }

        public async Task<IActionResult> AddUser()
        {
            return View("/Views/Account/Register.cshtml");
        }

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> AddUser(RegisterViewModel model, string returnUrl = null)
        //{
        //    ViewData["ReturnUrl"] = returnUrl;
        //    try
        //    {

        //        if (ModelState.IsValid)
        //        {
        //            var user = new ApplicationUser
        //            {
        //                FirstName = model.FirstName,
        //                LastName = model.LastName,
        //                Farmer_Id_Form_No = model.NationalId,
        //                //  Dob = model.Dob,       
        //                Gender = model.Gender,
        //                //  KraPin = model.KraPin,
        //                PhoneNumber = model.MobileNumber,
        //                Email = model.Email,
        //                UserName = model.MobileNumber
        //            };

        //            var result = await _userManager.CreateAsync(user, model.Password);
        //            if (result.Succeeded)
        //            {
        //                _logger.LogInformation("User created a new account with password.");

        //                //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        //                //var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
        //                //await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);

        //                //await _signInManager.SignInAsync(user, isPersistent: false);
        //                _logger.LogInformation("User created a new account with password.");
        //                //                    return RedirectToLocal(returnUrl);
        //                return RedirectToAction("ViewUsers", "User", new { area = "" });
        //            }
        //            //AddErrors(result);
        //            //  ViewData["Error"] = result.Errors.FirstOrDefault().Description;
        //            ViewData["Error"] = " The Phonenumber you entered is already taken. Please enter a different one.";
        //            return View(model);
        //        }
        //        // If we got this far, something failed, redisplay form
        //        return View(model);
        //        //   // return RedirectToAction("HOmeIndex");
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewData["Error"] = ex.Message;
        //        return View(model);
        //    }


        //}

        [Authorize(Roles = "Super Admin")]
        [HttpGet] 
        public async Task<IActionResult> ManageUserRoles(string id)
        {
            
            ViewBag.userId = id;
            var user = await _userManager.FindByIdAsync(id);

            if(user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }
            var model = new List<UserRoleViewModel>();
            foreach (var role in _roleManager.Roles) {

                var userRoleViewModel = new UserRoleViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else {
                    userRoleViewModel.IsSelected = false;
                }
                model.Add(userRoleViewModel);
            }

            return View(model);
        }

        [Authorize(Roles = "Super Admin")]
        [HttpPost]
        public async Task<IActionResult> ManageUserRoles(List<UserRoleViewModel> model, string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }

            result = await _userManager.AddToRolesAsync(user, model.Where(x => x.IsSelected).Select(y=> y.RoleName));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add user existing roles");
                return View(model);
            }

            return RedirectToAction("ManageUserRoles", new {id = id });
        }
    }
}
