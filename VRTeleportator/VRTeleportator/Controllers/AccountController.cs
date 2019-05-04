using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VRTeleportator.Exceptions;
using VRTeleportator.Models;
using VRTeleportator.ViewModels;

namespace VRTeleportator.Controllers
{
    [Produces("application/json")]
    //[Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/account")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly AppDataBase dbContext;
        private readonly IPasswordHasher<User> passwordHasher;
        private readonly IPasswordValidator<User> passwordValidator;

        public AccountController(UserManager<User> userManager,
            AppDataBase dbContext,
            IPasswordHasher<User> passwordHasher,
            IPasswordValidator<User> passwordValidator)
        {
            this.userManager = userManager;
            this.dbContext = dbContext;
            this.passwordHasher = passwordHasher;
            this.passwordValidator = passwordValidator;
        }

        [HttpGet]
        [Route("lessons")]
        public IActionResult GetLessons()
        {
            var lesson = dbContext.Lessons.Include(l => l.UserLessons).ThenInclude(u => u.User).ToList();
            return Json(lesson);
        }

        [HttpGet]
        [Route("{UserId}/get_my_lessons")]
        public async Task<IActionResult> GetMyLessons(Guid UserId)
        {
            if (await userManager.FindByIdAsync(UserId.ToString()) == null)
            {
                //throw new ServiceException(Exceptions.StatusCode.NotFound);
            }

            List<Lesson> lessonsView = new List<Lesson>();
            var lessons = dbContext.Lessons.Include(u => u.UserLessons).ThenInclude(u => u.User).ToList();
            var result = await userManager.FindByIdAsync(UserId.ToString());

            foreach (var lesson in lessons)
            {
                var user = lesson.UserLessons.Select(u => u.User).ToList();
                if (user.Contains(result))
                {
                    lessonsView.Add(lesson);
                }
            }
            return Json(lessonsView.Select(t => new
            {
                t.Name,
                t.Creator,
                t.Description,
                t.LessonId,
                t.Path,
                t.Picture,
                t.ReleaseDate,
                t.PurchaseDate
            }));
        }

        [HttpGet]
        [Route("{UserId}/shop")]
        public IActionResult GetShopLessons(Guid UserId)
        {
            List<Lesson> lessonsView = new List<Lesson>();
            var lessons = dbContext.Lessons.Include(u => u.UserLessons).ThenInclude(u => u.User).ToList();
            var result = userManager.FindByIdAsync(UserId.ToString()).GetAwaiter().GetResult();

            foreach (var lesson in lessons)
            {
                var user = lesson.UserLessons.Select(u => u.User).ToList();
                if (!user.Contains(result))
                {
                    lessonsView.Add(lesson);
                }
            }
            return Json(lessonsView);
        }

        [HttpPut]
        [Route("wallet")]
        public async Task<IActionResult> ReplanishWallet([FromBody] WalletReplanishViewModel model)
        {
            var result = dbContext.Users.FirstOrDefault(u => u.Id == model.UserId).Wallet += model.Wallet;
            await dbContext.SaveChangesAsync();

            return Ok(dbContext.Users.FirstOrDefault(u => u.Id == model.UserId).Wallet);
        }

        [HttpPost]
        [Route("{UserId}/lessons/purchase")]
        public async Task<IActionResult> PurchaseLesson([FromBody] PurchaseViewModel model, Guid UserId)
        {
            var result = await dbContext.Users.FindAsync(UserId);

            if (result.UserLessons == null)
            {
                result.UserLessons = new List<UserLessons>();
            }

            result.UserLessons.Add(new UserLessons { UserId = UserId, LessonId = model.LessonId });
            result.Wallet -= model.Price;

            await dbContext.SaveChangesAsync();

            return Ok(result.Wallet);
        }

        [HttpPut]
        [Route("edit")]
        public async Task<IActionResult> EditAccount([FromBody] EditAccountModelView model)
        {
            var result = await userManager.FindByIdAsync(model.UserId.ToString());

            if (model.FirstName != null)
            {
                result.FirstName = model.FirstName;
            }
            if (model.LastName != null)
            {
                result.LastName = model.LastName;
            }
            if (model.Email != null)
            {
                result.Email = model.Email;
            }

            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        [Route("change_password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordViewModel model)
        {
            var result = await userManager.FindByIdAsync(model.UserId.ToString());

            IdentityResult res = await passwordValidator.ValidateAsync(userManager, result, model.NewPassword);
            if (!res.Succeeded)
            {
                return BadRequest();
            }
            result.PasswordHash = passwordHasher.HashPassword(result, model.NewPassword);
            await dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}