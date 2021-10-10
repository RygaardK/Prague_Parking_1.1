using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Prague_Buss_Parking
{
	class Program
	{

		static string[] ParkingList = new string[100];
		static string[] ParkingTicket = new string[200];

		static readonly string Logo =
							  "  ██████╗ ██████╗  █████╗  ██████╗ ██╗   ██╗███████╗    ██████╗  █████╗ ██████╗ ██╗  ██╗██╗███╗   ██╗ ██████╗   \n" +
							  "  ██╔══██╗██╔══██╗██╔══██╗██╔════╝ ██║   ██║██╔════╝    ██╔══██╗██╔══██╗██╔══██╗██║ ██╔╝██║████╗  ██║██╔════╝   \n" +
							  "  ██████╔╝██████╔╝███████║██║  ███╗██║   ██║█████╗      ██████╔╝███████║██████╔╝█████╔╝ ██║██╔██╗ ██║██║  ███╗  \n" +
							  "  ██╔═══╝ ██╔══██╗██╔══██║██║   ██║██║   ██║██╔══╝      ██╔═══╝ ██╔══██║██╔══██╗██╔═██╗ ██║██║╚██╗██║██║   ██║  \n" +
							  "  ██║     ██║  ██║██║  ██║╚██████╔╝╚██████╔╝███████╗    ██║     ██║  ██║██║  ██║██║  ██╗██║██║ ╚████║╚██████╔╝  \n" +
							  "  ╚═╝     ╚═╝  ╚═╝╚═╝  ╚═╝ ╚═════╝  ╚═════╝ ╚══════╝    ╚═╝     ╚═╝  ╚═╝╚═╝  ╚═╝╚═╝  ╚═╝╚═╝╚═╝  ╚═══╝ ╚═════╝   \n";

		static void Main(string[] args)
		{
			int menuInput;
			do
			{
				menuInput = MainMenu();
			} while (menuInput != 9);
		}
		private static int MainMenu()
		{
			Console.Clear();
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine(Logo);
			Console.WriteLine("\n ");
			Console.WriteLine("Choose an option:");
			Console.WriteLine("1) Add a Vehicle");
			Console.WriteLine("2) Move a Vehicle");
			Console.WriteLine("3) Checkout a Vehicle");
			Console.WriteLine("4) Show all Vehicles");
			Console.WriteLine("5) Search for my Vehicle");
			Console.WriteLine("9) Exit");
			Console.Write("\n Select an option: ");
			int menuInput;
			if (int.TryParse(Console.ReadLine(), out menuInput))
			{
				switch (menuInput)
				{
					case 1:
						AddVehiclesMenue();// Going to submenu -> AddVehiclesMenue
						break;
					case 2:
						ChangePlace();
						break;
					case 3:
						CheckOut();
						break;
					case 4:
						ShowVehicles();
						break;
					case 5:
						SearchVehicles();
						break;
					case 9:
						return 9;//exit
					default:
						Console.WriteLine("Enter a valid digit");//else
						break;
				}
			}
			return menuInput;
		}
		public static void AddVehiclesMenue()
		{
			{
				Console.Clear();
				Console.WriteLine(Logo);
				Console.WriteLine("\n ");
				Console.WriteLine("What do you wanna park:");
				Console.WriteLine("1) Park a Car");
				Console.WriteLine("2) Park a MC\n \n");
				Console.WriteLine("9) Exit");
				Console.Write("\n Select an option: ");
				try
				{
					int menuInput = int.Parse(Console.ReadLine());
					switch (menuInput)
					{
						case 1:
							AddCar();// Create for MC function
							break;
						case 2:
							AddMC();
							break;
						case 9:
							//AddVehicles();
							break;
						default:
							Console.WriteLine("Enter a valid digit!");
							break;
					}
					MainMenu();
				}
				catch (Exception ex)
				{
					Console.Clear();
					Console.WriteLine(ex.Message);
					Console.WriteLine("... Press a key to continue ...");
					Console.ReadKey();
					MainMenu();
				}
			}
		}
		public static void AddCar()
		{
			Console.WriteLine("Write your license plate number, 4-10 (A-Z & 0-9)");
			string userInput = Console.ReadLine().ToUpper();
			if (InputControl(userInput) && SearchIndexOf(userInput) < 0)
			{
				int index;
				if (IsNull() >= 0 && GetTicket(out index))
				{
					DateTime now = DateTime.Now;
					int mySpot = IsNull();
					Console.WriteLine($"We did park your vehicle on: {mySpot + 1}, with: {userInput}.");
					Console.WriteLine($"Time & Date: {now}");
					ParkingList[mySpot] = "CAR#" + userInput;
					ParkingTicket[index] = userInput + " " + now;
					Console.WriteLine("Press a key to continue!");
					Console.ReadKey();
				}
				else
				{
					Console.WriteLine("We did not have any parkingspots left!");
					Console.WriteLine("Press a key to continue!");
					Console.ReadKey();
				}
			}
			else
			{
				Console.WriteLine("Either you did not use the correct format (a-z/A-Z/0-9) or your vehicle already is parked here.");
				Console.WriteLine("Press a key to continue!");
				Console.ReadKey();
			}
			MainMenu();

		}// Done
		public static void AddMC()
		{
			Console.WriteLine("Write your license plate number, 4-10 (A-Z & 0-9)");
			string userInput = Console.ReadLine().ToUpper();
			int index;
			if (InputControl(userInput) && SearchIndexOf(userInput) < 0 && GetTicket(out index))
			{
				DateTime now = DateTime.Now;
				if (HasMC() >= 0)
				{
					int mySpot = HasMC();
					ParkingList[mySpot] = ParkingList[mySpot] + " / MC#" + userInput;
					ParkingTicket[index] = userInput + " " + now;
					Console.WriteLine($"We did find a spot on: {mySpot + 1}, and now your: {userInput}, is parked there.");
					Console.WriteLine("TimeStamp: " + now);
					Console.WriteLine("Press a key to continue!");
					Console.ReadKey();
				}
				else if (IsNull() >= 0)
				{
					int mySpot = IsNull();
					ParkingList[mySpot] = "MC#" + userInput;
					ParkingTicket[index] = userInput + " " + now;
					Console.WriteLine($"We did find a spot on: {mySpot + 1}, and now your: {userInput}, is parked there.");
					Console.WriteLine("TimeStamp: " + now);
					Console.WriteLine("Press a key to continue!");
					Console.ReadKey();
				}
			}
			else
			{
				Console.WriteLine("Either you did not use the correct format (a-z/A-Z/0-9) or your vehicle already is parked here.");
				Console.WriteLine("Press a key to continue!");
				Console.ReadKey();
			}
			MainMenu();
		}// Done
		public static void SearchVehicles()
		{
			Console.WriteLine("Write your license plate number, 4-10 (A-Z & 0-9)");
			string userInput = Console.ReadLine().ToUpper();
			if (InputControl(userInput) && SearchIndexOf(userInput) >= 0)
			{
				Console.WriteLine("Your car is placed on: " + (SearchIndexOf(userInput) + 1));
				Console.WriteLine("Press a key to continue!");
				Console.ReadKey();
			}
			else
			{
				Console.WriteLine("Either you did not use the correct format (a-z/A-Z/0-9) or your vehicle is NOT parked here!");
				Console.WriteLine("Press a key to continue!");
				Console.ReadKey();
			}
		} // Done
		static void ShowVehicles()
		{
			const int cols = 6;
			int n = 1;
			Console.Clear();
			for (int i = 0; i < ParkingList.Length; i++)
			{
				if (n >= cols && n % cols == 0)
				{
					Console.WriteLine();
					n = 1;
				}
				if (ParkingList[i] == null)
				{
					Console.ForegroundColor = ConsoleColor.Green;
					Console.Write((i + 1) + ": Empty \t");
					n++;
				}
				else if (!ParkingList[i].Contains("CAR#") && !ParkingList[i].Contains("/"))
				{
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.Write((i + 1) + ": " + ParkingList[i] + " \t");
					n++;
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.Write((i + 1) + ": " + ParkingList[i] + "\t");
					n++;
				}
			}
			Console.WriteLine("\n\n... Press key to continue ...");
			Console.ReadKey();
			Console.Clear();
		}//Done -- VG 1. Visualisering av vad som finns på parkeringsplatsen.
		static void ChangePlace()
		{
			Console.WriteLine("Write the registration number you wanna move.");
			string userInput = Console.ReadLine().ToUpper();
			int indexOF = SearchIndexOf(userInput);
			if (InputControl(userInput) && indexOF >= 0)
			{
				Console.WriteLine("That is a: " + VehicleType(userInput));
				Console.WriteLine("Write your new Parking spot:");
				int number;
				bool newParking = int.TryParse(Console.ReadLine(), out number);
				number--;
				while (IsFull(number) || number >= 99)
				{
					Console.Clear();
					ShowVehicles();
					Console.WriteLine("If you wanna exit write 100");
					Console.WriteLine((number + 1) + ": That spot was taken! Choose a new one: (Between 1 - 100");
					newParking = int.TryParse(Console.ReadLine(), out number);
				}
				string myType = VehicleType(userInput);
				if (myType == "MC#")
				{
					if (ParkingList[indexOF].Contains("/"))
					{
						string[] vehicle = ParkingList[indexOF].Split(" / ");
						if (ParkingList[number] != null && ParkingList[number].Contains("MC#"))
						{
							if (vehicle[0] == "MC#" + userInput)
							{
								ParkingList[indexOF] = vehicle[1];
								ParkingList[number] = ParkingList[number] + " / " + vehicle[0];
							}
							else if (vehicle[1] == "MC#" + userInput)
							{
								ParkingList[indexOF] = vehicle[0];
								ParkingList[number] = ParkingList[number] + " / " + vehicle[1];
							}
						}
						else
						{
							if (vehicle[0] == "MC#" + userInput)
							{
								ParkingList[indexOF] = vehicle[1];
								ParkingList[number] = vehicle[0];
							}
							else if (vehicle[1] == "MC#" + userInput)
							{
								ParkingList[indexOF] = vehicle[0];
								ParkingList[number] = vehicle[1];
							}
						}
					}
					else
					{
						ParkingList[number] = ParkingList[indexOF];
						ParkingList[indexOF] = null;
					}
				}
				else if (myType == "CAR#")
				{
					ParkingList[number] = ParkingList[indexOF];
					ParkingList[indexOF] = null;
				}
				Console.WriteLine($"We did move your: {myType}, to: {(number + 1)}");
				Console.WriteLine("Press a key to continue!");
				Console.ReadKey();
			}
			else
			{
				Console.WriteLine("Your Vehicle is NOT parked here!");
				Console.WriteLine("Press a key to continue!");
				Console.ReadKey();
			}
			MainMenu();
		}//Done
		static void CheckOut()
		{
			Console.WriteLine("Write your license plate number, 4-10 (A-Z & 0-9)");
			string userInput = Console.ReadLine().ToUpper();
			if (InputControl(userInput) && SearchIndexOf(userInput) >= 0)
			{
				DateTime now = DateTime.Now;
				TimeSpan interval = new TimeSpan();
				int myIndex = SearchIndexOf(userInput);
				string myTime;
				int myTicket = SearchTicket(userInput, out myTime);
				DateTime checkInDate = DateTime.Parse(myTime);
				if (VehicleType(userInput) == "MC#")
				{
					if (ParkingList[myIndex].Contains("/"))
					{
						string[] vehicle = ParkingList[myIndex].Split(" / ");
						interval = now - checkInDate;
						if (vehicle[0] == "MC#" + userInput)
						{ 
							ParkingList[myIndex] = vehicle[1];
						}
						else
						{
							ParkingList[myIndex] = vehicle[0];
						}
						ParkingTicket[myTicket] = null;


						Console.WriteLine("Press a key to continue!");
						Console.ReadKey();
					}
					else
					{
						ParkingTicket[myTicket] = null;
					}
				}
				else if (VehicleType(userInput) == "CAR#")
				{
					ParkingTicket[myTicket] = null;
					ParkingList[myIndex] = null;
				}
				Console.WriteLine($"Removing: {userInput}, on: {myIndex +1}");
				Console.WriteLine($"You have been parked for: {interval}");
				Console.WriteLine("Press a key to continue!");
				Console.ReadKey();
			}
			else
			{
				Console.WriteLine("Either you did not use the correct format (a-z/A-Z/0-9) or your vehicle is NOT parked here!");
				Console.WriteLine("Press a key to continue!");
				Console.ReadKey();
			}
			MainMenu();
		}//Done
		static string VehicleType(string userInput)
		{
			for (int i = 0; i < ParkingList.Length; i++)
			{
				if (ParkingList[i] != null && ParkingList[i].Contains("CAR#" + userInput))
				{
					return "CAR#";
				}
				else if (ParkingList[i] != null && ParkingList[i].Contains("MC#" + userInput))
				{
					return "MC#";
				}
			}
			return "null";
		}//Checks the Vehicle type e.g. "CAR" or "MC"
		static bool InputControl(string input)
		{
			string input_Reg = input;
			var charControl = new Regex(@"[0-9A-Z]\d{1}");
			bool lenghtControl = input_Reg.Length >= 4 && input_Reg.Length <= 10;
			if (charControl.IsMatch(input_Reg) && lenghtControl)
			{
				return true;
			}
			else
			{
				return false;
			}
		}//Controls if the INPUT is the correct format. -- VG 3. "Säkra upp användarinput"
		static int SearchIndexOf(string userInput)
		{
			for (int i = 0; i < ParkingList.Length; i++)
			{
				if (ParkingList[i] != null)
				{
					if (ParkingList[i].Contains("/"))
					{
						string[] vehicle = ParkingList[i].Split(" / ");
						if (vehicle[0] == "MC#" + userInput)
						{
							return i;
						}
						else if (vehicle[1] == "MC#" + userInput)
						{
							return i;
						}
						else
						{
							continue;
						}
					}
					else if (ParkingList[i] == "MC#" + userInput)
					{
						return i;
					}
					else if (ParkingList[i] == "CAR#" + userInput)
					{
						return i;
					}
				}
				else
				{
					continue;
				}
			}
			return -1;
		}//Strict control on a search of registration number, returns the index.
		static int HasMC()
		{
			for (int i = 0; i < ParkingList.Length; i++)
			{
				if (ParkingList[i] != null && !ParkingList[i].Contains("CAR#") && !ParkingList[i].Contains("/"))
				{
					return i;
				}
			}
			return -1;
		}//Just returns the index of the first SOLO MC in the list or returns -1.
		static int IsNull()
		{
			for (int i = 0; i < ParkingList.Length; i++)
			{
				if (ParkingList[i] == null)
				{
					return i;
				}
				else
				{
					continue; // samt MC // Separerea funktionerna EN SÖK MC(hasMC) o EN SÖK NULL(isNull)
				}
			}
			return -1;
		}//Just returns the index of the first Null in the list or returns -1.
		static bool IsFull(int userInput)
		{
			if (ParkingList[userInput] != null)
			{
				if (ParkingList[userInput].StartsWith("CAR#") || ParkingList[userInput].Contains("/"))
				{
					return true;
				}
			}
			return false;
		}//Checks if the SPECIFIC index is occupied by 2 MC or 1 Car
		static bool GetTicket(out int index)
		{
			for (int i = 0; i < ParkingTicket.Length; i++)
			{
				if (ParkingTicket[i] == null)
				{
					index = i;
					return true;
				}
			}
			index = -1;
			return false;
		}//Checks first of null and returns the value in ParkingTicket[] or false and -1.
		static int SearchTicket(string userInput, out string now)
		{
			for (int i = 0; i < ParkingTicket.Length; i++)
			{
				if (ParkingTicket[i] != null && ParkingTicket[i].StartsWith(userInput))
				{
					string dateTmp = ParkingTicket[i].Replace(userInput + " ", "");
					now = dateTmp;
					return i;
				}
			}
			now = "";
			return -1;
		}//Checks for a ticket by registration number.
		
	}
}