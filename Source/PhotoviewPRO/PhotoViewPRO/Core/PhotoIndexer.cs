using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PhotoViewPRO.Core
{
    public class PhotoIndexer
    {
        public readonly PhotoSelectResult PhotoSelectResult;
        public readonly BitmapImage SelectedPhoto;
        public readonly IndexedPhoto[] IndexedPhotos;

        public PhotoIndexer(PhotoSelectResult photoSelectResult) 
        {
            this.PhotoSelectResult = photoSelectResult;
            this.SelectedPhoto = new BitmapImage(new Uri(photoSelectResult.FilePath));

            List<IndexedPhoto> indexedPhotos = new();

            foreach(String file in Directory.GetFiles(photoSelectResult.ParentDirectory))
            {
                if (photoSelectResult.Extensions.Contains(Path.GetExtension(file).ToUpper().Replace(".", String.Empty)))
                {
                    IndexedPhoto indexedPhoto = new();
                    indexedPhoto.FilePath = file;
                    indexedPhoto.ParentDirectory = photoSelectResult.ParentDirectory;

                    BitmapImage bitmap = new BitmapImage();
                    try
                    {
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(file);
                        bitmap.EndInit();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show($"Failed to load image: \n{indexedPhoto.FilePath}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        continue;
                    }

                    indexedPhoto.ThumbnailImage = new TransformedBitmap(bitmap, new ScaleTransform(32.0 / bitmap.PixelWidth, 32.0 / bitmap.PixelHeight));

                    indexedPhotos.Add(indexedPhoto);
                }
            }

            this.IndexedPhotos = indexedPhotos.ToArray();
        }
    }
}
