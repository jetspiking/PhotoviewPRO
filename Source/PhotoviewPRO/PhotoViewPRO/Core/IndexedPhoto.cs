using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace PhotoViewPRO.Core
{
    public class IndexedPhoto
    {
        public String FilePath { get; set; }
        public String ParentDirectory { get; set; }
        public BitmapSource ThumbnailImage { get; set; }
    }
}
