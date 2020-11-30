using Mtama.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mtama.VM
{
    public class ProfileViewModelVM
    {
        public ProfileViewModel profileViewModel { get; set; }


        public string UserRole { get; set; } = "";
        public bool AdminEdit { get; set; }
        public string Markers { get; set; }


        public string BlobUri { get; set; }
        public string Container { get; set; }
        public string SAS { get; set; }


    }
}
