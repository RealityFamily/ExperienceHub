using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VRTeleportator.ViewModels
{
    public class LessonAddViewModel
    {
        public string Name { get; set; }
        public Guid CreatorId { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
