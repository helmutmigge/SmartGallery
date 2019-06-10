#include <stdexcept>
#include "pch.h"
#include "ChangeImage.h"
#include "MemoryBuffer.h"

using namespace Windows::Graphics::Imaging;
using namespace Windows::Foundation;

using namespace SmartGalleryRuntime;
using namespace Platform;
using namespace Microsoft::WRL;
using namespace cv;

void SmartGalleryRuntime::ChangeImage::Brightness(Windows::Graphics::Imaging::SoftwareBitmap ^ input, Windows::Graphics::Imaging::SoftwareBitmap ^ output, double value)
{
	if (input->BitmapPixelFormat != Windows::Graphics::Imaging::BitmapPixelFormat::Bgra8)
	{
		throw ref new Platform::InvalidArgumentException("SoftwareBitmap must be in the format Rgba8");
	}
	Mat inputMat, brighnessMat;

	if (!(TryConvert(input, inputMat) && TryConvert(output, brighnessMat)))
	{
		throw ref new Platform::InvalidArgumentException("SoftwareBitmap imput fail");
	}
	inputMat.convertTo(brighnessMat, -1, 1, value);
}

void SmartGalleryRuntime::ChangeImage::Contrast(Windows::Graphics::Imaging::SoftwareBitmap ^ input, Windows::Graphics::Imaging::SoftwareBitmap ^ output, double value)
{
	if (input->BitmapPixelFormat != Windows::Graphics::Imaging::BitmapPixelFormat::Bgra8)
	{
		throw ref new Platform::InvalidArgumentException("SoftwareBitmap must be in the format Rgba8");
	}
	Mat inputMat, brighnessMat;

	if (!(TryConvert(input, inputMat) && TryConvert(output, brighnessMat)))
	{
		throw ref new Platform::InvalidArgumentException("SoftwareBitmap imput fail");
	}
	inputMat.convertTo(brighnessMat, -1, value, 0);
}

bool SmartGalleryRuntime::ChangeImage::TryConvert(Windows::Graphics::Imaging::SoftwareBitmap ^ from, cv::Mat & convertedMat)
{
	unsigned char* pPixels = nullptr;
		unsigned int capacity = 0;
		if (!GetPointerToPixelData(from, &pPixels, &capacity))
		{
			return false;
		}

	Mat mat(from->PixelHeight,
		from->PixelWidth,
		CV_8UC4, // assume input SoftwareBitmap is BGRA8
		(void*)pPixels);

	// shallow copy because we want convertedMat.data = pPixels
	// don't use .copyTo or .clone
	convertedMat = mat;
	return true;
}

bool SmartGalleryRuntime::ChangeImage::GetPointerToPixelData(Windows::Graphics::Imaging::SoftwareBitmap ^ bitmap, unsigned char ** pPixelData, unsigned int * capacity)
{
	BitmapBuffer^ bmpBuffer = bitmap->LockBuffer(BitmapBufferAccessMode::ReadWrite);
	IMemoryBufferReference^ reference = bmpBuffer->CreateReference();
	ComPtr<IMemoryBufferByteAccess> pBufferByteAccess;
	if ((reinterpret_cast<IInspectable*>(reference)->QueryInterface(IID_PPV_ARGS(&pBufferByteAccess))) != S_OK)
	{
		return false;
	}

	if (pBufferByteAccess->GetBuffer(pPixelData, capacity) != S_OK)
	{
		return false;
	}
	return true;
}
