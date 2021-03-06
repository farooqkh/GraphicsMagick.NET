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

#include "Enums\MagickFormat.h"

using namespace System::Collections::Generic;
using namespace System::Collections::ObjectModel;

namespace GraphicsMagick
{
	///=============================================================================================
	///<summary>
	/// Class that contains information about an image format.
	///</summary>
	public ref class MagickFormatInfo sealed
	{
		//===========================================================================================
	private:
		//===========================================================================================
		static initonly List<String^>^ _CustomStealthFormats = gcnew List<String^>(gcnew array<String^>
		{
			"8BIM", "8BIMTEXT", "8BIMWTEXT", "APP1", "APP1JPEG", "CACHE", "EXIF", "ICM", "ICC", "IMAGE",
				"IPTC", "IPTCTEXT", "IPTCWTEXT", "XMP"
		});
		//===========================================================================================
		String^ _Description;
		MagickFormat _Format;
		Magick::CoderInfo* _CoderInfo;
		//===========================================================================================
		MagickFormatInfo() {};
		//===========================================================================================
		static void AddStealthCoder(std::list<Magick::CoderInfo>* coderList, std::string name);
		//===========================================================================================
		static Collection<MagickFormatInfo^>^ LoadFormats();
		//===========================================================================================
	internal:
		//===========================================================================================
		static initonly Collection<MagickFormatInfo^>^ All = LoadFormats();
		//===========================================================================================
	public:
		///==========================================================================================
		///<summary>
		/// The description of the format.
		///</summary>
		property String^ Description
		{
			String^ get();
		}
		///==========================================================================================
		///<summary>
		/// The format.
		///</summary>
		property MagickFormat Format
		{
			MagickFormat get();
		}
		///==========================================================================================
		///<summary>
		/// Format supports multiple frames.
		///</summary>
		property bool IsMultiFrame
		{
			bool get();
		}
		///==========================================================================================
		///<summary>
		/// Format is readable.
		///</summary>
		property bool IsReadable
		{
			bool get();
		}
		///==========================================================================================
		///<summary>
		/// Format is writable.
		///</summary>
		property bool IsWritable
		{
			bool get();
		}
		///==========================================================================================
		///<summary>
		/// Returns a string that represents the current format.
		///</summary>
		virtual String^ ToString() override;
		//===========================================================================================
	};
	//==============================================================================================
}