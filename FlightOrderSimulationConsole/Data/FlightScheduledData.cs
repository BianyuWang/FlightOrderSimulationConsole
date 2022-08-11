using FlightOrderSimulationConsole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightOrderSimulationConsole.Data
{
   public static class FlightScheduledData
    {
      public  static List<AirLine> AirLines = new List<AirLine>()
        { new AirLine{AirLineId = 1, DepartAirport=AirportAbbr.YUL,ArriveAirport= AirportAbbr.YYZ},
          new AirLine{ AirLineId=2,DepartAirport = AirportAbbr.YUL,ArriveAirport= AirportAbbr.YYC},
          new AirLine{ AirLineId=3, DepartAirport = AirportAbbr.YUL,ArriveAirport= AirportAbbr.YVR},
        };


        public static List<FlightSchedule> FlightScheduleList = new List<FlightSchedule>()
        {


        new FlightSchedule() {
         
        DepartureDate= 1,
        Flights= new List<Flight> {
        new Flight{ FlightId=1,AirLineId=1},
        new Flight{ FlightId=3,AirLineId=3},
        new Flight{ FlightId=2,AirLineId=2},
        }},
               new FlightSchedule() {
        DepartureDate = 2,
        Flights= new List<Flight> {
        new Flight{ FlightId=4,AirLineId=1},
        new Flight{ FlightId=6,AirLineId=3},
        new Flight{ FlightId=5,AirLineId=2},
        }}
        };


    }
}
