using PhotoViewPRO.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PhotoViewPRO
{
    public partial class PhotoListItem : UserControl
    {
        public const int ThumbnailSize = 32;
        public IndexedPhoto IndexedPhoto;
        public PhotoListItem(IndexedPhoto indexedPhoto)
        {
            InitializeComponent();
            this.IndexedPhoto = indexedPhoto;
            this.Thumbnail.Source = indexedPhoto.ThumbnailImage;
            this.Name.Content = System.IO.Path.GetFileName(indexedPhoto.FilePath);
        }
    }
}
