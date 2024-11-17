using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomTest
{
    internal abstract class Vehicle : IVehicle
    {
        public string RegNummer { get; private set; }
        public string Color { get; set; }

        public Vehicle(string color)
        {
            RegNummer = "ABC" + Random.Shared.Next(100, 999).ToString();
            Color = color;
        }
        public abstract double Size();
        

    }

    internal class Car : Vehicle
    {
        public bool IsElectric { get; set; }
        public Car(bool isElectric, string color) : base(color)
        {
            IsElectric = isElectric;
        }
        public override double Size()
        {
            return 1;
        }
    }

    internal class Motorcycle : Vehicle
    {
        public string Brand { get; set; }
        public Motorcycle(string brand, string color) : base(color)
        {
            Brand = brand;
        }
        public override double Size()
        {
            return 0.5;
        }

    }

    internal class Bus : Vehicle
    {
        public int Passenger { get; set; }
        public Bus(int passenger, string color) : base(color)
        {
            Passenger = passenger;
        }
        public override double Size()
        {
            return 2;
        }
    }
}
