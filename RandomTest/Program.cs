namespace RandomTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<ParkingSpot> parkingSpots = ParkingSpot.CreateParkingList();
            List<Vehicle> vehicles = Helper.vehiclesList; //kanske använder den sen!
            Random rnd = new Random();

            while (true)
            {
                Console.WriteLine("\nParkNr" + "\t\t" + "Status" + "\t\t" + "RegNummer" + "\tFärg" + "\tFordontyp" + "\tUnik-Egenskap");
                Console.WriteLine("======================================================================================");
                for (int i = 0; i < parkingSpots.Count; i++)
                {
                    if (parkingSpots[i].Capacity < 1)
                    {
                        foreach (var vehicle in parkingSpots[i].Vehicles)
                        {
                            if(vehicle is Car car)
                            {
                                Console.WriteLine("[P" + parkingSpots[i].Number + "]" + "\t\tUpptagen" + "\t" + vehicle.RegNummer
                               + "\t\t" + vehicle.Color + "\t" + vehicle.GetType().Name + "\t\t" + (car.IsElectric == true ? "Elbil" : "Diesel/bensin)"));
                            }
                            if (vehicle is Motorcycle motorcycle)
                            {
                                Console.WriteLine("[P" + parkingSpots[i].Number + "]" + "\t\tUpptagen" + "\t" + vehicle.RegNummer
                               + "\t\t" + vehicle.Color + "\t" + vehicle.GetType().Name + "\tBrand:" + motorcycle.Brand);
                            }
                            if (vehicle is Bus bus)
                            {
                                Console.WriteLine("[P" + parkingSpots[i].Number + "]" + "\t\tUpptagen" + "\t" + vehicle.RegNummer
                               + "\t\t" + vehicle.Color + "\t" + vehicle.GetType().Name + "\t\tPassengers:" + bus.Passenger);
                            }

                        }
                    }
                    else
                    {
                        Console.WriteLine("[P" + parkingSpots[i].Number + "]" + "\t\tLedig");
                    }
                    
                }
                Console.WriteLine("==================================================================");
                Console.WriteLine("\n\n\n\tMeny");
                Console.WriteLine("~~~~~~~~~~~~~~~~~~");
                Console.WriteLine("[S]kapa och parkera fordon");
                Console.WriteLine("[C]hecka ut fordon");
                Console.WriteLine("[A]vsluta programmet");
                char input = Console.ReadKey().KeyChar;
                

                switch (input)
                {
                    case 's':
                        int randomNumber = rnd.Next(1, 4);
                        Vehicle newVehicle = null;
                        if (randomNumber == 1)
                        {
                            newVehicle = Helper.CreateCar();
                           
                        }
                        else if(randomNumber == 2)
                        {
                            newVehicle = Helper.CreateMotorcycle();
                            
                        }
                        else if (randomNumber == 3)
                        {
                            newVehicle = Helper.CreateBus();

                        }
                        if (newVehicle != null)
                        {
                            Helper.ParkCar(parkingSpots, newVehicle);
                        }
                        Console.Clear();
                        break;
                    case 'c':
                        Console.Write("\nAnge fordonets regnummer: ");
                        string regnummer = Console.ReadLine().ToUpper();
                        Helper.AvslutaParkering(parkingSpots, regnummer);
                        Thread.Sleep(5000);
                        Console.Clear();
                        break;
                    case 'a':
                        Environment.Exit(0); break;
                    default:
                        Console.WriteLine("\nOgiltigt val, Välj ett val från menyn!");
                        Thread.Sleep(1000);
                        Console.Clear();
                        break;
                }
            }
        }
    }
}