using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mtama.Data;
using Mtama.Models;
using Mtama.VM;

namespace Mtama.Managers
{
    public class PaymentManager
    {
        public static VM.PaymentModelVM ViewTransaction(ApplicationDbContext _context, int Id, string userId)
        {
            var PM = new PaymentModelVM();
            PM.Id = Id;
            PM.BlobUri = ConfigurationManager.GetAppSetting("BlobUri");
            PM.SAS = ConfigurationManager.GetAppSetting("SAS");
            PM.Container = ConfigurationManager.GetAppSetting("Container");

            PM.paymentModel = _context.Payments.Where(p => p.Id == Id).Include(p => p.Sender).Include(p => p.Receiver).FirstOrDefault();

            if (PM.paymentModel == null) { throw new Exception("Payment not found"); }

            if (PM.paymentModel.SenderIdNew == userId)
            {
                PM.Sender = true;
            }
            if (PM.paymentModel.ReceiverId == userId)
            {
                PM.Receiver = true;
            }
            if (PM.paymentModel.ReceiverId == userId || PM.paymentModel.SenderIdNew == userId)
            {
                if (PM.paymentModel.SenderIdNew == userId && PM.paymentModel.TxStatus == Common.TxStatusPending)
                {
                    PM.ShowVerify = true;
                }
            }
            return PM;

        }

        public static VM.PaymentModelVM ViewTransaction(ApplicationDbContext _context, PaymentModelVM PM, string userId)
        {

            var modeldata = _context.Payments.Where(p => p.Id == PM.paymentModel.Id).Include(p => p.Sender).Include(p => p.Receiver).FirstOrDefault();
            
            if (modeldata.ReceiverId == userId)
            {
                //  var payment = _context.Payments.Where(p => p.ReceiverId == userId).FirstOrDefault();

                if (PM.paymentModel.ReceiverAttachment != modeldata.ReceiverAttachment)
                {
                    modeldata.ReceiverAttachment = PM.paymentModel.ReceiverAttachment;
                }
                if (PM.paymentModel.ReceiverComment != modeldata.ReceiverComment)
                {
                    modeldata.ReceiverComment = PM.paymentModel.ReceiverComment;
                }

                _context.Payments.Update(modeldata);
                _context.SaveChanges();

            }

            if (modeldata.SenderIdNew == userId)
            {
                // var payment = _context.Payments.Where(p => p.SenderId == userId).FirstOrDefault();

                if (PM.paymentModel.SenderAttachment != modeldata.SenderAttachment)
                {
                    modeldata.SenderAttachment = PM.paymentModel.SenderAttachment;
                }
                if (PM.paymentModel.SenderComment != modeldata.SenderComment)
                {
                    modeldata.SenderComment = PM.paymentModel.SenderComment;
                }

                if (PM.paymentModel.TxStatus == Common.TxStatusVerified)
                {
                    modeldata.TxStatus = Common.TxStatusVerified;
                }
                _context.Payments.Update(modeldata);
                _context.SaveChanges();
            }









            return PM;

        }

        public static Models.PaymentModel MakePayments()
        {
            var pm = new PaymentModel();
            pm.TxGuid = Guid.NewGuid().ToString();
            return pm;
        }

        public static PaymentModel MakePayments(ApplicationDbContext _context, PaymentModel model, string userId)
        {
            var receiver = _context.Users.Where(u => u.WalletAddress == model.ReceiverWallet).SingleOrDefault();

            model.ReceiverId = receiver.Id;
            model.SenderIdNew = userId;
            model.TimeStamp = DateTime.UtcNow;
            model.TxStatus = Common.TxStatusPending;

            _context.Payments.Add(model);
            _context.SaveChanges();

            return _context.Payments.Where(p => p.TxGuid == model.TxGuid).FirstOrDefault();
        }

        public static async Task<List<PaymentModel>> GetPayments(ApplicationDbContext _context, string userId = "")
        {
            if (userId == "")
            {
                return await _context.Payments.Include(p => p.Sender).Include(p => p.Receiver).ToListAsync();
            }
            else
            {
                return await _context.Payments.Where(p => p.SenderIdNew == userId || p.ReceiverId == userId).Include(p => p.Sender).Include(p => p.Receiver).ToListAsync();
            }

        }

        public static async Task<List<ApplicationUser>> ViewFarmers(ApplicationDbContext _context, UserManager<ApplicationUser> _userManager, string aggregatorId)
        {
            var userData = await _context.Users.ToListAsync();

            var data = _context.AggregatorAssociations.Where(u => u.AggregatorId == aggregatorId).ToList();

            foreach (var item in userData)
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
            return userData;
        }

        public static async Task LinkAggregatorToFarmer(ApplicationDbContext _context, string UserId, string AggId, string Link)
        {
            if (Link == "Link")
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
        }

        public static async Task<bool> InitatePayments(ApplicationDbContext _context, ApplicationUser user, string UserId, string SenderId, string FileOrg, string FileName, string Comments)
        {
            var pm = new PaymentModel();
            var guid = Guid.NewGuid().ToString();

            pm.SenderIdNew = SenderId;
            pm.TxGuid = guid;
            pm.ReceiverId = user.Id;
            pm.ReceiverName = user.FirstName + " " + user.LastName;
            pm.ReceiverWallet = user.WalletAddress;
            pm.TimeStamp = DateTime.UtcNow;
            pm.TxStatus = Common.TxStatusDraft;
            pm.AggregatorAttachment = FileOrg;
            pm.AggregatorComment = Comments;
            pm.FileName = FileName;

            _context.Payments.Add(pm);
            _context.SaveChanges();

            return true;

        }

        public static async Task<List<PaymentModel>> ViewNotifications(ApplicationDbContext _context)
        {

            return await _context.Payments.ToListAsync();
        }

        public static async Task<PaymentModel> MakePaymentFromAdmin(ApplicationDbContext _context, string PaymentId)
        {
            return _context.Payments.Where(u => u.Id == Convert.ToInt32(PaymentId)).FirstOrDefault();
        }




















    }
}
