using FlightOrderSimulationConsole.IService;
using FlightOrderSimulationConsole.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightOrderSimulationConsole.Service
{
    public class OrderFlightService : IOrderFlightService
    {
        private readonly IDataAccessService _dataAccessService;
        public OrderFlightService(IDataAccessService dataAccessService)
        {
            _dataAccessService = dataAccessService;
        }

        /// <summary>
        /// Assign each order to flight,
        /// To show the difference,  users can choose how many days
        /// they want to generate the flight schedule
        /// </summary>
        public List<OrderDetailDisplay> AddOrderToFlights(List<OrderComeIn> orders, int ForHowManyDays)
        {
            //list of filght schedule, (day X , flight with their capacity)
            List<FlightSchedule> FlightSchedules = new List<FlightSchedule>();

            //order details which should be return at the end
            List<OrderDetailDisplay> orderDetails = new List<OrderDetailDisplay>();

            //1.count numbers of orders by destination, Quantity will be used to
            //  calculate how many orders have not been processed 
            var orderDestCount = orders.GroupBy(l => l.Destination)
                    .Select(cl => new OrderGroup
                    {
                        Destination = cl.First().Destination,
                        Quantity = cl.Count(),
                    }).ToList();

            //2. Schedule daily flights according to user input
            for (int i = 0; i < ForHowManyDays; i++)
            {
                FlightSchedule schedule = new FlightSchedule();
                schedule.DepartureDate = i + 1;
                schedule.Flights = _dataAccessService.GetFlightScheduleByDay(i%2).Flights;

                //Arrange orders for each flight
                foreach (var flight in schedule.Flights)
                {
                    //get destination of flight
                    AirportAbbr destination = _dataAccessService.GetAirLineById(flight.AirLineId).ArriveAirport;

                    //Find the order of the same destination
                    var orderGroup = orderDestCount.Where(orders => orders.Destination == destination).FirstOrDefault();

                    if (orderGroup != null)
                    {
                        //assign the remaining orders to the flight

                        //Get the number of orders to be shipped
                        int arrangedOrderNumber = orderGroup.Quantity > 20 ? 20 : orderGroup.Quantity;

                        //loading orders
                        var ordersOnBord = orders.Where(o => o.Destination == destination).Skip(i * 20).Take(arrangedOrderNumber).ToList();
                        foreach (var order in ordersOnBord)
                        {
                            OrderDetailDisplay orderDetail = new OrderDetailDisplay()
                            {
                                AirLineId = flight.AirLineId,
                                OrderId = order.OrderNumber,
                                DepartureDate = schedule.DepartureDate,
                                FlightId = flight.FlightId
                            };

                            orderDetails.Add(orderDetail);
                            flight.Loading++;
                        }

                        //Get the  number of orders that are not on borad.
                        orderGroup.Quantity -= arrangedOrderNumber;

                    }



                }
                FlightSchedules.Add(schedule);

            }

            //After all the flights are arranged, get the remaining orders
            foreach (var orderGroup in orderDestCount)
            {
                if (orderGroup.Quantity > 0)
                {
                    var orderNoOnbord = orders.Where(o => o.Destination == orderGroup.Destination).TakeLast(orderGroup.Quantity).ToList();
                    foreach (var order in orderNoOnbord)
                    {
                        OrderDetailDisplay ord = new OrderDetailDisplay()
                        {
                            OrderId = order.OrderNumber
                        };
                        orderDetails.Add(ord);
                    }

                }

            }

            //sort the order detail by order number before returning 
            return orderDetails.OrderBy(o => o.OrderId).ToList();
        }

        /// <summary>
        /// read json file, I think the format of Json is...Confusing, but I still use the original file, 
        /// here I use the Dynamic.ExpandoObject to get the order information 
        /// </summary>
        public List<OrderComeIn> ReadOrderFromJson()
        {
            List<OrderComeIn> orders = new List<OrderComeIn>();

            //get file
            var path = Directory.GetCurrentDirectory();
            int first = path.IndexOf("bin");
            var jsonfile = path.Substring(0, first) + "files\\orders.json";

            //read file into dynamic object
            StreamReader r = new StreamReader(jsonfile);
            string jsonString = r.ReadToEnd();
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            dynamic details = JsonConvert.DeserializeObject<ExpandoObject>(jsonString, new ExpandoObjectConverter());
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            // convert to a dictionary, key is the order number, and value should be an other key value pair
            var expandoDict = details as IDictionary<string, object>;

            foreach (var i in expandoDict)
            {
                OrderComeIn order = new OrderComeIn()
                {
                    OrderNumber = i.Key,
                    Destination = (AirportAbbr)Enum.Parse(typeof(AirportAbbr), ((dynamic)i.Value).destination, true)
                };

                orders.Add(order);
            }

            //before return the result, sort order list by destination then by order number
            return orders.OrderBy(o => o.Destination).ThenBy(o => o.OrderNumber).ToList();
        }
    }
}
