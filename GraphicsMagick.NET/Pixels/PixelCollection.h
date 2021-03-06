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

#include "Base\PixelBaseCollection.h"

namespace GraphicsMagick
{
	///=============================================================================================
	///<summary>
	/// Class that can be used to access the individual pixels of an image.
	///</summary>
	public ref class PixelCollection sealed : PixelBaseCollection
	{
		//===========================================================================================
	private:
		const Magick::PixelPacket* _Pixels;
		//===========================================================================================
	protected private:
		//===========================================================================================
		property const Magick::PixelPacket* Pixels
		{
			virtual const Magick::PixelPacket* get() override sealed
			{
				return _Pixels;
			}
		}
		//===========================================================================================
	internal:
		//===========================================================================================
		PixelCollection(Magick::Image* image, int x, int y, int width, int height);
		//===========================================================================================
	};
	//==============================================================================================
}