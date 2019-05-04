using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VRTeleportator.Models
{
    public class Category
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<SubCategory> SubCategories { get; set; }
    }
}
