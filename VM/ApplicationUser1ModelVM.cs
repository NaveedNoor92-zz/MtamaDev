using Mtama.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mtama.VM
{
    public class ApplicationUser1ModelVM
    {
        public ApplicationUser applicationUser { get; set; }

        public string UserRole { get; set; }
        public bool AdminEdit { get; set; }
        public string Markers { get; set; }


    }
}
