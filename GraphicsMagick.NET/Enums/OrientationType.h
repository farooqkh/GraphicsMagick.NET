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
#pragma once

#include "Stdafx.h"

namespace GraphicsMagick
{
	///=============================================================================================
	///<summary>
	/// Specified the photo orientation of the image.
	///</summary>
	public enum class OrientationType
	{
		Undefined = Magick::UndefinedOrientation,
		TopLeft = Magick::TopLeftOrientation,
		TopRight = Magick::TopRightOrientation,
		BottomRight = Magick::BottomRightOrientation,
		BottomLeft = Magick::BottomLeftOrientation,
		LeftTop = Magick::LeftTopOrientation,
		RightTop = Magick::RightTopOrientation,
		RightBottom = Magick::RightBottomOrientation,
		LeftBotom = Magick::LeftBottomOrientation
	};
	//==============================================================================================
}
