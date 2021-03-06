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
using System.Collections.Generic;
using System.Configuration;
using System.Web;

namespace GraphicsMagick.Web
{
	///=============================================================================================
	/// <summary>
	/// Httpmodule that uses MagickScript to resize images.
	/// </summary>
	public sealed class MagickScriptModule : IHttpModule
	{
		//===========================================================================================
		private static IEnumerable<IUrlResolver> _ScriptUrlResolvers
		{
			get
			{
				foreach (UrlResolverSettings settings in MagickWebSettings.UrlResolvers)
				{
					yield return settings.CreateInstance();
				}
			}
		}
		//===========================================================================================
		private static IHttpHandler HandleRequest(HttpContext context)
		{
			Uri url = (Uri)context.Items["MagickScriptModule.Url"];

			foreach (IUrlResolver scriptUrlResolver in _ScriptUrlResolvers)
			{
				if (scriptUrlResolver.Resolve(url))
					return CreateHttpHandler(scriptUrlResolver);
			}

			return null;
		}
		//===========================================================================================
		private static IHttpHandler CreateHttpHandler(IUrlResolver urlResolver)
		{
			MagickScriptHandler scriptHandler = new MagickScriptHandler(urlResolver);
			return scriptHandler.IsValid ? scriptHandler : null;
		}
		//===========================================================================================
		private void OnBeginRequest(object sender, EventArgs arguments)
		{
			HttpContext context = ((HttpApplication)sender).Context;
			context.Items["MagickScriptModule.Url"] = context.Request.Url;
		}
		//===========================================================================================
		private void OnPostAuthorizeRequest(object sender, EventArgs arguments)
		{
			HttpContext context = ((HttpApplication)sender).Context;
			IHttpHandler newHandler = HandleRequest(context);
			if (newHandler != null)
				context.RemapHandler(newHandler);
		}
		//===========================================================================================
		private void OnPostMapRequestHandler(object sender, EventArgs arguments)
		{
			HttpContext context = ((HttpApplication)sender).Context;
			IHttpHandler newHandler = HandleRequest(context);
			if (newHandler != null)
				context.Handler = newHandler;
		}
		///==========================================================================================
		/// <summary>
		/// Initializes the module and prepares it to handle requests.
		/// </summary>
		/// <param name="context">An HttpApplication that provides access to the methods, properties,
		/// and events common to all application objects within an ASP.NET application</param>
		public void Init(HttpApplication context)
		{
			if (context == null)
				return;

			if (MagickWebSettings.UrlResolvers.Count == 0)
				throw new ConfigurationErrorsException("Define at least one url resolver.");

			context.BeginRequest += OnBeginRequest;

			if (HttpRuntime.UsingIntegratedPipeline)
				context.PostAuthorizeRequest += OnPostAuthorizeRequest;
			else
				context.PostMapRequestHandler += OnPostMapRequestHandler;
		}
		///==========================================================================================
		/// <summary>
		/// Disposes of the resources (other than memory) used by the module.
		/// </summary>
		public void Dispose()
		{
		}
		//===========================================================================================
	}
	//==============================================================================================
}
