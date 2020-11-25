using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RexMercury.Models.ManageViewModels
{
    public class UserRoleViewModel
    {
        public string RoleId { get; set; }
       
        public string RoleName { get; set; }
        public bool IsSelected { get; set; }
    }
}
