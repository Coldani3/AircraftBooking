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

			try
			{
				StartRendering();
			}
			finally
			{
				Client.GetClient().Shutdown();
			}
        }

		static void StartRendering()
		{
			//initialise menu stuff.
			Console.SetWindowSize(70, 40);
			MainMenu menu = new MainMenu();
			Renderer renderer = new Renderer();
			MenuManager = new MenuManager(renderer, menu);
			MenuManager manager = MenuManager;

			renderer.Render(manager);

			while (Running)
			{
				ConsoleKeyInfo input = Console.ReadKey(true);
				if (input.Key == ConsoleKey.Escape) 
				{
					Running = false;
				}

				manager.ActiveMenu.OnInput(input, manager);
				
				renderer.Render(manager);
				System.Threading.Thread.Sleep(100);
			}
		}
    }
}
