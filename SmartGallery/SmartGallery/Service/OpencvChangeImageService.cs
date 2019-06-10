using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using SmartGalleryRuntime;

namespace SmartGallery.Service
{
    public class OpencvChangeImageService: IChangeImageService
    {
        private readonly ChangeImage _changeImage;

        public OpencvChangeImageService()
        {
            this._changeImage = new ChangeImage();
        }

        public SoftwareBitmap Contrast(SoftwareBitmap input, double value)
        {
            SoftwareBitmap output = new SoftwareBitmap(input.BitmapPixelFormat, input.PixelWidth, input.PixelHeight, BitmapAlphaMode.Premultiplied);
            _changeImage.Contrast(input,output,value);
            return output;

        }

        public SoftwareBitmap Brightness(SoftwareBitmap input, double value)
        {
            SoftwareBitmap output = new SoftwareBitmap(input.BitmapPixelFormat, input.PixelWidth, input.PixelHeight, BitmapAlphaMode.Premultiplied);
            _changeImage.Brightness(input, output, value);
            return output;
        }
    }
}
