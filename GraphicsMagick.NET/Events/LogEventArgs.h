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
#include "..\Enums\ExceptionTypes.h"
#include "..\Exceptions\Base\MagickException.h"

namespace GraphicsMagick
{
	///=============================================================================================
	///<summary>
	/// EventArgs for Log events.
	///</summary>
	public ref class LogEventArgs sealed : EventArgs
	{
		//===========================================================================================
	private:
		ExceptionTypes _EventType;
		String^ _Message;
		//===========================================================================================
	internal:
		//===========================================================================================
		LogEventArgs(ExceptionTypes eventType, String^ message);
		//===========================================================================================
	public:
		///==========================================================================================
		///<summary>
		/// Type of the log message.
		///</summary>
		property String^ Message
		{
			String^ get();
		}
		///==========================================================================================
		///<summary>
		/// Type of the log message.
		///</summary>
		property ExceptionTypes EventType
		{
			ExceptionTypes get();
		}
		//===========================================================================================
	};
	//==============================================================================================
}