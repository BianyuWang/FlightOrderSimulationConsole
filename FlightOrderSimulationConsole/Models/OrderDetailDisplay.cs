using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightOrderSimulationConsole.Models
{
    public class OrderDetailDisplay
    {
        public string OrderId { get; set; }
        public int AirLineId { get; set; }
        public int FlightId { get; set; }
        public int DepartureDate { get; set; }

        public OrderDetailDisplay()
        {
            AirLineId = 0;
            FlightId = 0;
            DepartureDate=0;
        }
    }
}