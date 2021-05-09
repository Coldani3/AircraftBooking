using ConsoleMenuLibrary;
using AircraftBooking.Shared;
using AircraftBooking.Shared.Packets;
using System.Collections.Generic;
using System;

namespace AircraftBooking.Client
{
	public class SelectPlaneMenu : SelectItemMenu
	{
		public SelectPlaneMenu(PlaneInfo[] planes) : base(NamesFromPlanes(planes), GenActionsForLength(planes.Length))
		{
			Program.MenuManager.SetPersistentMenuData("planes", planes);
		}

		public static string[] NamesFromPlanes(PlaneInfo[] planes)
		{
			List<string> names = new List<string>();

			foreach (PlaneInfo plane in planes)
			{
				names.Add(plane.PlaneName);
			}

			return names.ToArray();
		}

		public static Action[] GenActionsForLength(int planeCount)
		{
			List<Action> actions = new List<Action>();

			for (int i = 0; i < planeCount; i++)
			{
				actions.Add(() => SelectPlane(i));
			}

			return actions.ToArray();
		}

		public static void SelectPlane(int index)
		{
			Program.MenuManager.ChangeMenu(new SelectSeatOnPlaneMenu(index));
			//Client.GetClient().SendPacket(new BookPlaneSeatPacket().Construct(), Client.GetSocket());
		}

	}
}