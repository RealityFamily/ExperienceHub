using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using VRTeleportator.Models;
using System;

namespace VRTeleportator.Controllers
{
    //[Produces("application/json")]
    [Route("api/file")]
    public class FileController : Controller
    {
        private readonly IHostingEnvironment environment;
        private readonly AppDataBase context;

        public FileController(IHostingEnvironment environment, AppDataBase context)
        {
            this.environment = environment;
            this.context = context;
        }

        [HttpPost]
        [Route("{LessonId}/upload")]
        public async Task<IActionResult> UploadFile(IFormFile uploadedFile, Guid LessonId)
        {
            var result = context.Lessons.Find(LessonId);
            var path = uploadedFile.FileName;

            using (var fileStream = new FileStream(Path.Combine(environment.WebRootPath, path), FileMode.Create))
            {
                await uploadedFile.CopyToAsync(fileStream);
            }

            result.Path = path;
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}