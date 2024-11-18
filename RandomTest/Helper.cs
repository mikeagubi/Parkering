using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices.Marshalling;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RandomTest
{
    internal class Helper
    {
        public static List<Vehicle> vehiclesList = new List<Vehicle>();
        public static Car CreateCar()
        {
            bool isElectric;
            Console.Write("\n\nAnge Bilens Färg: ");
            string color = Console.ReadLine();
            Console.Write("Vill du skapa en Elbil? Ange [Ja/Nej]: ");
            string eldriven = Console.ReadLine().ToLower();
            if (eldriven == "ja")
            {
                isElectric = true;
            }
            else
            {
                isElectric = false;
            }
            Car car = new Car(isElectric, color);
            Console.WriteLine("----------------------------------------------------------------");
            Console.WriteLine("\n\n");
            Console.WriteLine("Bilen är nu skapad.");
            Console.WriteLine($"Regnummer:  {car.RegNummer}");
            Console.WriteLine($"Färg:  {car.Color}");
            Console.WriteLine($"Drivmedel: {(car.IsElectric == true ? "El-driven" : "Diesel/Bensin")}");
            vehiclesList.Add(car);
            return car;
        }

        public static Motorcycle CreateMotorcycle()
        {
            Console.Write("\n\nAnge Motorcykelns Färg: ");
            string color = Console.ReadLine();
            Console.Write("\nAnge Motorcykelns Tillverkare: ");
            string brand = Console.ReadLine();

            Motorcycle motorcycle = new Motorcycle(brand, color);
            Console.WriteLine("----------------------------------------------------------------");
            Console.WriteLine("\n\n");
            Console.WriteLine("Motorcykeln är nu skapad.");
            Console.WriteLine($"Regnummer:  {motorcycle.RegNummer}");
            Console.WriteLine($"Färg:  {motorcycle.Color}");
            Console.WriteLine($"Tillverkare: {motorcycle.Brand}");
            vehiclesList.Add(motorcycle);
            return motorcycle;
        }

        public static Bus CreateBus()
        {
            Console.Write("\n\nAnge Bussens Färg: ");
            string color = Console.ReadLine();
            Console.Write("Ange Antal Passagerare: ");
            int passengers;
            while(!int.TryParse(Console.ReadLine(), out passengers))
            {
                Console.WriteLine("Ogiltig inmatning, ange siffror!");
            }
            Bus bus = new Bus(passengers, color);
            Console.WriteLine("----------------------------------------------------------------");
            Console.WriteLine("\n\n");
            Console.WriteLine("Bussen är nu skapad.");
            Console.WriteLine($"Regnummer:  {bus.RegNummer}");
            Console.WriteLine($"Färg:  {bus.Color}");
            Console.WriteLine($"Passagerare: {bus.Passenger}");
            vehiclesList.Add (bus);
            return bus;
        }

        public static void ParkCar(List<ParkingSpot> parkering, Vehicle newVehicle)
        {
            bool parked = false;
            for (int i = 0; i < parkering.Count; i++)
            {
                if (newVehicle is Car && parkering[i].IsFree && parkering[i].Capacity == newVehicle.Size())
                {
                    parkering[i].Vehicles.Add(newVehicle);
                    parkering[i].Capacity -= newVehicle.Size();
                    parkering[i].ParkedDate = DateTime.Now;
                    parked = true;
                    if (parkering[i].Capacity == 0) parkering[i].IsFree = false;
                    
                    break;
                }
                else if (newVehicle is Motorcycle && parkering[i].Capacity >= newVehicle.Size() &&
                 (parkering[i].IsFree || parkering[i].Vehicles.TrueForAll(v => v is Motorcycle)))
                {
                    parkering[i].Vehicles.Add(newVehicle);
                    parkering[i].Capacity -= newVehicle.Size();
                    parkering[i].ParkedDate = DateTime.Now;
                    parked = true;
                    if (parkering[i].Capacity == 0) parkering[i].IsFree = false;
                    
                    break;
                }
                else if (newVehicle is Bus && i < parkering.Count -1 && (parkering[i].IsFree && parkering[i + 1].IsFree) && (parkering[i].Capacity == 1 && parkering[i+1].Capacity == 1))
                {
                    parkering[i].Vehicles.Add(newVehicle);
                    parkering[i].ParkedDate = DateTime.Now;
                    parkering[i].Capacity -= 1;
                    parkering[i].IsFree = false;
                    parkering[i+1].Vehicles.Add(newVehicle);
                    parkering[i+1].ParkedDate = DateTime.Now;
                    parkering[i+1].Capacity -= 1;
                    parkering[i+1].IsFree = false;
                    parked = true;
                    
                    break;
                }
             }
            if (parked == false)
            {
                Console.WriteLine("Ingen ledig plats kvar för fordonet.");
            }
        }

        public static void AvslutaParkering(List<ParkingSpot> parkering, string regNummer)
        {
            TimeSpan parkTime;
            double parkToPay;
            bool regnummerMatchar = false;

            for (int i = 0; i < parkering.Count; i++)
            {
                for (int j = 0; j < parkering[i].Vehicles.Count; j++)
                {
                    if (regNummer == parkering[i].Vehicles[j].RegNummer)
                    {
                        regnummerMatchar = true;
                        parkTime = DateTime.Now - parkering[i].ParkedDate;
                        double totalMitues = parkTime.TotalMinutes;

                        if (parkering[i].Vehicles[j] is Car)
                        {
                            parkering[i].Vehicles.Remove(parkering[i].Vehicles[j]);
                            parkering[i].Capacity += 1;
                            parkToPay = totalMitues * 1.5;
                            Console.WriteLine($"Bil med regnummer {regNummer} lämnade parkeringen.");
                            Console.WriteLine($"Total parkeringstid var:{totalMitues} Att betala: {parkToPay}kr");
                        }
                        else if (parkering[i].Vehicles[j] is Motorcycle)
                        {
                            parkering[i].Vehicles.Remove(parkering[i].Vehicles[j]);
                            parkering[i].Capacity += 0.5;
                            parkToPay = totalMitues * 1.5 / 2;
                            Console.WriteLine($"Moptorcykel med regnummer {regNummer} lämnade parkeringen");
                            Console.WriteLine($"Total parkeringstid var:{totalMitues} Att betala: {parkToPay}kr");
                        }
                        else if (parkering[i].Vehicles[j] is Bus)
                        {
                            parkering[i].Vehicles.Remove(parkering[i].Vehicles[j]);
                            parkering[i].Capacity += 1;

                            if (i + 1 < parkering.Count && parkering[i + 1].Vehicles.Count > 0)
                            {
                                for (int k = 0; k < parkering[i + 1].Vehicles.Count; k++)
                                {
                                    if (parkering[i + 1].Vehicles[k].RegNummer == regNummer)
                                    {
                                        parkering[i + 1].Vehicles.Remove(parkering[i + 1].Vehicles[k]);
                                        parkering[i + 1].Capacity += 1;
                                        parkToPay = totalMitues * 1.5 * 2;
                                        Console.WriteLine($"Buss med regnummer {regNummer} lämnade parkeringarna {parkering[i].Number} och {parkering[i + 1].Number}.");
                                        Console.WriteLine($"Total parkeringstid var:{totalMitues} Att betala: {parkToPay}kr");
                                        break;
                                    }
                                }
                            }
                        }

                        if (parkering[i].Vehicles is Motorcycle && parkering[i].Capacity > 0)
                        {
                            parkering[i].IsFree = true;
                        }
                        if (parkering[i].Capacity == 1 && parkering[i].Vehicles.Count == 0)
                        {
                            parkering[i].IsFree = true;
                        }
                        if (i + 1 < parkering.Count && parkering[i + 1].Capacity == 1 && parkering[i + 1].Vehicles.Count == 0)
                        {
                            parkering[i + 1].IsFree = true;
                        }
                    }
                }
            }

            if (regnummerMatchar == false)
            {
                Console.WriteLine("Regnumret matchar inget fordon!");
            }

        }

        //public static void AvslutaParkering(List<ParkingSpot> parkering, string regNummer)
        //{
        //    TimeSpan parkTime;
        //    double totalMinutes;
        //    bool regnummerMatchar = false;

        //    for (int i = 0; i < parkering.Count; i++)
        //    {
        //        for (int j = 0; j < parkering[i].Vehicles.Count; j++)
        //        {
        //            if (regNummer == parkering[i].Vehicles[j].RegNummer)
        //            {
        //                regnummerMatchar = true;
        //                parkTime = DateTime.Now - parkering[i].ParkedDate;
        //                totalMinutes = parkTime.TotalMinutes;

        //                if (parkering[i].Vehicles[j] is Car)
        //                {
        //                    HanteraBil(parkering, i, j, regNummer, totalMinutes);
        //                }
        //                else if (parkering[i].Vehicles[j] is Motorcycle)
        //                {
        //                    HanteraMotorcykel(parkering, i, j, regNummer, totalMinutes);
        //                }
        //                else if (parkering[i].Vehicles[j] is Bus)
        //                {
        //                    HanteraBuss(parkering, i, j, regNummer, totalMinutes);
        //                }

        //                KontrolleraOchUppdateraLedigPlats(parkering, i);
        //                return; // Avslutar eftersom fordonet har hittats och hanterats
        //            }
        //        }
        //    }

        //    if (!regnummerMatchar)
        //    {
        //        Console.WriteLine("Regnumret matchar inget fordon!");
        //    }
        //}

        //// Metod för att hantera avslut av bil
        //private static void HanteraBil(List<ParkingSpot> parkering, int i, int j, string regNummer, double totalMinutes)
        //{
        //    parkering[i].Vehicles.RemoveAt(j);
        //    parkering[i].Capacity += 1;

        //    double cost = totalMinutes * 1.5;
        //    Console.WriteLine($"Bil med regnummer {regNummer} lämnade parkeringen.");
        //    Console.WriteLine($"Total parkeringstid var: {totalMinutes} minuter. Att betala: {cost} kr.");
        //}

        //// Metod för att hantera avslut av motorcykel
        //private static void HanteraMotorcykel(List<ParkingSpot> parkering, int i, int j, string regNummer, double totalMinutes)
        //{
        //    parkering[i].Vehicles.RemoveAt(j);
        //    parkering[i].Capacity += 0.5;

        //    double cost = totalMinutes * (1.5 / 2);
        //    Console.WriteLine($"Motorcykel med regnummer {regNummer} lämnade parkeringen.");
        //    Console.WriteLine($"Total parkeringstid var: {totalMinutes} minuter. Att betala: {cost} kr.");
        //}

        //// Metod för att hantera avslut av buss
        //private static void HanteraBuss(List<ParkingSpot> parkering, int i, int j, string regNummer, double totalMinutes)
        //{
        //    parkering[i].Vehicles.RemoveAt(j);
        //    parkering[i].Capacity += 1;

        //    // Hantera den extra platsen för bussen
        //    if (i + 1 < parkering.Count)
        //    {
        //        for (int k = 0; k < parkering[i + 1].Vehicles.Count; k++)
        //        {
        //            if (parkering[i + 1].Vehicles[k].RegNummer == regNummer)
        //            {
        //                parkering[i + 1].Vehicles.RemoveAt(k);
        //                parkering[i + 1].Capacity += 1;

        //                double cost = totalMinutes * (1.5 * 2);
        //                Console.WriteLine($"Buss med regnummer {regNummer} lämnade parkeringarna {parkering[i].Number} och {parkering[i + 1].Number}.");
        //                Console.WriteLine($"Total parkeringstid var: {totalMinutes} minuter. Att betala: {cost} kr.");
        //                break;
        //            }
        //        }
        //    }
        //}

        //// Kontrollera om en plats är ledig och uppdatera status
        //private static void KontrolleraOchUppdateraLedigPlats(List<ParkingSpot> parkering, int i)
        //{
        //    if (parkering[i].Capacity == 1 && parkering[i].Vehicles.Count == 0)
        //    {
        //        parkering[i].IsFree = true;
        //    }
        //    if (i + 1 < parkering.Count && parkering[i + 1].Capacity == 1 && parkering[i + 1].Vehicles.Count == 0)
        //    {
        //        parkering[i + 1].IsFree = true;
        //    }
        //}


    }
}
