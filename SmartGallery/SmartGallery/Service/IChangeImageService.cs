using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;

namespace SmartGallery.Service
{
    public interface IChangeImageService
    {
        SoftwareBitmap Contrast(SoftwareBitmap bitmap, double value);
        SoftwareBitmap Brightness(SoftwareBitmap bitmap, double value);
    }
}
