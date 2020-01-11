// ToadicusTools
//
// EventSniffer.cs
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

#if DEBUG

using KSP;
using System;
using System.Collections.Generic;
using System.Text;
using ToadicusTools.Extensions;
using ToadicusTools.Text;
using UnityEngine;

namespace ToadicusTools.DebugTools
{
	[KSPAddon(KSPAddon.Startup.EveryScene, false)]
	public class EventSniffer : MonoBehaviour
	{
		public void Awake()
		{
			GameEvents.onCrewOnEva.Add(this.onCrewOnEva);
			GameEvents.onCrewBoardVessel.Add(this.onCrewBoardVessel);

			GameEvents.onKerbalStatusChange.Add(this.onKerbalStatusChange);

			GameEvents.onPartPack.Add(this.onPartPack);
			GameEvents.onPartUnpack.Add(this.onPartUnpack);

			GameEvents.onVesselCreate.Add(this.onVesselCreate);
			GameEvents.onVesselDestroy.Add(this.onVesselDestroy);
			GameEvents.onVesselLoaded.Add(this.onVesselLoaded);
			GameEvents.onVesselGoOnRails.Add(this.onVesselGoOffRails);
			GameEvents.onVesselGoOffRails.Add(this.onVesselGoOffRails);

			GameEvents.onSameVesselDock.Add(this.onSameVesselDockUndock);
			GameEvents.onSameVesselUndock.Add(this.onSameVesselDockUndock);
			GameEvents.onPartUndock.Add(this.onPartUndock);
			GameEvents.onUndock.Add(this.onReportEvent);

			GameEvents.onPartCouple.Add(this.onPartCouple);
			GameEvents.onPartJointBreak.Add(this.onPartJointBreak);

			GameEvents.onEditorPartEvent.Add(this.onEditorPartEvent);
			GameEvents.onEditorShipModified.Add(this.onEditorShipModified);

			ToadicusTools.Extensions.ComponentExtensions.LogDebug(this, "Awake.");
		}

		public void OnDestroy()
		{
			GameEvents.onCrewOnEva.Remove(this.onCrewOnEva);
			GameEvents.onCrewBoardVessel.Remove(this.onCrewBoardVessel);

			GameEvents.onKerbalStatusChange.Remove(this.onKerbalStatusChange);

			GameEvents.onPartPack.Remove(this.onPartPack);
			GameEvents.onPartUnpack.Remove(this.onPartUnpack);

			GameEvents.onVesselCreate.Remove(this.onVesselCreate);
			GameEvents.onVesselDestroy.Remove(this.onVesselDestroy);
			GameEvents.onVesselLoaded.Remove(this.onVesselLoaded);
			GameEvents.onVesselGoOnRails.Remove(this.onVesselGoOffRails);
			GameEvents.onVesselGoOffRails.Remove(this.onVesselGoOffRails);

			GameEvents.onSameVesselDock.Remove(this.onSameVesselDockUndock);
			GameEvents.onSameVesselUndock.Remove(this.onSameVesselDockUndock);
			GameEvents.onPartUndock.Remove(this.onPartUndock);
			GameEvents.onUndock.Remove(this.onReportEvent);

			GameEvents.onPartCouple.Remove(this.onPartCouple);
			GameEvents.onPartJointBreak.Remove(this.onPartJointBreak);

			GameEvents.onEditorPartEvent.Remove(this.onEditorPartEvent);

			GameEvents.onEditorShipModified.Remove(this.onEditorShipModified);

			ToadicusTools.Extensions.ComponentExtensions.LogDebug(this, "Destroyed.");
		}

		public void onEditorShipModified(ShipConstruct construct)
		{
			using (PooledStringBuilder sb = this.getStringBuilder())
			{
				sb.AppendFormat("construct: {0}", construct.shipName);

				Logging.PostLogMessage(sb.ToString());
			}
		}

		public void onEditorPartEvent(ConstructionEventType type, Part part)
		{
			using (PooledStringBuilder sb = this.getStringBuilder())
			{
				sb.AppendFormat("ConstructionEventType={0}, Part={1}",
					Enum.GetName(typeof(ConstructionEventType), type), part);

				Logging.PostLogMessage(sb.ToString());
			}
		}

