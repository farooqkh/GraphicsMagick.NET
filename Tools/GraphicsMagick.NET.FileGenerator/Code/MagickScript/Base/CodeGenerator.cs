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
using System.CodeDom.Compiler;
using System.Linq;
using System.Reflection;

namespace GraphicsMagick.NET.FileGenerator
{
	//==============================================================================================
	internal abstract class CodeGenerator
	{
		//===========================================================================================
		private GraphicsMagickNET _GraphicsMagickNET;
		//===========================================================================================
		private void WriteAttributeForEach(IndentedTextWriter writer, ParameterInfo[] allParameters)
		{
			ParameterInfo[] parameters = allParameters.Where(p => _GraphicsMagickNET.GetXsdAttributeType(p) != null).ToArray();
			if (parameters.Length == 0)
				return;

			parameters = parameters.OrderBy(p => p.Name).ToArray();

			writer.WriteLine("for each(XmlAttribute^ attribute in element->Attributes)");
			WriteStartColon(writer);

			if (parameters.DistinctBy(p => _GraphicsMagickNET.GetCppTypeName(p)).Count() == 1)
			{
				writer.Write("arguments[attribute->Name] = _Variables->GetValue<");
				writer.Write(_GraphicsMagickNET.GetCppTypeName(parameters[0]));
				writer.WriteLine(">(attribute);");
			}
			else
			{
				for (int i = 0; i < parameters.Length; i++)
				{
					string xsdName = _GraphicsMagickNET.GetXsdName(parameters[i]);

					if (i > 0)
						writer.Write("else ");

					writer.Write("if (attribute->Name == \"");
					writer.Write(xsdName);
					writer.WriteLine("\")");
					writer.Indent++;
					writer.Write("arguments[\"");
					writer.Write(xsdName);
					writer.Write("\"] = ");
					WriteGetAttributeValue(writer, _GraphicsMagickNET.GetCppTypeName(parameters[i]), xsdName);
					writer.Indent--;
				}
			}

			WriteEndColon(writer);
		}
		//===========================================================================================
		private void WriteCallIfElse(IndentedTextWriter writer, MethodBase[] methods)
		{
			for (int i = 0; i < methods.Length; i++)
			{
				ParameterInfo[] parameters = methods[i].GetParameters();

				if (i > 0)
					writer.Write("else ");

				writer.Write("if (");
				if (parameters.Length > 0)
				{
					writer.Write("OnlyContains(arguments");
					foreach (ParameterInfo parameter in parameters)
					{
						writer.Write(", \"");
						writer.Write(parameter.Name);
						writer.Write("\"");
					}
					writer.Write(")");
				}
				else
				{
					writer.Write("arguments->Count == 0");
				}

				writer.WriteLine(")");
				writer.Indent++;
				WriteHashtableCall(writer, methods[i], parameters);
				writer.Indent--;
			}
		}
		//===========================================================================================
		private void WriteElementForEach(IndentedTextWriter writer, ParameterInfo[] allParameters)
		{
			ParameterInfo[] parameters = allParameters.Where(p => _GraphicsMagickNET.GetXsdAttributeType(p) == null).ToArray();
			if (parameters.Length == 0)
				return;

			writer.WriteLine("for each(XmlElement^ elem in element->SelectNodes(\"*\"))");
			WriteStartColon(writer);

			if (parameters.DistinctBy(p => _GraphicsMagickNET.GetCppTypeName(p)).Count() == 1)
			{
				writer.Write("arguments[elem->Name] = ");
				WriteCreateMethod(writer, _GraphicsMagickNET.GetCppTypeName(parameters[0]));
				writer.WriteLine("(elem);");
			}
			else
			{
				for (int i = 0; i < parameters.Length; i++)
				{
					string xsdName = _GraphicsMagickNET.GetXsdName(parameters[i]);

					if (i > 0)
						writer.Write("else ");

					writer.Write("if (elem->Name == \"");
					writer.Write(xsdName);
					writer.WriteLine("\")");
					writer.Indent++;
					writer.Write("arguments[\"");
					writer.Write(xsdName);
					writer.Write("\"] = ");
					WriteCreateMethod(writer, _GraphicsMagickNET.GetCppTypeName(parameters[i]));
					writer.WriteLine("(elem);");
					writer.Indent--;
				}
			}

			WriteEndColon(writer);
		}
		//===========================================================================================
		private void WriteGetValue(IndentedTextWriter writer, ParameterInfo parameter)
		{
			string typeName = _GraphicsMagickNET.GetCppTypeName(parameter);
			string xsdTypeName = _GraphicsMagickNET.GetXsdAttributeType(parameter);

			if (xsdTypeName != null)
			{
				WriteGetElementValue(writer, typeName, parameter.Name);
			}
			else
			{
				WriteCreateMethod(writer, typeName);
				writer.Write("(");
				WriteSelectElement(writer, typeName, parameter.Name);
				writer.WriteLine(");");
			}
		}
		//===========================================================================================
		protected void WriteHashtableParameters(IndentedTextWriter writer, ParameterInfo[] parameters)
		{
			for (int k = 0; k < parameters.Length; k++)
			{
				writer.Write("(");
				writer.Write(_GraphicsMagickNET.GetCppTypeName(parameters[k]));
				writer.Write(")arguments[\"");
				writer.Write(parameters[k].Name);
				writer.Write("\"]");

				if (k != parameters.Length - 1)
					writer.Write(", ");
			}
		}
		//===========================================================================================
		private void WriteInvalidCombinations(IndentedTextWriter writer, MethodBase[] methods)
		{
			writer.WriteLine("else");
			writer.Indent++;
			writer.Write("throw gcnew ArgumentException(\"Invalid argument combination for '" + _GraphicsMagickNET.GetXsdName(methods[0]) + "', allowed combinations are:");
			foreach (MethodBase method in methods)
			{
				writer.Write(" [");
				ParameterInfo[] parameters = method.GetParameters();
				for (int i = 0; i < parameters.Length; i++)
				{
					writer.Write(parameters[i].Name);
					if (i != parameters.Length - 1)
						writer.Write(", ");
				}
				writer.Write("]");
			}
			writer.WriteLine("\");");
			writer.Indent--;
		}
		//===========================================================================================
		private void WriteMethod(IndentedTextWriter writer, MethodBase method, ParameterInfo[] parameters)
		{
			foreach (ParameterInfo parameter in parameters)
			{
				string typeName = MagickNET.GetCppTypeName(parameter);

				writer.Write(typeName);
				writer.Write(" ");
				writer.Write(parameter.Name);
				writer.Write("_ = ");
				WriteGetValue(writer, parameter);
			}

			WriteCall(writer, method, parameters);
		}
		//===========================================================================================
		private void WriteMethod(IndentedTextWriter writer, MethodBase[] methods, ParameterInfo[] parameters)
		{
			writer.WriteLine("System::Collections::Hashtable^ arguments = gcnew System::Collections::Hashtable();");

			WriteAttributeForEach(writer, parameters);
			WriteElementForEach(writer, parameters);
			WriteCallIfElse(writer, methods);
			WriteInvalidCombinations(writer, methods);
		}
		//===========================================================================================
		protected static void WriteParameters(IndentedTextWriter writer, ParameterInfo[] parameters)
		{
			for (int i = 0; i < parameters.Length; i++)
			{
				writer.Write(parameters[i].Name);
				writer.Write("_");

				if (i != parameters.Length - 1)
					writer.Write(", ");
			}
		}
		//===========================================================================================
		protected CodeGenerator()
		{
			_GraphicsMagickNET = new GraphicsMagickNET(QuantumDepth.Q16);
		}
		//===========================================================================================
		protected GraphicsMagickNET MagickNET
		{
			get
			{
				return _GraphicsMagickNET;
			}
		}
		//===========================================================================================
		protected abstract void WriteCall(IndentedTextWriter writer, MethodBase method, ParameterInfo[] parameters);
		//===========================================================================================
		protected abstract void WriteHashtableCall(IndentedTextWriter writer, MethodBase method, ParameterInfo[] parameters);
		//===========================================================================================
		protected static void WriteCreateMethod(IndentedTextWriter writer, string typeName)
		{
			switch (typeName)
			{
				case "array<double>^":
					writer.Write("_Variables->GetDoubleArray");
					break;
				case "Coordinate":
					writer.Write("CreateCoordinate");
					break;
				case "PathArc^":
					writer.Write("CreateArc");
					break;
				case "IEnumerable<Coordinate>^":
					writer.Write("CreateCoordinates");
					break;
				case "IEnumerable<PathBase^>^":
					writer.Write("CreatePaths");
					break;
				case "IEnumerable<PathArc^>^":
					writer.Write("CreatePathArcs");
					break;
				case "IEnumerable<PathCurveto^>^":
					writer.Write("CreatePathCurvetos");
					break;
				case "IEnumerable<PathQuadraticCurveto^>^":
					writer.Write("CreatePathQuadraticCurvetos");
					break;
				case "IEnumerable<SparseColorArg^>^":
					writer.Write("CreateSparseColorArgs");
					break;
				case "ImageProfile^":
					writer.Write("CreateProfile");
					break;
				case "MagickImage^":
					writer.Write("CreateMagickImage");
					break;
				case "MagickGeometry^":
					writer.Write("CreateMagickGeometry");
					break;
				case "MontageSettings^":
					writer.Write("CreateMontageSettings");
					break;
				case "PixelStorageSettings^":
					writer.Write("CreatePixelStorageSettings");
					break;
				case "QuantizeSettings^":
					writer.Write("CreateQuantizeSettings");
					break;
				default:
					throw new NotImplementedException(typeName);
			}
		}
		//===========================================================================================
		protected static void WriteEndColon(IndentedTextWriter writer)
		{
			writer.Indent--;
			writer.WriteLine("}");
		}
		//===========================================================================================
		protected static void WriteGetElementValue(IndentedTextWriter writer, string typeName, string attributeName)
		{
			writer.Write("_Variables->GetValue<");
			writer.Write(typeName);
			writer.Write(">(element, \"");
			writer.Write(attributeName);
			writer.WriteLine("\");");
		}
		//===========================================================================================
		protected static void WriteGetAttributeValue(IndentedTextWriter writer, string typeName, string attributeName)
		{
			writer.Write("_Variables->GetValue<");
			writer.Write(typeName);
			writer.WriteLine(">(attribute);");
		}
		//===========================================================================================
		protected void WriteMethod(IndentedTextWriter writer, MethodBase[] methods)
		{
			ParameterInfo[] parameters = (from method in methods
													from param in method.GetParameters()
													select param).DistinctBy(p => p.Name).ToArray();

			if (methods.Length == 1)
			{
				WriteMethod(writer, methods[0], parameters);
			}
			else
			{
				MethodBase[] sortedMethods = (from method in methods
														orderby string.Join(" ", from parameter in method.GetParameters()
																						 select parameter.Name)
														select method).ToArray();
				WriteMethod(writer, sortedMethods, parameters);
			}
		}
		//===========================================================================================
		protected static void WriteSelectElement(IndentedTextWriter writer, string typeName, string elementName)
		{
			switch (typeName)
			{
				case "array<double>^":
				case "MagickImage^":
					writer.Write("element");
					if (!string.IsNullOrEmpty(elementName))
					{
						writer.Write("[\"");
						writer.Write(elementName);
						writer.Write("\"]");
					}
					break;
				case "IEnumerable<Coordinate>^":
				case "IEnumerable<Drawable^>^":
				case "IEnumerable<PathBase^>^":
				case "IEnumerable<PathArc^>^":
				case "IEnumerable<PathCurveto^>^":
				case "IEnumerable<PathQuadraticCurveto^>^":
				case "ImageProfile^":
					writer.Write("element");
					break;
				case "MontageSettings^":
				case "PixelStorageSettings^":
				case "QuantizeSettings^":
					writer.Write("element[\"");
					writer.Write(elementName);
					writer.Write("\"]");
					break;
				default:
					throw new NotImplementedException(typeName);
			}
		}
		//===========================================================================================
		protected static void WriteStartColon(IndentedTextWriter writer)
		{
			writer.WriteLine("{");
			writer.Indent++;
		}
		//===========================================================================================
	}
	//==============================================================================================
}
