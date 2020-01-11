// ToadicusTools
//
// ComponentExtensions.cs
//
// Copyright © 2015, toadicus
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

using System;
using ToadicusTools.Text;
using UnityEngine;

namespace ToadicusTools.Extensions
{
	public static class ComponentExtensions
	{

		public static void Log(this Component component, LogChannel channel, string Msg)
		{
			Type componentType = component.GetType();
			string name;

			if (componentType == typeof(Vessel))
			{
				name = string.Format(Text.SIFormatProvider.SIFormatter, "{0} ({1})", componentType.Name, (component as Vessel).vesselName);
			}
			else if (componentType == typeof(Part))
			{
				name = string.Format(Text.SIFormatProvider.SIFormatter, "{0} ({1})", componentType.Name, (component as Part).partInfo.name);
			}
			else
			{
				name = componentType.Name;
			}

			string message = string.Format(Text.SIFormatProvider.SIFormatter, "[{0}] {1}", name, Msg);

			Logging.PostLogMessage(channel, message);
		}

		public static void Log(this Component component, string Msg)
		{
			component.Log(LogChannel.Log, Msg);
		}

		public static void Log(this Component component, string format, params object[] args)
		{
			string message = string.Format(Text.SIFormatProvider.SIFormatter, format, args);

			component.Log(message);
		}

		public static void LogWarning(this Component component, string Msg)
		{
			component.Log(LogChannel.Warning, Msg);
		}

		public static void LogWarning(this Component component, string format, params object[] args)
		{
			string message = string.Format(Text.SIFormatProvider.SIFormatter, format, args);

			component.LogWarning(message);
		}

		public static void LogError(this Component component, string Msg)
		{
			component.Log(LogChannel.Error, Msg);
		}

		public static void LogError(this Component component, string format, params object[] args)
		{
			string message = string.Format(Text.SIFormatProvider.SIFormatter, format, args);

			component.LogError(message);
		}

		[System.Diagnostics.Conditional("DEBUG")]
		public static void LogDebug(this Component component, string Msg)
		{
			component.Log(LogChannel.Log, Msg);
		}

		[System.Diagnostics.Conditional("DEBUG")]
		public static void LogDebug(this Component component, string format, params object[] args)
		{
			string message = string.Format(Text.SIFormatProvider.SIFormatter, format, args);
			Logging.PostLogMessage(message);
			component.Log(message);
		}

	}
}