		public void onCrewOnEva(GameEvents.FromToAction<Part, Part> data)
		{
			using (PooledStringBuilder sb = this.getStringBuilder())
			{
				this.FromPartToPartHelper(
					sb,
					data
				);
			}
		}

		public void onCrewBoardVessel(GameEvents.FromToAction<Part, Part> data)
		{
			using (PooledStringBuilder sb = this.getStringBuilder())
			{
				this.FromPartToPartHelper(
					sb,
					data
				);
			}
		}

		public void onKerbalStatusChange(ProtoCrewMember kerbal, ProtoCrewMember.RosterStatus fromStatus, ProtoCrewMember.RosterStatus toStatus)
		{
			using (PooledStringBuilder sb = this.getStringBuilder())
			{
				string item;

				if (kerbal != null)
				{
					item = kerbal.name;
				}
				else
				{
					item = "null";
				}

				sb.AppendFormat("\n\tKerbal: {0}", item);

				sb.AppendFormat("\n\tfromStatus: {0}", Enum.GetName(typeof(ProtoCrewMember.RosterStatus), fromStatus));

				sb.AppendFormat("\n\ttoStatus: {0}", Enum.GetName(typeof(ProtoCrewMember.RosterStatus), toStatus));

				Logging.PostLogMessage(sb.ToString());
			}
		}

		public void onPartPack(Part data)
		{
			using (PooledStringBuilder sb = this.getStringBuilder())
			{
				this.PartEventHelper(sb, data);
			}
		}

		public void onPartUnpack(Part data)
		{
			using (PooledStringBuilder sb = this.getStringBuilder())
			{
				this.PartEventHelper(sb, data);
			}
		}

		public void onSameVesselDockUndock(GameEvents.FromToAction<ModuleDockingNode, ModuleDockingNode> data)
		{
			using (PooledStringBuilder sb = this.getStringBuilder())
			{
				this.FromModuleToModuleHelper(
					sb,
					new GameEvents.FromToAction<PartModule, PartModule>(data.from, data.to)
				);
			}
		}
        
        public void onPartJointBreak(PartJoint joint, float f)
		{
			using (PooledStringBuilder sb = this.getStringBuilder())
			{
				this.PartJointHelper(sb, joint);
			}
		}

		public void onPartUndock(Part part)
		{
			using (PooledStringBuilder sb = this.getStringBuilder())
			{
				this.PartEventHelper(sb, part);
			}
		}

		public void onReportEvent(EventReport data)
		{
			using (PooledStringBuilder sb = this.getStringBuilder())
			{
				this.EventReportHelper(sb, data);
			}
		}

		public void onPartCouple(GameEvents.FromToAction<Part, Part> data)
		{
			using (PooledStringBuilder sb = this.getStringBuilder())
			{
				this.FromPartToPartHelper(sb, data);
			}
		}

		public void onVesselCreate(Vessel data)
		{
			this.VesselEventHelper(this.getStringBuilder(), data);
		}

		public void onVesselDestroy(Vessel data)
		{
			using (PooledStringBuilder sb = this.getStringBuilder())
			{
				this.VesselEventHelper(sb, data);
			}
		}

		public void onVesselGoOffRails(Vessel data)
		{
			using (PooledStringBuilder sb = this.getStringBuilder())
			{
				this.VesselEventHelper(sb, data);
			}
		}

		public void onVesselGoOnRails(Vessel data)
		{
			using (PooledStringBuilder sb = this.getStringBuilder())
			{
				this.VesselEventHelper(sb, data);
			}
		}


		public void onVesselLoaded(Vessel data)
		{
			using (PooledStringBuilder sb = this.getStringBuilder())
			{
				this.VesselEventHelper(sb, data);
			}
		}


		internal void VesselEventHelper(PooledStringBuilder sb, Vessel data)
		{
			this.appendVessel(sb, data);

			Logging.PostLogMessage(sb.ToString());
		}

		internal void EventReportHelper(PooledStringBuilder sb, EventReport data)
		{
			sb.Append("\n\tOrigin Part:");
			this.appendPartAncestry(sb, data.origin);

			sb.AppendFormat(
				"\n\tother: '{0}'" +
				"\n\tmsg: '{1}'" +
				"\n\tsender: '{2}'",
				data.other,
				data.msg,
				data.sender
			);

			Logging.PostLogMessage(sb.ToString());
		}

