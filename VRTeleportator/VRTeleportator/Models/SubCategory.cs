using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VRTeleportator.Models
{
    public class SubCategory
    {
        public Guid SubCategoryId { get; set; }
        public Guid CategoryId { get; set; }
        public string SubCategoryName { get; set; }
    }
}
