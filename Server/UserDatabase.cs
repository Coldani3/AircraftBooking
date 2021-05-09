using System;
using System.Collections.Generic;
using System.IO;
using AircraftBooking.Shared;

namespace AircraftBooking.Server
{
	public class UserDatabase
	{
		public string FileLocation;
		public List<User> Users = new List<User>();

		public UserDatabase(string fileLocation)
		{
			this.FileLocation = fileLocation;
			this.LoadFromFile();
		}

		public void LoadFromFile()
		{
			if (File.Exists(this.FileLocation))
			{
				string[] lines = File.ReadAllLines(this.FileLocation);

				foreach (string line in lines)
				{
					string[] split = line.Split(';');

					this.AddUser(new User(split[0], split[1]));
				}
			}
			else
			{
				File.Create(this.FileLocation);
			}
		}

		public void WriteToFile()
		{
			if (!File.Exists(this.FileLocation))
			{
				File.Create(this.FileLocation);
			}

			string users = "";

			foreach (User user in this.Users)
			{
				users += $"{user.Username};{user.GetPassword()}\n";
			}

			File.WriteAllText(this.FileLocation, users);
		}

		public void AddUser(User user)
		{
			this.Users.Add(user);
		}

		public void DeleteUser(User user)
		{
			this.Users.Remove(user);
		}

		public void DeleteUser(string username)
		{
			foreach (User user in this.Users)
			{
				if (user.Username == username)
				{
					this.DeleteUser(user);
					break;
				}
			}
		}

		public bool TryLogin(User user)
		{
			foreach (User currUser in this.Users)
			{
				if (user.Username == currUser.Username && currUser.CheckPassword(user.GetPassword()))
				{
					return true;
				}
			}

			return false;
		}
	}
}