// ToadicusTools
//
// DebugPartModule.cs
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
using System.Reflection;
using System.Text;
using ToadicusTools.Text;
using UnityEngine;

namespace ToadicusTools.DebugTools
{
	public class DebugPartModule : PartModule
	{
		public override void OnAwake()
		{
			BaseField field;
			for (int idx = 0; idx < this.Fields.Count; idx++)
			{
				field = this.Fields[idx];
				field.guiActive = field.guiActiveEditor = true;
			}

			this.Events["DumpModule"].guiName = string.Format("Dump {0}", this.GetType().Name);
			// TODO: Fix this; part or partInfo isn't defined during OnAwake at load screen.
			// this.Events["DumpPart"].guiName = string.Format("Dump {0}", this.part.partInfo.name);

			base.OnAwake();
		}

		public override void OnLoad(ConfigNode node)
		{
			base.OnLoad(node);
		}

		public override void OnSave(ConfigNode node)
		{
			base.OnSave(node);
		}

		public override void OnActive()
		{
			base.OnActive();
		}

		public override void OnInactive()
		{
			base.OnInactive();
		}

		public override void OnInitialize()
		{
			base.OnInitialize();
		}

		public override void OnFixedUpdate()
		{
			base.OnFixedUpdate();
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
		}

		public override string GetInfo()
		{
			return base.GetInfo();
		}

		[KSPEvent(guiActive = true, guiActiveEditor = true)]
		public void DumpPart()
		{
			if (this.part != null)
			{
				DumpClassObject(this.part);
			}

			if (this.part.Modules != null)
			{
				PartModule module;
				for (int idx = 0; idx < this.part.Modules.Count; idx++)
				{
					module = this.part.Modules[idx];

					DumpClassObject(module);
				}
			}
		}

		[KSPEvent(guiActive = true, guiActiveEditor = true)]
		public void DumpModule()
		{
			DumpClassObject(this);
		}

		public static void DumpClassObject(object obj)
		{
			using (PooledStringBuilder sb = PooledStringBuilder.Get())
			{
				sb.Append(obj.GetType().Name);
				sb.Append(":\n");

				FieldInfo[] fieldInfos = obj.GetType().GetFields(
					                        System.Reflection.BindingFlags.Public |
					                        System.Reflection.BindingFlags.NonPublic |
					                        System.Reflection.BindingFlags.Instance |
					                        System.Reflection.BindingFlags.FlattenHierarchy
				                        );

				FieldInfo fieldInfo;
				for (int idx = 0; idx < fieldInfos.Length; idx++)
				{
					fieldInfo = fieldInfos[idx];
					try
					{
						sb.AppendFormat("{0}: {1}\n", fieldInfo.Name, fieldInfo.GetValue(obj));
					}
					catch
					{
						// Do nothing
					}
				}

				PropertyInfo[] propInfos = obj.GetType().GetProperties(
					                          System.Reflection.BindingFlags.Public |
					                          System.Reflection.BindingFlags.NonPublic |
					                          System.Reflection.BindingFlags.Instance |
					                          System.Reflection.BindingFlags.FlattenHierarchy
				                          );
				PropertyInfo propInfo;
				for (int idx = 0; idx < propInfos.Length; idx++)
				{
					propInfo = propInfos[idx];
					try
					{
						sb.AppendFormat("{0}: {1}\n", propInfo.Name, propInfo.GetValue(obj, null));
					}
					catch
					{
						// Do nothing
					}
				}

				MethodInfo[] methodInfos = obj.GetType().GetMethods(
					                          System.Reflection.BindingFlags.Public |
					                          System.Reflection.BindingFlags.NonPublic |
					                          System.Reflection.BindingFlags.Instance |
					                          System.Reflection.BindingFlags.FlattenHierarchy
				                          );

				MethodInfo methodInfo;
				for (int idx = 0; idx < methodInfos.Length; idx++)
				{
					methodInfo = methodInfos[idx];
					try
					{
						if (methodInfo.ReturnType != typeof(void) && methodInfo.GetParameters().Length == 0)
						{
							sb.AppendFormat("{0} returns: '''{1}'''\n", methodInfo.Name, methodInfo.Invoke(obj, null));
						}
					}
					catch
					{
						// Do nothing
					}
				}

				Logging.PostLogMessage(sb.ToString());
			}
		}
	}
}
