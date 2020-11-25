using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Mtama.Models.HomeViewModels
{
    public class PaymentModel
    {
        public int Id { get; set; }
        
        ////[Required
        public string SenderIdNew { get; set; }
        public ApplicationUser Sender { get; set; }

        //[Required        
        public string ReceiverId { get; set; }
        public string ReceiverName { get; set; }
        public ApplicationUser Receiver { get; set; }

        //[Required
        [Display(Name = "Sender Wallet")]
        public string SenderWallet { get; set; }

        //[Required  
        [Display(Name = "Receiver Wallet")]
        public string ReceiverWallet { get; set; }

        //[Required
        [Display(Name = "Amount In Fiat")]
        public decimal AmountInFiat { get; set; }

        //[Required
        public decimal AmountInEther { get; set; }
        
        //[Required
        public string Currency { get; set; } = "Kenyan";

        //[Required
        [Display(Name = "Time Stamp")]
        public DateTime TimeStamp { get; set; }

        //[Required
        [Display(Name = "Transaction Status")]
        public string TxStatus { get; set; } = Common.TxStatusPending; //"Pending/Verified/Completed"

        [Display(Name = "Transaction Note")]
        public string TxNote { get; set; }

        //[Required
        [Display(Name = "Transaction Hash")]
        public string TxHash { get; set; } = "0x0000000000000000";

        //[Required
        [Display(Name = "Transaction Hash2")]
        public string TxHash2 { get; set; } = "0x0000000000000000";

        public string TxGuid { get; set; }

        [Display(Name = "Sender Attachment")]
        public string SenderAttachment { get; set; }

        [Display(Name = "Sender Comment")]
        public string SenderComment { get; set; }

        [Display(Name = "Receiver Attachment")]
        public string ReceiverAttachment { get; set; }

        [Display(Name = "Receiver Comment")]
        public string ReceiverComment { get; set; }




        public string AggregatorAttachment { get; set; }
        public string AggregatorComment { get; set; }
        public string FileName { get; set; }

    }

    public class PaymentCSVModel
    {

        public int Id { get; set; }
        public string SenderName { get; set; }
        public string SenderWallet { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverWallet { get; set; }
        public decimal AmountInFiat { get; set; }
        public decimal AmountInEther { get; set; }
        public string Currency { get; set; } 
        public DateTime TimeStamp { get; set; }
        public string TxStatus { get; set; }
        public string TxNote { get; set; }
        public string TxHash { get; set; } 
        public string TxHash2 { get; set; } 
        public string TxGuid { get; set; }
        public string SenderAttachment { get; set; }
        public string SenderComment { get; set; }
        public string ReceiverAttachment { get; set; }
        public string ReceiverComment { get; set; }


        public PaymentCSVModel(PaymentModel pm)
        {
            Id = pm.Id;
            SenderName = pm.Sender.FirstName + " " + pm.Sender.LastName;
            SenderWallet = pm.SenderWallet;
            SenderAttachment = pm.SenderAttachment;
            SenderComment = pm.SenderComment;
            ReceiverName = pm.Receiver.FirstName + " " + pm.Receiver.LastName;
            ReceiverWallet = pm.ReceiverWallet;
            ReceiverAttachment = pm.ReceiverAttachment;
            ReceiverComment = pm.ReceiverComment;
            AmountInFiat = pm.AmountInFiat;
            AmountInEther = pm.AmountInEther;
            Currency = pm.Currency;
            TimeStamp = pm.TimeStamp;
            TxStatus = pm.TxStatus;
            TxNote = pm.TxNote;
            TxHash = pm.TxHash;
            TxHash2 = pm.TxHash2;
            TxGuid = pm.TxGuid;
        }
    

    }

}

