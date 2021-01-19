using Mtama.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mtama.VM
{
    public class PaymentModelVM
    {
        public PaymentModel paymentModel { get; set; }

        public int Id { get; set; }
        public bool Sender { get; set; } = false;
        public bool Receiver { get; set; } = false;
        public bool ShowVerify { get; set; } = false;

        public string BlobUri { get; set; }
        public string Container { get; set; }
        public string SAS { get; set; }



    }
}
