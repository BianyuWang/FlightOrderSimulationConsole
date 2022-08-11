using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightOrderSimulationConsole.Models
{
    public class Order
    {
        public string OrderId { get; set; }
        public AirportAbbr Destination  { get; set; }
    }

    public class OrderGroup
    {
       
        public AirportAbbr Destination { get; set; }
        public int Quantity { get; set; }
    }

}
