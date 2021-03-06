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
#include "Pixel.h"

namespace GraphicsMagick
{
	//==============================================================================================
	Pixel::Pixel()
	{
	}
	//==============================================================================================
	void Pixel::CheckChannels(int channels)
	{
		Throw::IfTrue("value", channels < 1 || channels > 5, "Invalid number of channels (supported sizes are 1-5).");
	}
	//==============================================================================================
	void Pixel::Initialize(int x, int y, array<Magick::Quantum>^ value)
	{
		_X = x;
		_Y = y;
		_Value = value;
	}
	//==============================================================================================
	array<Magick::Quantum>^ Pixel::Value::get()
	{
		return _Value;
	}
	//==============================================================================================
	Pixel^ Pixel::Create(int x, int y, array<Magick::Quantum>^ value)
	{
		Pixel^ pixel = gcnew Pixel();
		pixel->Initialize(x, y, value);
		return pixel;
	}
	//==============================================================================================
	Pixel::Pixel(int x, int y, int channels)
	{
		CheckChannels(channels);

		Initialize(x, y, gcnew array<Magick::Quantum>(channels));
	}
	//==============================================================================================
	Pixel::Pixel(int x, int y, array<Magick::Quantum>^ value)
	{
		Throw::IfNull("value", value);
		CheckChannels(value->Length);

		Initialize(x, y, value);
	}
	//==============================================================================================
	Magick::Quantum Pixel::default::get(int channel)
	{
		return GetChannel(channel);
	}
	//==============================================================================================
	void Pixel::default::set(int channel, Magick::Quantum value)
	{
		SetChannel(channel, value);
	}
	//==============================================================================================
	int Pixel::Channels::get()
	{
		return _Value->Length;
	}
	//==============================================================================================
	int Pixel::X::get()
	{
		return _X;
	}
	//==============================================================================================
	void Pixel::X::set(int value)
	{
		if (value < 0)
			return;

		_X = value;
	}
	//==============================================================================================
	int Pixel::Y::get()
	{
		return _Y;
	}
	//==============================================================================================
	void Pixel::Y::set(int value)
	{
		if (value < 0)
			return;

		_Y = value;
	}
	//==============================================================================================
	bool Pixel::operator == (Pixel^ left, Pixel^ right)
	{
		return Object::Equals(left, right);
	}
	//==============================================================================================
	bool Pixel::operator != (Pixel^ left, Pixel^ right)
	{
		return !Object::Equals(left, right);
	}
	//==============================================================================================
	bool Pixel::Equals(Object^ obj)
	{
		if (ReferenceEquals(this, obj))
			return true;

		return Equals(dynamic_cast<Pixel^>(obj));
	}
	//==============================================================================================
	bool Pixel::Equals(Pixel^ other)
	{
		if (ReferenceEquals(other, nullptr))
			return false;

		if (ReferenceEquals(this, other))
			return true;

		if (Channels != other->Channels)
			return false;

		for(int i=0; i < _Value->Length; i++)
		{
			if (_Value[i] != other->_Value[i])
				return false;
		}

		return true;
	}
	//==============================================================================================
	Magick::Quantum Pixel::GetChannel(int channel)
	{
		if (channel < 0 || channel >= _Value->Length)
			return 0;

		return _Value[channel];
	}
	//==============================================================================================
	int Pixel::GetHashCode()
	{
		return Value->GetHashCode();
	}
	//==============================================================================================
	void Pixel::SetChannel(int channel, Magick::Quantum value)
	{
		if (channel < 0 || channel >= _Value->Length)
			return;

		_Value[channel] = value;
	}
	//==============================================================================================
	MagickColor^ Pixel::ToColor()
	{
		if (_Value->Length == 3)
			return gcnew MagickColor(_Value[0], _Value[1], _Value[2]);

		if (_Value->Length >= 4)
			return gcnew MagickColor(_Value[0], _Value[1], _Value[2], _Value[3]);

		return nullptr;
	}
	//==============================================================================================
}