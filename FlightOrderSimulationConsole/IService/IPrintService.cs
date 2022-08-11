using FlightOrderSimulationConsole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightOrderSimulationConsole.IService
{
    public interface  IPrintService
{
        void PrintOrder(OrderDetailDisplay order);
        void PrintMenu();
        void PrintFlight(Flight flight, bool isByFlight = false, int day = 0);
        void PrintDetail(FlightSchedule schedule, bool isByFlight = false, int day = 0);

        void PrintFlightScheduleByFlight();
    }
}
