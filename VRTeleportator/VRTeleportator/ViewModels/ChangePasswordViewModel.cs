using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VRTeleportator.ViewModels
{
    public class ChangePasswordViewModel
    {
        public Guid UserId { get; set; }
        public string NewPassword { get; set; }
    }
}
