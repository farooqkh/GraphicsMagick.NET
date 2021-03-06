﻿//=================================================================================================
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

using System;
using GraphicsMagick;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GraphicsMagick.NET.Tests
{
	//==============================================================================================
	[TestClass]
	public class ColorMatrixTests
	{
		//===========================================================================================
		private const string _Category = "ColorMatrix";
		//===========================================================================================
		[TestMethod, TestCategory(_Category)]
		public void Test_Constructor()
		{
			ExceptionAssert.Throws<ArgumentException>(delegate()
			{
				new ColorMatrix(-1);
			});

			ExceptionAssert.Throws<ArgumentException>(delegate()
			{
				new ColorMatrix(7);
			});

			new ColorMatrix(1);

			ExceptionAssert.Throws<ArgumentException>(delegate()
			{
				new ColorMatrix(2, 1.0);
			});

			ColorMatrix matrix = new ColorMatrix(2, 0.0, 1.0, 0.1, 1.1);
			Assert.AreEqual(0.0, matrix.GetValue(0, 0));
			Assert.AreEqual(1.0, matrix.GetValue(1, 0));
			Assert.AreEqual(0.1, matrix.GetValue(0, 1));
			Assert.AreEqual(1.1, matrix.GetValue(1, 1));
		}
		//===========================================================================================
		[TestMethod, TestCategory(_Category)]
		public void Test_SetColumn()
		{
			ColorMatrix matrix = new ColorMatrix(2);

			matrix.SetColumn(0, 2, 4);
			Assert.AreEqual(2, matrix.GetValue(0, 0));
			Assert.AreEqual(4, matrix.GetValue(0, 1));

			matrix.SetColumn(1, 6, 8);
			Assert.AreEqual(6, matrix.GetValue(1, 0));
			Assert.AreEqual(8, matrix.GetValue(1, 1));
		}
		//===========================================================================================
		[TestMethod, TestCategory(_Category)]
		public void Test_SetRow()
		{
			ColorMatrix matrix = new ColorMatrix(2);

			matrix.SetRow(0, 2, 4);
			Assert.AreEqual(2, matrix.GetValue(0, 0));
			Assert.AreEqual(4, matrix.GetValue(1, 0));

			matrix.SetRow(1, 6, 8);
			Assert.AreEqual(6, matrix.GetValue(0, 1));
			Assert.AreEqual(8, matrix.GetValue(1, 1));
		}
		//===========================================================================================
		[TestMethod, TestCategory(_Category)]
		public void Test_Value()
		{
			ColorMatrix matrix = new ColorMatrix(2);

			matrix.SetValue(0, 0, 1.5);
			Assert.AreEqual(1.5, matrix.GetValue(0, 0));

			Assert.AreEqual(0.0, matrix.GetValue(3, 1));
			Assert.AreEqual(0.0, matrix.GetValue(1, 3));
		}
		//===========================================================================================
	}
	//==============================================================================================
}
