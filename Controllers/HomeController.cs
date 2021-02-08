using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mtama.Models;
using Microsoft.EntityFrameworkCore;
using Mtama.Data;
using System;
using System.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Mtama.Services;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.IO;
using X.PagedList;
using Microsoft.AspNetCore.Http;
using Mtama.Managers;
using Mtama.VM;

namespace Mtama.Controllers
{
    [Authorize]
    // [EnableCors("AllowAzureWebsitesOrigin")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;


        public HomeController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<AccountController> logger)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetImage()
        {
            var user = await _userManager.GetUserAsync(User);

            return View();
        }

        // GET: PaymentsViewModels
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(user);
            ViewBag.UserRole = roles[0];
            return View();

            
        }

        [Authorize(Roles = "Super Admin , Admin")]
        [HttpGet]
        public IActionResult MakePayments(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var pm = PaymentManager.MakePayments();

            return View(pm);
        }

        [Authorize(Roles = "Super Admin , Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MakePayments(PaymentModel model, string returnUrl = null)
        {
            try
            {
                ViewData["ReturnUrl"] = returnUrl;
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                model = PaymentManager.MakePayments(_context, model, userId);
                
                ViewData["ShowVerify"] = true;
                return RedirectToAction("ViewTransaction", new { id = model.Id });                
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "There was an error in saving your transaction. " + ex.Message;
                return View();
            }

        }




        public async Task<IActionResult> MakePaymentFromAdmin(string PaymentId, string ReceiverId)
        {
            try
            {
                var model = new PaymentModel();
                //var user = await _userManager.FindByIdAsync(ReceiverId);
                //var modeldata = _context.Payments.Where(p => p.Id == Convert.ToInt64(PaymentId)).Include(p => p.Sender).Include(p => p.Receiver).FirstOrDefault();

                //var receiver = _context.Users.Where(u => u.WalletAddress == user.WalletAddress).SingleOrDefault();

                //var model = new PaymentModel();

                //var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                var getPaymentData = await PaymentManager.MakePaymentFromAdmin(_context, PaymentId);

                model.ReceiverId = getPaymentData.ReceiverId;
                model.ReceiverWallet = getPaymentData.ReceiverWallet;
                model.ReceiverName = getPaymentData.ReceiverName;
                model.SenderIdNew = getPaymentData.SenderIdNew;
                model.TimeStamp = DateTime.UtcNow;
                model.TxStatus = Common.TxStatusPending;
                model.TxGuid = Guid.NewGuid().ToString();

                return View(model);



                //return RedirectToAction("ViewTransaction", new { id = PaymentId});
                //return View();
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "There was an error in saving your transaction. " + ex.Message;
                return RedirectToAction("ViewNotifications");
            }

        }




        [HttpGet]
        public async Task<IActionResult> ViewTransaction(int id)
        {


            //if (User.IsInRole("Admin") || (User.IsInRole("Super Admin")))
            //{
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var payment = PaymentManager.ViewTransaction(_context, id, userId);
                var sender =  await _userManager.FindByIdAsync(payment.paymentModel.SenderIdNew);
                payment.paymentModel.Sender = sender;
                return View(payment);

            //}
            //else
            //{
            //    //throw new Exception("You are not authorized to view this payment");

            //    return RedirectToAction("AccessDenied", "Account");
            //}



        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ViewTransaction(PaymentModelVM model, string returnUrl = null)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var returnmodel = PaymentManager.ViewTransaction(_context, model, userId);

                //if (!User.IsInRole("Admin")) { 
                //    throw new Exception("Only Transaction Initiator can verify transaction");
                //}

                ViewData["ShowVerify"] = false;
                return RedirectToAction("ViewTransaction", new { id = returnmodel.paymentModel.Id });
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "There was an error in Viewing transaction. " + ex.Message;
                return RedirectToAction("ViewTransaction", new { id = model.Id });
            }
        }




        [Authorize(Roles = "Aggregator, Admin, supplier, Super Admin, Farmer")]
        [HttpGet]
        public async Task<IActionResult> ViewPayments(string searchString, string sortOrder, string currentFilter, int? page)
        {
            return View(await GetPayments());
        }

        [Authorize(Roles = "Aggregator, Super Admin")]
        [HttpGet]
        public async Task<IActionResult> ViewPaymentsLinkedFarmers()
        {
            return View(await GetPaymentsLinkedFarmers());
        }


        public ActionResult SearchUser(string term)
        {
            var names = _context.Users.Where(p => p.FirstName.Contains(term) || p.LastName.Contains(term)).Select(p => (p.FirstName + " " + p.LastName + " (" + p.WalletAddress + ")")).ToList();
            names.RemoveAll(s => String.IsNullOrEmpty(s));

            return Json(names);
        }

        [HttpPost]
        public JsonResult MakePayments1(string Prefix)
        {
            var UsersList = (from N in _context.Users.ToList()
                             where N.FirstName.StartsWith(Prefix)
                             select new { N.FirstName });
            return Json(UsersList);
        }

        private async Task<List<PaymentModel>> GetPayments()
        {
            if (User.IsInRole("Admin") || (User.IsInRole("Super Admin")))
            {
                var payment = await PaymentManager.GetPayments(_context);
                foreach (var item in payment)
                {
                    var sender = await _userManager.FindByIdAsync(item.SenderIdNew);
                    item.Sender = sender;
                }

                return payment;

            }
            else
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                var payment = await PaymentManager.GetPayments(_context, userId);
                foreach (var item in payment)
                {
                    var sender = await _userManager.FindByIdAsync(item.SenderIdNew);
                    item.Sender = sender;
                }

                return payment;
            }
        }


        private async Task<List<PaymentModel>> GetPaymentsLinkedFarmers()
        {
            try
            {
                var payment = new List<PaymentModel>();
                if (User.IsInRole("Aggregator"))
                {
                    var aggregator = await _userManager.GetUserAsync(User);
                    var data = _context.AggregatorAssociations.Where(u => u.AggregatorId == aggregator.Id).ToList();

                    foreach (var item1 in data)
                    {
                        var temp = await PaymentManager.GetPayments(_context, Convert.ToString(item1.FarmerId));
                        foreach (var item in temp)
                        {
                            var sender = await _userManager.FindByIdAsync(item.SenderIdNew);
                            item.Sender = sender;
                            payment.Add(item);
                        }
                    }
                    return payment;
                }
                else
                {
                    return payment;
                }


            }
            catch (Exception ex)
            {
                return new List<PaymentModel>();

            }
        }





        [Authorize]
        [HttpGet]
        [Route("Home/Payments.csv")]
        [Produces("text/csv")]
        public async Task<IActionResult> GetPaymentsAsCsv()
        {
            var payments = await GetPayments();
            List<PaymentCSVModel> lstModel = new List<PaymentCSVModel>();
            foreach (var p in payments)
            {
                lstModel.Add(new PaymentCSVModel(p));
            }
            return Ok(lstModel);
        }




        [Authorize(Roles = "Super Admin,Aggregator")]
        public async Task<IActionResult> ViewFarmers(string StatusMessage = "")
        {
            var aggregator = await _userManager.GetUserAsync(User);
            ViewBag.AggId = aggregator.Id;
            if (StatusMessage != "")
            {
                ViewBag.StatusMessage = StatusMessage;
            }
            return View(await PaymentManager.ViewFarmers(_context, _userManager, aggregator.Id));
        }


        [HttpPost]
        public async Task<IActionResult> LinkAggregatorToFarmer(string UserId, string AggId, string link)
        {

            await PaymentManager.LinkAggregatorToFarmer(_context, UserId, AggId, link);
            if (link == "Unlink")
            {
                ViewBag.statusMessage = "Successfully unlinked.";
            }
            else
            {
                ViewBag.statusMessage = "Successfully linked.";
            }

            return Json(link);


            //var markers = _context.Markers.Where(u => u.UserId == user.Id).SingleOrDefault();
            //if (markers != null)
            //{
            //    markers.LatLng = model.mapCoords;
            //    await _context.SaveChangesAsync();
            //}
            //var result = _context.AggregatorAssociations.Add(data);

        }

        [Authorize(Roles = "Super Admin,Aggregator")]
        public async Task<List<ApplicationUser>> ShowAggregatorsFarmers()
        {
            try
            {
                var users = new List<ApplicationUser>();

                var aggregator = await _userManager.GetUserAsync(User);
                var data = _context.AggregatorAssociations.Where(u => u.AggregatorId == aggregator.Id).ToList();

                foreach (var item in data)
                {
                    var user = await _userManager.FindByIdAsync(item.FarmerId);
                    users.Add(user);
                }

                return users;
            }
            catch (Exception ex)
            {

                throw ex;
            }


            //var markers = _context.Markers.Where(u => u.UserId == user.Id).SingleOrDefault();
            //if (markers != null)
            //{
            //    markers.LatLng = model.mapCoords;
            //    await _context.SaveChangesAsync();
            //}
            //var result = _context.AggregatorAssociations.Add(data);

        }



        [Authorize(Roles = "Super Admin,Admin,Aggregator")]
        public IActionResult InitatePayment(string UserId)
        {
            ViewBag.UserId = UserId;
            ViewBag.sasToken = ConfigurationManager.GetAppSetting("SAS");   
            return View();
        }

        [Authorize(Roles = "Super Admin,Admin,Aggregator")]
        public async Task<IActionResult> InitatePayments(string UserId, string FileOrg, string FileName, string Comments)
        {
            
            try
            {
                var SenderId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var user = await _userManager.FindByIdAsync(UserId);
          
                await PaymentManager.InitatePayments(_context, user, UserId, SenderId, FileOrg, FileName, Comments);


                //pm = _context.Payments.Where(p => p.TxGuid == pm.TxGuid).FirstOrDefault();
                //return RedirectToAction("ViewTransaction", new { id = user.Id });
                //return RedirectToAction("ViewFarmers");

                //var model1 = _context.Payments.Where(p => p.TxGuid == guid).FirstOrDefault();
                //ViewData["ShowVerify"] = true;


                //return RedirectToAction("ViewTransaction", new { id = model1.Id });

                ViewData["success"] = "Payment Initiated Successfully!";
                return RedirectToAction("ViewFarmers", new { StatusMessage = "Payment Initiated Successfully!" });



            }
            catch (Exception ex)
            {
                ViewData["Error"] = "There was an error in saving your transaction. " + ex.Message;
                return RedirectToAction("ViewFarmers");
            }












            //await _userManager.UpdateAsync(user);
            //return View();

        }



        [Authorize(Roles = "Super Admin,Admin")]
        public async Task<IActionResult> ViewNotifications()
        {
            try
            {
                return View(await PaymentManager.ViewNotifications(_context));
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "There was an error while fetching your notifications. " + ex.Message;
                return RedirectToAction("Index");
            }
            //foreach (var item in temp)
            //{
            //    //user = await _userManager.GetUserAsync(User);
            //    var roles = await _userManager.GetRolesAsync(item);
            //    item.UserRole = roles[0];

            //}
     
        }












        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
