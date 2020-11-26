using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Mtama.Data;
using Mtama.Models;

namespace Mtama.Managers
{
    public class PaymentManager
    {
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
    }
}
