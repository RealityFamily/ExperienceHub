using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VRTeleportator.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace VRTeleportator
{
    public class AppDataBase : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public AppDataBase(DbContextOptions options) : base(options)
        {
        }
        public DbSet<User> UserAccounts { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
    }
}
