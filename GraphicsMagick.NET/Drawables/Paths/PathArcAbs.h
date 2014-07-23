//=================================================================================================
// Copyright 2014 Dirk Lemstra <https://graphicsmagick.codeplex.com/>
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

#include "Args\PathArc.h"
#include "Base\PathWrapper.h"

namespace GraphicsMagick
{
	///=============================================================================================
	///<summary>
	/// Encapsulation of the PathArcAbs object (Elliptical Arc).
	///</summary>
	public ref class PathArcAbs sealed : PathWrapper<Magick::PathArcAbs>
	{
		//===========================================================================================
	public:
		///==========================================================================================
		///<summary>
		/// Initializes a new instance of the PathArcAbs class.
		///</summary>
		///<param name="pathArc">The coordinate to use.</param>
		PathArcAbs(PathArc^ pathArc);
		///==========================================================================================
		///<summary>
		/// Initializes a new instance of the PathArcAbs class.
		///</summary>
		///<param name="pathArcs">The coordinates to use.</param>
		PathArcAbs(IEnumerable<PathArc^>^ pathArcs);
		//===========================================================================================
	};
	//==============================================================================================
}