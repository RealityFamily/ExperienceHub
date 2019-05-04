using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VRTeleportator.Models;
using VRTeleportator.ViewModels;

namespace VRTeleportator.Controllers
{
    [Produces("application/json")]
    //[Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/lesson")]
    public class LessonController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly AppDataBase dbContext;
        private readonly IHostingEnvironment environment;

        public LessonController(UserManager<User> userManager, AppDataBase dbContext, IHostingEnvironment environment)
        {
            this.userManager = userManager;
            this.dbContext = dbContext;
            this.environment = environment;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddLesson([FromBody]LessonAddViewModel model)
        {
            var result = await dbContext.Users.FindAsync(model.CreatorId);

            Lesson lesson = new Lesson
            {
                Name = model.Name,
                Price = model.Price,
                ReleaseDate = DateTime.UtcNow.Date,
                Description = model.Description,
                Creator = $"{result.FirstName} {result.LastName}",
            };

            await dbContext.AddAsync(lesson);
            await dbContext.SaveChangesAsync();
            return Json(lesson.LessonId);
        }
    }
}