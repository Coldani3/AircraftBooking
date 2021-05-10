using ConsoleMenuLibrary;
using AircraftBooking.Shared;
using AircraftBooking.Shared.Packets;
using System.Collections.Generic;
using System;

namespace AircraftBooking.Client
{
	public class SelectPlaneMenu : SelectItemMenu
	{
		public SelectPlaneMenu(PlaneInfo[] planes) : base(NamesFromPlanes(), GenActionsForLength(planes.Length, planes))
		{
			
		}

		public SelectPlaneMenu() : base(NamesFromPlanes(), GenActionsForLength(SessionData.Planes.Length, SessionData.Planes))
		{
			
		}

		public static string[] NamesFromPlanes()
		{
			List<string> names = new List<string>();

			foreach (PlaneInfo plane in SessionData.Planes)
			{
				names.Add(plane.PlaneName);
			}

			names.Add("Logout");

			return names.ToArray();
		}

		public static Action[] GenActionsForLength(int planeCount, PlaneInfo[] planes)
		{
			List<Action> actions = new List<Action>();

			for (int i = 0; i < planeCount; i++)
			{
				PlaneInfo plane = planes[i];
				actions.Add(() => {
					SelectPlane(plane);
				});
			}

			actions.Add(() => Logout());

			return actions.ToArray();
		}

		public static void SelectPlane(PlaneInfo plane)
		{
			Program.MenuManager.ChangeMenu(new SelectSeatOnPlaneMenu(plane.PlaneID));
		}

		public static void Logout()
		{
			Program.Running = false;
			Client.GetClient().SendPacket(new LogOutPacket().Construct(SessionData.CurrentUser), Client.GetSocket());
		}

	}
}