// ToadicusTools
//
// TextureTools.cs
//
// Copyright © 2014-2015, toadicus
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// 1. Redistributions of source code must retain the above copyright notice,
//    this list of conditions and the following disclaimer.
//
// 2. Redistributions in binary form must reproduce the above copyright notice,
//    this list of conditions and the following disclaimer in the documentation and/or other
//    materials provided with the distribution.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,
// INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
// WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using KSP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ToadicusTools.Text;
using UnityEngine;
using System.Reflection;

namespace ToadicusTools
{
	public static class IOTools
	{
		//public static readonly string KSPRootPath = KSPUtil.ApplicationRootPath.Replace("\\", "/");
		//public static readonly string GameDataPath = string.Format("{0}GameData/", GetRootPath);

		public static string KSPRootPath
		{
			get
			{
				return Assembly.GetExecutingAssembly().Location;
			}
		}
		public static string GameDataPath
		{ 
			get
			{
				var rootPath = Assembly.GetExecutingAssembly().Location;
				var gameDataIndex = rootPath.IndexOf("GameData", StringComparison.CurrentCultureIgnoreCase);
				return Path.Combine(rootPath.Remove(gameDataIndex, rootPath.Length - gameDataIndex), "GameData");
			}
		}
		public static bool LoadTexture(
			out Texture2D texture,
			string path,
			int width,
			int height,
			TextureFormat format,
			bool mipmap,
			bool isGameDataRelative = true
		)
		{
			string url = string.Empty;

			if (isGameDataRelative)
			{
				url = path;
				path = string.Format("{0}{1}", GameDataPath, path);
			}

			bool success = false;

			if (File.Exists(path))
			{
				texture = new Texture2D(width, height, format, mipmap);

				if (texture.LoadImage(File.ReadAllBytes(path)))
				{
					success = true;
				}
			}
			else
			{
				using (PooledStringBuilder sb = PooledStringBuilder.Get())
				{

					sb.AppendFormat("ToadicusTools.IOTools.LoadTexture: specified file '{0}' did not exist.", path);

					if (url == string.Empty)
					{
						url = path.Substring(GameDataPath.Length);
					}

					texture = null;

					try
					{
						string extension = Path.GetExtension(url);

						if (extension != string.Empty)
						{
							url = url.Substring(0, url.Length - extension.Length);
						}

						sb.AppendFormat("  Attempting falling back to GameDatabase.GetTexture from URL '{0}'...", url);

						texture = GameDatabase.Instance.GetTexture(url, false);
					}
					finally
					{
						success = !(texture == null);

						sb.Append(success ? " success!" : " failed!");

						Debug.LogWarning(sb.ToString());
					}
				}
			}

			return success;
		}

		public static bool LoadTexture(out Texture2D texture, string path, int width, int height)
		{
			return LoadTexture(out texture, path, width, height, TextureFormat.ARGB32, true, true);
		}
	}
}

