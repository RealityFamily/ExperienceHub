using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VRTeleportator.Models;

namespace VRTeleportator.ViewModels
{
    public class UserViewModel
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public float Wallet { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public List<Lesson> Lessons { get; set; }


    }
}
