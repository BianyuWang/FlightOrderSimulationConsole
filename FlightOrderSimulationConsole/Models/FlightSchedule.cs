using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightOrderSimulationConsole.Models
{
    public class FlightSchedule
    {
        public int DepartureDate { get; set; }
        public List<Flight>? Flights { get; set; }
    }
}
