using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VRTeleportator.Models;
using VRTeleportator.ViewModels;

namespace VRTeleportator.Controllers
{
    [Produces("application/json")]
    [Route("api/registration")]
    public class RegistrationController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly AppDataBase dbContext;

        public RegistrationController(UserManager<User> userManager, AppDataBase dbContext)
        {
            this.userManager = userManager;
            this.dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Registration([FromBody] RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    UserName = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Wallet = default
                    
                };
                var result = await userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    return BadRequest();
                }
            }
            return Ok();
        }
    }
}