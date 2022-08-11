using FlightOrderSimulationConsole.Data;
using FlightOrderSimulationConsole.IService;
using FlightOrderSimulationConsole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightOrderSimulationConsole.Service
{
    public class DataAccessService : IDataAccessService
    {
        public AirLine GetAirLineById(int id)
        {
          return  FlightScheduledData.AirLines.Where(a => a.AirLineId == id).FirstOrDefault();

        }

        public FlightSchedule GetFlightScheduleByDay(int dayAt)
        {
            return GetFlightScheduleList().ElementAt(dayAt);
        }

        public List<FlightSchedule> GetFlightScheduleList()
        {
            return FlightScheduledData.FlightScheduleList;
        }

        public void LoadFlightScheduleByFlight()
        {
            throw new NotImplementedException();
        }
    }
}
