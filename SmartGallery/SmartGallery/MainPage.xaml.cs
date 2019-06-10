using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using SmartGallery.Service;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x416

namespace SmartGallery
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public SoftwareBitmap OutputBitmap
        {
            get { return (SoftwareBitmap)GetValue(OutputBitmapProperty); }
            set { SetValue(OutputBitmapProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OutputBitmap.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OutputBitmapProperty =
            DependencyProperty.Register("OutputBitmap", typeof(SoftwareBitmap), typeof(MainPage), new PropertyMetadata(null, OutputBitmapCallback));

        private static async void OutputBitmapCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                MainPage mainPage = (MainPage)d;
                var bitmapSource = new SoftwareBitmapSource();
                await bitmapSource.SetBitmapAsync((SoftwareBitmap)e.NewValue);
                mainPage.Output.Source = bitmapSource;
            }
        }


        public SoftwareBitmap InputBitmap
        {
            get { return (SoftwareBitmap)GetValue(InputBitmapProperty); }
            set { SetValue(InputBitmapProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InputBitmap.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InputBitmapProperty =
            DependencyProperty.Register("InputBitmap", typeof(SoftwareBitmap), typeof(MainPage), new PropertyMetadata(null, InputBitmapCallback));

        private static async void InputBitmapCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                MainPage mainPage = (MainPage)d;
                var bitmapSource = new SoftwareBitmapSource();
                await bitmapSource.SetBitmapAsync((SoftwareBitmap)e.NewValue);
                mainPage.Input.Source = bitmapSource;
            }
        }


        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void LoadImage(object sender, RoutedEventArgs e)
        {
            StorageFile imageFile = await ChooseImageFileAsync();
            if (imageFile != null)
            {
                InputBitmap = await SoftwareBitmapByStorageFile(imageFile);
            }
        }

        private async Task<StorageFile> ChooseImageFileAsync()
        {
            FileOpenPicker fileOpenPicker = new FileOpenPicker();
            fileOpenPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            fileOpenPicker.FileTypeFilter.Add(".jpg");
            fileOpenPicker.FileTypeFilter.Add(".png");
            fileOpenPicker.ViewMode = PickerViewMode.Thumbnail;

            return await fileOpenPicker.PickSingleFileAsync();
        }

        private async Task<SoftwareBitmap> SoftwareBitmapByStorageFile(StorageFile imageFile)
        {
            SoftwareBitmap inputBitmap;
            using (IRandomAccessStream stream = await imageFile.OpenAsync(FileAccessMode.Read))
            {
                // Create the decoder from the stream
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);

                // Get the SoftwareBitmap representation of the file
                inputBitmap = await decoder.GetSoftwareBitmapAsync();
            }

            if (inputBitmap.BitmapPixelFormat != BitmapPixelFormat.Bgra8
                || inputBitmap.BitmapAlphaMode != BitmapAlphaMode.Premultiplied)
            {
                inputBitmap = SoftwareBitmap.Convert(inputBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
            }

            return inputBitmap;
        }

        private void ChangeImage(object sender, RoutedEventArgs e)
        {
            IChangeImageService service = new OpencvChangeImageService();
            if (Brightness.IsChecked == true)
            {
                OutputBitmap = service.Brightness(InputBitmap, Value.Value);
            }
            else
            {
                OutputBitmap = service.Contrast(InputBitmap, Value.Value);
            }

        }
    }
}
