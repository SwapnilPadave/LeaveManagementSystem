using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagementSystem.Data
{
    public class RoleMaster : IdentityUser
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
