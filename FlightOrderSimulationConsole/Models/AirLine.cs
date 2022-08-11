using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightOrderSimulationConsole.Models
{
    public class AirLine
    {
        public int AirLineId { get; set; }
        public AirportAbbr DepartAirport { get; set; }
        public AirportAbbr ArriveAirport { get; set; }
    }
}
