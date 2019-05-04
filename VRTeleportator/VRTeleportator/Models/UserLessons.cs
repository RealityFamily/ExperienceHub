using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VRTeleportator.Models
{
    public class UserLessons
    {
        public Guid UserLessonsId { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid LessonId { get; set; }
        public Lesson Lesson { get; set; }
    }
}
