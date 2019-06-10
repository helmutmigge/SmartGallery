#pragma once

#include <opencv2\core\core.hpp>
#include <opencv2\imgproc\imgproc.hpp>
namespace SmartGalleryRuntime
{
    public ref class ChangeImage sealed
    {
    public:
		void Brightness(Windows::Graphics::Imaging::SoftwareBitmap^ input, Windows::Graphics::Imaging::SoftwareBitmap^ output, double value);
		void Contrast(Windows::Graphics::Imaging::SoftwareBitmap^ input, Windows::Graphics::Imaging::SoftwareBitmap^ output, double value);
	private:
		bool TryConvert(Windows::Graphics::Imaging::SoftwareBitmap^ from, cv::Mat& convertedMat);

		bool GetPointerToPixelData(Windows::Graphics::Imaging::SoftwareBitmap^ bitmap,unsigned char** pPixelData, unsigned int* capacity);
	};
}
