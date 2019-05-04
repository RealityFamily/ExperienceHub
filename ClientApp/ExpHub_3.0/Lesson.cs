using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ExperenceHubApp
{
    public class Lesson
    {
        public string Name;
        public string Path;
        public string localpath;
        public DateTime PurchaseDate;
        public DateTime ReleaseDate;
        public byte[] Picture;
        public float Price;
        public string Description;
        public string Creator;
        public Guid CreatorID;
        public Guid LessonID;
        public string Category;
        public string Subcategory;

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            if (localpath == null)
            {
                localpath = localpath;
            }
        }
    }
}
