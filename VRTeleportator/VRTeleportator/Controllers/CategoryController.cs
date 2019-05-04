using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VRTeleportator.Models;
using Microsoft.EntityFrameworkCore;
using VRTeleportator.ViewModels;

namespace VRTeleportator.Controllers
{
    [Produces("application/json")]
    [Route("api/Category")]
    public class CategoryController : Controller
    {
        private readonly AppDataBase dbContext;

        public CategoryController(AppDataBase dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddCategory([FromBody] CreateCategoryViewModel createRequest)
        {
            Category category = new Category()
            {
                CategoryName = createRequest.Name,
                SubCategories = new List<SubCategory>()
            };
            await dbContext.Categories.AddAsync(category);
            await dbContext.SaveChangesAsync();
            return Ok();
        }
            
        [HttpPost] 
        [Route("{CategoryId}/sub/add")]
        public async Task<IActionResult> AddSubCategory([FromBody] CreateSubCategoryViewModel createRequest, Guid CategoryId)
        {
            var category = dbContext.Categories.Include(b => b.SubCategories).FirstOrDefault(g => g.CategoryId == CategoryId);

            SubCategory subCategory = new SubCategory()
            {
                SubCategoryName = createRequest.Name
            };

            await dbContext.SubCategories.AddAsync(subCategory);
            category.SubCategories.Add(subCategory);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public IActionResult GetCategories()
        {
             
            var categories = dbContext.Categories.Include(s => s.SubCategories).ToList();
          
            var res = categories.ToDictionary(x => x.CategoryName, x => x.SubCategories.Select(s => s.SubCategoryName).ToList());
            return Json(res);
        }
    }
}