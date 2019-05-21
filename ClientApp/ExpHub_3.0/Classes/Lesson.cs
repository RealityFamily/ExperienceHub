using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ExperenceHubApp
{
    public class Lesson
    {
        public string Name;
        public string Path;
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

        public ImageSource GetPrev()
        {
            if (Picture != null) {
                BitmapImage biImg = new BitmapImage();
                MemoryStream ms = new MemoryStream(Picture);
                biImg.BeginInit();
                biImg.StreamSource = ms;
                biImg.EndInit();

                ImageSource imgSrc = biImg as ImageSource;

                return imgSrc;
            } else
            {
                return null;
            }
        }
    }
}
