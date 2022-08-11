using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightOrderSimulationConsole.Models
{
    public class OrderComeIn 
    {
        public string OrderNumber { get; set; }
        public AirportAbbr Destination { get; set; }
    }
}
