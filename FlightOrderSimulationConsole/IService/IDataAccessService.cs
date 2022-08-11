using FlightOrderSimulationConsole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightOrderSimulationConsole.IService
{
    public interface IDataAccessService
    {
        public AirLine GetAirLineById(int id);
        public List<FlightSchedule> GetFlightScheduleList();
        public FlightSchedule GetFlightScheduleByDay(int dayAt);
        void LoadFlightScheduleByFlight();
    }
}
