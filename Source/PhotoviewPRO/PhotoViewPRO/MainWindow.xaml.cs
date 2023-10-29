using PhotoViewPRO.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace PhotoViewPRO
{
    public partial class MainWindow : Window
    {
        public PhotoListItem CurrentPhoto { get; set; }
        private String _selectedPhotoPath { get; set; }
        private PhotoIndexer? _photoIndexer { get; set; }
        private Boolean _isFullscreen { get; set; } = true;
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadPhotoSource(String filePath)
        {
            this._selectedPhotoPath = filePath;
            SelectedPhoto.Source = new BitmapImage(new Uri(filePath));
        }

        private void File_MouseClick(object sender, RoutedEventArgs e)
        {
            PhotoSelectResult? photoSelectResult = PhotoSelect.SelectFromDirectory();
            if (photoSelectResult == null) return;

            PhotoListItemsPanel.Children.Clear();
            LoadPhotoSource(photoSelectResult.FilePath);

            this._photoIndexer = new(photoSelectResult);
            foreach (IndexedPhoto indexedPhoto in this._photoIndexer.IndexedPhotos)
            {
                PhotoListItem photoListItem = new PhotoListItem(indexedPhoto);
                photoListItem.HorizontalAlignment = HorizontalAlignment.Stretch;

                photoListItem.MouseLeftButtonDown += (object sender, MouseButtonEventArgs e) =>
                {
                    ClickOnPhotoListElement(indexedPhoto, photoListItem);
                };

                if (photoSelectResult.FilePath.Equals(indexedPhoto.FilePath))
                {
                    CurrentPhoto = photoListItem;
                    photoListItem.Background = new SolidColorBrush(Colors.LightGray);
                }

                PhotoListItemsPanel.Children.Add(photoListItem);
            }
        }

        private void ClickOnPhotoListElement(IndexedPhoto indexedPhoto, PhotoListItem photoListItem)
        {
            CurrentPhoto.Background = new SolidColorBrush(Colors.Transparent);
            LoadPhotoSource(indexedPhoto.FilePath);
            photoListItem.Background = new SolidColorBrush(Colors.LightGray);
            CurrentPhoto = photoListItem;
        }

        private void ToPreviousPhoto()
        {
            if (this._photoIndexer == null || this.PhotoListItemsPanel.Children == null) return;

            for (int i = 0; i < this._photoIndexer.IndexedPhotos.Count(); i++)
            {
                if (this._selectedPhotoPath.Equals(this._photoIndexer.IndexedPhotos[i].FilePath))
                {
                    if (i == 0) return;

                    ClickOnPhotoListElement(this._photoIndexer.IndexedPhotos[i - 1], (PhotoListItem)this.PhotoListItemsPanel.Children[i-1]);
                }
            }
        }

        private void ToNextPhoto()
        {
            if (this._photoIndexer == null || this.PhotoListItemsPanel == null) return;

            for (int i = this._photoIndexer.IndexedPhotos.Count()-1; i > -1; i--)
            {
                if (this._selectedPhotoPath.Equals(this._photoIndexer.IndexedPhotos[i].FilePath))
                {
                    if (i == this._photoIndexer.IndexedPhotos.Count()-1) return;

                    ClickOnPhotoListElement(this._photoIndexer.IndexedPhotos[i + 1], (PhotoListItem)this.PhotoListItemsPanel.Children[i + 1]);
                }
            }
        }

        private void Print_MouseClick(object sender, RoutedEventArgs e)
        {
            if (this._selectedPhotoPath == null) return;

            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                Image image = new Image();
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(this._selectedPhotoPath);
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                image.Source = bitmapImage;

                Task.Delay(500);
                {
                    Double aspectRatio = bitmapImage.Width / bitmapImage.Height;
                    Double printableWidth = printDialog.PrintableAreaWidth;
                    Double printableHeight = printDialog.PrintableAreaHeight;

                    Double printWidth, printHeight;

                    if (printableWidth / printableHeight > aspectRatio)
                    {
                        printHeight = printableHeight;
                        printWidth = printHeight * aspectRatio;
                    }
                    else
                    {
                        printWidth = printableWidth;
                        printHeight = printWidth / aspectRatio;
                    }

                    VisualBrush visualBrush = new VisualBrush(image);
                    DrawingVisual printVisual = new DrawingVisual();
                    using (DrawingContext drawingContext = printVisual.RenderOpen())
                    {
                        drawingContext.DrawRectangle(visualBrush, null, new Rect(new Point((printableWidth - printWidth) / 2, (printableHeight - printHeight) / 2), new Size(printWidth, printHeight)));
                    }

                    printDialog.PrintVisual(printVisual, "Print Image");
                };
            }
        }

        private void Clipboard_MouseClick(object sender, RoutedEventArgs e)
        {
            if (this._selectedPhotoPath == null) return;

            Clipboard.SetImage(new BitmapImage(new Uri(this._selectedPhotoPath)));
        }

        private void RotateLeft_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedPhoto.Source != null)
            {
                RotateTransform rotateTransform = SelectedPhoto.LayoutTransform as RotateTransform;
                if (rotateTransform != null)
                {
                    Double currentAngle = rotateTransform.Angle;
                    rotateTransform.Angle = currentAngle - 90;
                }
            }
        }

        private void RotateRight_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedPhoto.Source != null)
            {
                RotateTransform rotateTransform = SelectedPhoto.LayoutTransform as RotateTransform;
                if (rotateTransform != null)
                {
                    Double currentAngle = rotateTransform.Angle;
                    rotateTransform.Angle = currentAngle + 90;
                }
            }
        }
        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            ToPreviousPhoto();
        }
        private void Next_Click(object sender, RoutedEventArgs e)
        {
            ToNextPhoto();
        }
        private void Fullscreen_Click(object sender, RoutedEventArgs e)
        {
            _isFullscreen = !_isFullscreen;
            PhotoListScroller.Visibility = _isFullscreen ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
