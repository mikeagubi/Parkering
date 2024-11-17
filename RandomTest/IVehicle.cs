using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomTest
{
    internal interface IVehicle
    {
        string RegNummer { get; }
        string Color { get; set; }
        double Size();
        
        
    }
}
