//=================================================================================================
// Copyright 2014-2015 Dirk Lemstra <https://graphicsmagick.codeplex.com/>
//
// Licensed under the ImageMagick License (the "License"); you may not use this file except in 
// compliance with the License. You may obtain a copy of the License at
//
//   http://www.imagemagick.org/script/license.php
//
// Unless required by applicable law or agreed to in writing, software distributed under the
// License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either
// express or implied. See the License for the specific language governing permissions and
// limitations under the License.
//=================================================================================================
#include "Stdafx.h"
#include "MagickErrorInfo.h"

namespace GraphicsMagick
{
	//==============================================================================================
	MagickErrorInfo::MagickErrorInfo()
	{
		_MeanErrorPerPixel = 0.0;
		_NormalizedMaximumError = 0.0;
		_NormalizedMeanError = 0.0;
	}
	//==============================================================================================
	MagickErrorInfo::MagickErrorInfo(Magick::Image* image)
	{
		_MeanErrorPerPixel = image->meanErrorPerPixel();
		_NormalizedMaximumError = image->normalizedMaxError();
		_NormalizedMeanError = image->normalizedMeanError();
	}
	//==============================================================================================
	double MagickErrorInfo::MeanErrorPerPixel::get()
	{
		return _MeanErrorPerPixel;
	}
	//==============================================================================================
	double MagickErrorInfo::NormalizedMaximumError::get()
	{
		return _NormalizedMaximumError;
	}
	//==============================================================================================
	double MagickErrorInfo::NormalizedMeanError::get()
	{
		return _NormalizedMeanError;
	}
	//==============================================================================================
}