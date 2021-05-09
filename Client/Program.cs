using System;
using ConsoleMenuLibrary;

namespace AircraftBooking.Client
{
    class Program
    {
		//State that keeps track of the client's running status
		public static bool Running = true;
		public static MenuManager MenuManager;

        static void Main(string[] args)
        {
            Client.GetClient().Start();
			StartRendering();

        }

		static void StartRendering()
		{
			Console.SetWindowSize(70, 40);
			MainMenu menu = new MainMenu();
			Renderer renderer = new Renderer();
			MenuManager = new MenuManager(renderer, menu);
			MenuManager manager = MenuManager;
			bool ranOnce = false;

			while (Running)
			{
				ConsoleKeyInfo input = Console.ReadKey(true);
				if (input.Key == ConsoleKey.Escape) 
				{
					Running = false;
				}

				manager.ActiveMenu.OnInput(input, manager);

				// if (ranOnce) 
				// {
				// 	manager.ActiveMenu.OnInput(input, manager);
				// }
				// else
				// {
				// 	ranOnce = true;
				// }
				
				renderer.Render(manager);
				System.Threading.Thread.Sleep(100);
			}
		}
    }
}