		internal void PartEventHelper(PooledStringBuilder sb, Part part)
		{
			this.appendPartAncestry(sb, part);

			Logging.PostLogMessage(sb.ToString());
		}

		internal void FromPartToPartHelper(PooledStringBuilder sb, GameEvents.FromToAction<Part, Part> data)
		{
			sb.Append("\n\tFrom:");

			this.appendPartAncestry(sb, data.from);

			sb.Append("\n\tTo:");

			this.appendPartAncestry(sb, data.to);

			Logging.PostLogMessage(sb.ToString());
		}

		internal void FromModuleToModuleHelper(PooledStringBuilder sb, GameEvents.FromToAction<PartModule, PartModule> data)
		{
			sb.Append("\n\tFrom:");

			this.appendModuleAncestry(sb, data.from);

			sb.Append("\n\tTo:");

			this.appendModuleAncestry(sb, data.to);

			Logging.PostLogMessage(sb.ToString());
		}

		internal void HostedFromPartToPartHelper(PooledStringBuilder sb, GameEvents.HostedFromToAction<Part, Part> data)
		{
			sb.AppendLine("Caught onCrewOnEva");

			string item;

			if (data.host != null)
			{
				item = data.host.partName;
			}
			else
			{
				item = "NULL";
			}

			sb.AppendFormat("Host: {0}\n", item);


			if (data.from != null)
			{
				item = data.from.partName;
			}
			else
			{
				item = "NULL";
			}

			sb.AppendFormat("From: {0}\n", item);

			if (data.to != null)
			{
				item = data.to.partName;
			}
			else
			{
				item = "NULL";
			}

			sb.AppendFormat("To: {0}\n", item);
		}

		internal void PartJointHelper(PooledStringBuilder sb, PartJoint joint)
		{
			sb.Append("PartJoint: ");
			if (joint != null)
			{
				sb.Append(joint);
				this.appendPartAncestry(sb, joint.Host);
			}
			else
			{
				sb.Append("null");
			}

			Logging.PostLogMessage(sb.ToString());
		}

		internal PooledStringBuilder appendModuleAncestry(PooledStringBuilder sb, PartModule module, uint tabs = 1)
		{
			sb.Append('\n');
			for (ushort i=0; i < tabs; i++)
			{
				sb.Append('\t');
			}
			sb.Append("Module: ");

			if (module != null)
			{
				sb.Append(module.moduleName);
				this.appendPartAncestry(sb, module.part, tabs + 1u);
			}
			else
			{
				sb.Append("null");
			}

			return sb;
		}

		internal PooledStringBuilder appendPartAncestry(PooledStringBuilder sb, Part part, uint tabs = 1)
		{
			sb.Append('\n');
			for (ushort i=0; i < tabs; i++)
			{
				sb.Append('\t');
			}
			sb.Append("Part: ");

			if (part != null)
			{
				sb.AppendFormat("'{0}' ({1})", part.partInfo.title, part.partName);
				this.appendVessel(sb, part.vessel, tabs + 1u);
			}
			else
			{
				sb.Append("null");
			}

			return sb;
		}

		internal PooledStringBuilder appendVessel(PooledStringBuilder sb, Vessel vessel, uint tabs = 1)
		{
			sb.Append('\n');
			for (ushort i=0; i < tabs; i++)
			{
				sb.Append('\t');
			}
			sb.Append("Vessel: ");

			if (vessel != null)
			{
				sb.AppendFormat("{0} '{1}' ({2})",
					Enum.GetName(typeof(VesselType), vessel.vesselType),
					vessel.vesselName,
					vessel.id
				);
			}
			else
			{
				sb.Append("null");
			}

			return sb;
		}

		internal PooledStringBuilder getStringBuilder()
		{
			PooledStringBuilder sb = PooledStringBuilder.Get();
			sb.AppendFormat("{0}: called {1} ",
				this.GetType().Name,
				new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name
			);
			return sb;
		}
	}

	[AttributeUsage(AttributeTargets.Method)]
	internal class EventSubscriptionAttribute : Attribute
	{
		public System.Reflection.MethodInfo addMethod;
		public System.Reflection.MethodInfo removeMethod;

		public EventSubscriptionAttribute()
		{
			this.addMethod = null;
			this.removeMethod = null;
		}
	}
}

#endif
