using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RandomTest
{
    internal class ParkingSpot
    {
        public int Number { get; private set; }
        public double Capacity { get; set; }
        public List<Vehicle> Vehicles { get; set; }
        public bool IsFree { get; set; }
        public DateTime ParkedDate { get; set; }

        public ParkingSpot(int number)
        {
            Number = number;
            Capacity = 1;
            IsFree = true;
            Vehicles = new List<Vehicle>();
        }

        public static List<ParkingSpot> CreateParkingList()
        {
            List<ParkingSpot> list = new List<ParkingSpot>();
            for (int i = 0; i < 15; i++)
            {
                list.Add(new ParkingSpot(i)); 
            }
            return list;
        }
    }
}
