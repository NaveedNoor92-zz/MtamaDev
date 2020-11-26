using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mtama.Models;
using Microsoft.EntityFrameworkCore;
using Mtama.Data;
using System;
using System.Web;
using Mtama.Models.HomeViewModels;
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
using RexMercury.Models;

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
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult MakePayments(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var pm = new PaymentModel();
            pm.TxGuid = Guid.NewGuid().ToString();
            return View(pm);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MakePayments(PaymentModel model, string returnUrl = null)
        {
            try
            {
                ViewData["ReturnUrl"] = returnUrl;

                var receiver = _context.Users.Where(u => u.WalletAddress == model.ReceiverWallet).SingleOrDefault();

                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                model.ReceiverId = receiver.Id;
                model.SenderIdNew = userId;
                model.TimeStamp = DateTime.UtcNow;
                model.TxStatus = Common.TxStatusPending;

                _context.Payments.Add(model);
                _context.SaveChanges();

                model = _context.Payments.Where(p => p.TxGuid == model.TxGuid).FirstOrDefault();
                ViewData["ShowVerify"] = true;
                return RedirectToAction("ViewTransaction", new { id = model.Id });
                //return View();
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "There was an error in saving your transaction. " + ex.Message;
                return View();
            }

        }





        [HttpGet]
        public IActionResult ViewTransaction(int id)
        {
            ViewData["ShowVerify"] = false;
            ViewData["Id"] = id;
            ViewData["Sender"] = false;
            ViewData["Receiver"] = false;
            ViewData["bloburi"] = ConfigurationManager.GetAppSetting("BlobUri");
            ViewData["sas"] = ConfigurationManager.GetAppSetting("SAS");
            ViewData["container"] = ConfigurationManager.GetAppSetting("Container");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var payment = _context.Payments.Where(p => p.Id == id).Include(p => p.Sender).Include(p => p.Receiver).FirstOrDefault();
            if (payment == null) throw new Exception("Payment not found");
            if (payment.SenderIdNew == userId)
            {
                ViewData["Sender"] = true;
            }
            if (payment.ReceiverId == userId)
            {
                ViewData["Receiver"] = true;
            }
            if (payment.ReceiverId == userId || payment.SenderIdNew == userId)
            {
                if (payment.SenderIdNew == userId && payment.TxStatus == Common.TxStatusPending)
                {
                    ViewData["ShowVerify"] = true;
                }
            }
            else
            {
                if (!User.IsInRole("Admin"))
                    throw new Exception("You are not authorized to view this payment");
            }
            return View(payment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ViewTransaction(PaymentModel model, string returnUrl = null)
        {
            var modeldata = _context.Payments.Where(p => p.Id == model.Id).Include(p => p.Sender).Include(p => p.Receiver).FirstOrDefault();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (modeldata.ReceiverId == userId)
            {
                //  var payment = _context.Payments.Where(p => p.ReceiverId == userId).FirstOrDefault();

                if (model.ReceiverAttachment != modeldata.ReceiverAttachment)
                {
                    modeldata.ReceiverAttachment = model.ReceiverAttachment;
                }
                if (model.ReceiverComment != modeldata.ReceiverComment)
                {
                    modeldata.ReceiverComment = model.ReceiverComment;
                }

                _context.Payments.Update(modeldata);
                _context.SaveChanges();

            }

            if (modeldata.SenderIdNew == userId)
            {
                // var payment = _context.Payments.Where(p => p.SenderId == userId).FirstOrDefault();

                if (model.SenderAttachment != modeldata.SenderAttachment)
                {
                    modeldata.SenderAttachment = model.SenderAttachment;
                }
                if (model.SenderComment != modeldata.SenderComment)
                {
                    modeldata.SenderComment = model.SenderComment;
                }

                if (model.TxStatus == Common.TxStatusVerified)
                {
                    modeldata.TxStatus = Common.TxStatusVerified;
                }
                _context.Payments.Update(modeldata);
                _context.SaveChanges();
            }

            //if (!User.IsInRole("Admin")) { 
            //    throw new Exception("Only Transaction Initiator can verify transaction");
            //}

            ViewData["ShowVerify"] = false;
            return RedirectToAction("ViewTransaction", new { id = model.Id });
        }

        [Authorize(Roles = "Aggregator, Admin, supplier, Super Admin, Farmer")]
        [HttpGet]
        public async Task<IActionResult> ViewPayments(string searchString, string sortOrder, string currentFilter, int? page)
        {
            ViewData["Title"] = "View Payments";

            var payments = await GetPayments();
            return View(payments);
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
            if (User.IsInRole("Admin"))
            {
                return await _context.Payments.Include(p => p.Sender).Include(p => p.Receiver).ToListAsync();
            }
            else
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                return await _context.Payments.Where(p => p.SenderIdNew == userId || p.ReceiverId == userId).Include(p => p.Sender).Include(p => p.Receiver).ToListAsync();
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
        public async Task<IActionResult> ViewFarmers()
        {
            var temp = await _context.Users.ToListAsync();

            var aggregator = await _userManager.GetUserAsync(User);
            ViewBag.AggId = aggregator.Id;

            var data = _context.AggregatorAssociations.Where(u => u.AggregatorId == aggregator.Id).ToList();

            foreach (var item in temp)
            {
                //user = await _userManager.GetUserAsync(User);
                var roles = await _userManager.GetRolesAsync(item);
                item.UserRole = roles[0];
                foreach (var item1 in data)
                {
                    if (item1.FarmerId == item.Id)
                    {
                        item.AggregatorLinked = true;
                    }
                }

            }
        
          

            return View(temp);
        }


        [HttpPost]
        public async Task<IActionResult> LinkAggregatorToFarmer(string UserId, string AggId, string link)
        {
            if (link == "Link")
            {
                var data = new AggregatorAssociationModel
                {
                    AggregatorId = AggId,
                    FarmerId = UserId
                };

                _context.Add(data);
                _context.SaveChanges();
            }
            else
            {
                var data = _context.AggregatorAssociations.Where(u => u.AggregatorId == AggId).ToList();
                if (data != null)
                {

                    foreach (var item in data)
                    {
                        if (item.FarmerId == UserId)
                        {
                            item.FarmerId = "";
                            item.AggregatorId = "";
                        }
                    }

                    await _context.SaveChangesAsync();
                    

                }
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
            

                var user = await _userManager.FindByIdAsync(UserId);

                var pm = new PaymentModel();


                var SenderId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var guid = Guid.NewGuid().ToString();

                pm.SenderIdNew = SenderId;
                pm.TxGuid = guid;
                pm.ReceiverId = user.Id;
                pm.ReceiverName = user.FirstName +" "+ user.LastName;
                pm.ReceiverWallet = user.WalletAddress;
                pm.TimeStamp = DateTime.UtcNow;
                pm.TxStatus = Common.TxStatusDraft;
                pm.AggregatorAttachment = FileOrg;
                pm.AggregatorComment = Comments;
                pm.FileName = FileName;

               

                _context.Payments.Add(pm);
                 _context.SaveChanges();


                //pm = _context.Payments.Where(p => p.TxGuid == pm.TxGuid).FirstOrDefault();
                //return RedirectToAction("ViewTransaction", new { id = user.Id });
                //return RedirectToAction("ViewFarmers");

                //var model1 = _context.Payments.Where(p => p.TxGuid == guid).FirstOrDefault();
                //ViewData["ShowVerify"] = true;


                //return RedirectToAction("ViewTransaction", new { id = model1.Id });



                return RedirectToAction("ViewFarmers");



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
            var temp = await _context.Payments.ToListAsync();
            //foreach (var item in temp)
            //{
            //    //user = await _userManager.GetUserAsync(User);
            //    var roles = await _userManager.GetRolesAsync(item);
            //    item.UserRole = roles[0];

            //}
            return View(temp);
        }



        public async Task<IActionResult> MakePaymentFromAdmin(string PaymentId, string ReceiverId)
        {
            try
            {
                //var model = new PaymentModel();
                //var user = await _userManager.FindByIdAsync(ReceiverId);
                //var modeldata = _context.Payments.Where(p => p.Id == Convert.ToInt64(PaymentId)).Include(p => p.Sender).Include(p => p.Receiver).FirstOrDefault();

                //var receiver = _context.Users.Where(u => u.WalletAddress == user.WalletAddress).SingleOrDefault();

                //var model = new PaymentModel();
                var data = _context.Payments.Where(u => u.Id == Convert.ToInt32(PaymentId)).FirstOrDefault();

                //var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                


                return View(data);



                //return RedirectToAction("ViewTransaction", new { id = PaymentId});
                //return View();
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "There was an error in saving your transaction. " + ex.Message;
                return View();
            }

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
