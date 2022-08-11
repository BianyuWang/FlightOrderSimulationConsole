using FlightOrderSimulationConsole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightOrderSimulationConsole.IService
{
    public interface IOrderFlightService
    {
       public List<OrderComeIn> ReadOrderFromJson();
        public List<OrderDetailDisplay> AddOrderToFlights(List<OrderComeIn> orders, int ForHowManyDays);
    }
}
