// TweakableEverything
//
// TweakableTools.cs
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
using System.Linq;
using System.Collections.Generic;
using ToadicusTools;
using UnityEngine;

namespace ToadicusTools.TweakableTools
{
	public static class TweakableTools
	{
		public static void InitializeTweakable<T>(
			UI_Control floatTweakable,
			ref float localField,
			ref float remoteField,
			float centerValue,
			float lowerMult,
			float upperMult,
			float stepMult,
			bool clobberEverywhere = false
		)
		{
            if (floatTweakable == null)
			{
				Logging.PostErrorMessage("Got null Control during InitializeTweakable for type {0}; bailing out.",
					typeof(T).FullName);

				return;
			}

			Vector2 bounds;

			float minValue, maxValue, stepIncrement;

			bounds = LoadBounds<T>();
			stepMult = LoadStep<T>(stepMult);

			// If our field is uninitialized...
			if (localField < 0)
			{
				// ...fetch it from the remote field
				localField = centerValue;
			}
			lowerMult = Mathf.Max(lowerMult, bounds.x, 0);
			upperMult = Mathf.Max(lowerMult, Mathf.Min(upperMult, bounds.y));

			// Set the bounds and increment for our tweakable range.
			if (centerValue < 0)
			{
				maxValue = centerValue * lowerMult;
				minValue = centerValue * upperMult;
			}
			else
			{
				minValue = centerValue * lowerMult;
				maxValue = centerValue * upperMult;
			}

			stepIncrement = Mathf.Pow(10f, Mathf.RoundToInt(Mathf.Log10(Mathf.Abs(centerValue))) - 1);
			stepIncrement *= stepMult;

            if (floatTweakable is UI_FloatRange)
			{
				UI_FloatRange floatRange = floatTweakable as UI_FloatRange;

				floatRange.maxValue = maxValue;
				floatRange.minValue = minValue;
				floatRange.stepIncrement = stepIncrement;
			}
			else
			{
                Logging.PostErrorMessage(
					"InitializeTweakable<{0}>: Got floatTweakable of type {1}, expected {2}",
					typeof(T).FullName,
					floatTweakable.GetType().FullName,
					typeof(UI_FloatRange).FullName
				);

				return;
			}

            localField = Mathf.Clamp(localField, minValue, maxValue);

			if (HighLogic.LoadedSceneIsFlight || clobberEverywhere)
			{
				// Clobber the remote field with ours.
				remoteField = localField;
			}
		}

		public static void InitializeTweakable<T>(
			UI_Control floatRange,
			ref float localField,
			ref float remoteField,
			float centerValue,
			float lowerMult,
			float upperMult,
			bool clobberEverywhere = false
		)
		{
			InitializeTweakable<T>(
				floatRange,
				ref localField,
				ref remoteField,
				centerValue,
				lowerMult,
				upperMult,
				1f,
				false
			);
		}

		public static void InitializeTweakable<T>(
			UI_Control floatRange,
			ref float localField,
			ref float remoteField,
			float centerValue,
			bool clobberEverywhere = false
		)
		{

			Logging.PostLogMessage("Initializetweakable, <T>: " + typeof(T) + ", localField: " + localField + ", remoteField: " + remoteField + ", centerValue: " + centerValue);

            InitializeTweakable<T>(
				floatRange,
				ref localField,
				ref remoteField,
				centerValue,
				0f,
				2f,
				1f,
				clobberEverywhere
			);
		}

		public static void InitializeTweakable<T>(
			UI_Control floatRange,
			ref float localField,
			ref float remoteField,
			bool clobberEverywhere = false
		)
		{
			InitializeTweakable<T>(floatRange, ref localField, ref remoteField, remoteField, clobberEverywhere);
		}

		public static Vector2 LoadBounds<T>()
		{
			Vector2 bounds = new Vector2(float.NegativeInfinity, float.PositiveInfinity);

			try
			{
				KSP.IO.PluginConfiguration config = KSP.IO.PluginConfiguration.CreateForType<T>();

				config.load();

				bounds = config.GetValue("bounds", bounds);

				config.save();
			}
			catch (Exception e)
			{
				Logging.PostErrorMessage(
					"{0} handled while loading PluginData for type {1}: do you have a malformed XML file?",
					e.GetType().FullName,
					typeof(T).Name
				);

				Logging.PostDebugMessage(e.ToString());
			}

			return bounds;
		}

		public static float LoadStep<T>(float defStep = 1f)
		{
			double stepMult;

			stepMult = (double)defStep;

			try
			{
				KSP.IO.PluginConfiguration config = KSP.IO.PluginConfiguration.CreateForType<T>();

				config.load();

				stepMult = config.GetValue("stepMult", stepMult);

				config.save();
			}
			catch (Exception e)
			{
				Logging.PostErrorMessage(
					"{0} handled while loading PluginData for type {1}: do you have a malformed XML file?",
					e.GetType().FullName,
					typeof(T).Name
				);

				Logging.PostDebugMessage(e.ToString());
			}

			return (float)stepMult;
		}
	}
}
