// ToadicusTools
//
// PartExtensions.cs
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
using KSP.UI.Screens;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ToadicusTools.Extensions
{
	public static class PartExtensions
	{
		[Obsolete("Deprecated, please use hasModuleByType.")]
		public static bool hasModuleType<T>(this Part part) where T : PartModule
		{
			return part.hasModuleByType<T>();
		}

		public static bool hasModuleByType<T>(this Part part) where T : PartModule
		{
			return part.hasModuleByType(typeof(T));
		}

		public static bool hasModuleByType(this Part part, Type type)
		{
			if (part == null)
			{
				throw new ArgumentNullException("part");
			}

			if (type == null)
			{
				throw new ArgumentNullException("type");
			}

			if (!typeof(PartModule).IsAssignableFrom(type))
			{
				Logging.PostWarningMessage(
					"[ToadicusTools] Part.hasModuleByType: '{0}' is not a derivative of PartModule",
					type.FullName
				);
				return false;
			}

#if DEBUG
			Logging.PostLogMessage("[hasModuleByType]: Falling back to linear search.");
			#endif

			if (part.Modules != null)
			{
#if DEBUG
				Logging.PostLogMessage("[hasModuleByType]: Part.modules is defined; checking PartModule subtypes.");
				#endif

				PartModule module;
				for (int mIdx = 0; mIdx < part.Modules.Count; mIdx++)
				{
					module = part.Modules[mIdx];
					if (type.IsAssignableFrom(module.GetType()))
					{
						return true;
					}
				}
			}
			else
			{
#if DEBUG
				Logging.PostLogMessage("[hasModuleByType]: Part.modules is not defined; trying ModuleInfo search.");
				#endif

				AvailablePart.ModuleInfo moduleInfo;
				for (int idx = 0; idx < part.partInfo.moduleInfos.Count; idx++)
				{
					moduleInfo = part.partInfo.moduleInfos[idx];
					if (moduleInfo.moduleName == type.Name)
					{
						return true;
					}
				}
			}

			return false;
		}

		public static bool hasModuleByName(this Part part, string moduleName)
		{
			if (part == null)
			{
				throw new ArgumentNullException("Part.hasModuleByName: 'part' argument must not be null");
			}

			if (string.IsNullOrEmpty(moduleName))
			{
				Logging.PostWarningMessage("[ToadicusTools]: Part.hasModuleByName: moduleName argument is null or empty");
				return false;
			}

			#if DEBUG
			Debug.Log(string.Format("Checking if part {0} has module(s) named {1}", part.partInfo.name, moduleName));
			#endif

			PartModule module;

			if (part.Modules != null)
			{
				for (int mIdx = 0; mIdx < part.Modules.Count; mIdx++)
				{
					module = part.Modules[mIdx];

					if (module == null)
					{
						continue;
					}

					if (module.moduleName == moduleName)
					{
						return true;
					}
				}
			}
			else
			{
				AvailablePart.ModuleInfo moduleInfo;
				for (int idx = 0; idx < part.partInfo.moduleInfos.Count; idx++)
				{
					moduleInfo = part.partInfo.moduleInfos[idx];
					if (moduleInfo.moduleName == moduleName)
					{
						return true;
					}
				}
			}

			return false;
		}

		public static List<T> getModulesOfType<T>(this Part part) where T : PartModule
		{
			if (part == null)
			{
				throw new ArgumentNullException(
					string.Format("Part.getModulesOfType<{0}>: 'part' argument must not be null", typeof(T).Name)
				);
			}

			List<T> returnList = new List<T>();


			if (part.Modules != null)
			{
				PartModule module;
				for (int mIdx = 0; mIdx < part.Modules.Count; mIdx++)
				{
					module = part.Modules[mIdx];
					if (module is T)
					{
						returnList.Add(module as T);
					}
				}
			}
			else
			{
				part.LogWarning("Modules list is null during module search; returning empty list.");
			}

			return returnList;
		}

		public static T getFirstModuleOfType<T>(this Part part) where T: PartModule
		{
			if (part == null)
			{
				throw new ArgumentNullException(
					string.Format("Part.getFirstModuleOfType<{0}>: 'part' argument must not be null", typeof(T).Name)
				);
			}

			if (part.Modules == null)
			{
				part.LogWarning("Modules list is null during module search; returning null.");

				return null;
			}

			PartModule module;
			for (int mIdx = 0; mIdx < part.Modules.Count; mIdx++)
			{
				module = part.Modules[mIdx];
				if (module is T)
				{
					return module as T;
				}
			}

			return null;
		}

		public static bool tryGetFirstModuleOfType<T>(this Part part, out T module) where T : PartModule
		{
			if (part == null)
			{
				throw new ArgumentNullException(
					string.Format("Part.tryGetFirstModuleOfType<{0}>: 'part' argument must not be null", typeof(T).Name)
				);
			}

			module = part.getFirstModuleOfType<T>();

			if (module == null)
			{
				return false;
			}

			return true;
		}

		public static PartModule getFirstModuleByName(this Part part, string moduleName)
		{
			PartModule module;
			if (part.tryGetFirstModuleByName(moduleName, out module))
			{
				return module;
			}

			return null;
		}

		public static bool tryGetFirstModuleByName(this Part part, string moduleName, out PartModule module)
		{
			if (part == null)
			{
				throw new ArgumentNullException("Part.getFirstModuleByName: 'part' argument must not be null");
			}

			for (int idx = 0; idx < part.Modules.Count; idx++)
			{
				module = part.Modules[idx];

				if (module.moduleName == moduleName)
				{
					return true;
				}
			}

			module = null;
			return false;
		}

		public static bool hasAncestorPart(this Part part, Part checkPart)
		{
			if (part == null)
			{
				Debug.LogError("Part.hasAncestorPart: 'part' argument should not be null.  Returning false.");
				return false;
			}
			if (checkPart == null)
			{
				Debug.LogError("Part.hasAncestorPart: 'checkPart' argument should not be null.  Returning false.");
				return false;
			}

			Part ancestorPart = part;

			do
			{
				if (ancestorPart == checkPart)
				{
					return true;
				}

				ancestorPart = ancestorPart.parent;
			}
			while (ancestorPart != null);

			return false;
		}

		public static bool isDecoupler(this Part part)
		{
			if (part.hasModuleByType<ModuleDecouple>() || part.hasModuleByType<ModuleAnchoredDecoupler>())
			{
				return true;
			}
			else
			{
				if (
					part.hasModuleByName("ModuleDockingNode") &&
					part.hasModuleByName("ModuleStagingToggle") &&
					part.isInStagingList()
				)
				{
					return true;
				}
			}

			return false;
		}

		public static bool isDockingNode(this Part part)
		{
			return hasModuleByType<ModuleDockingNode>(part);
		}

		public static bool isInStagingList(this Part part)
		{
			if (StageManager.Instance == null || StageManager.Instance.Stages == null)
			{
				return false;
			}

			StageGroup currentGroup;
			StageIcon currentIcon;
			for (int gIdx = 0; gIdx < StageManager.Instance.Stages.Count; gIdx++)
			{
				currentGroup = StageManager.Instance.Stages[gIdx];

				if (currentGroup == null || currentGroup.Icons == null)
				{
					continue;
				}

				for (int iIdx = 0; iIdx < currentGroup.Icons.Count; iIdx++)
				{
					currentIcon = currentGroup.Icons[iIdx];

					if (currentIcon.Part == part)
					{
						return true;
					}
				}
			}

			return false;
		}
	}
}