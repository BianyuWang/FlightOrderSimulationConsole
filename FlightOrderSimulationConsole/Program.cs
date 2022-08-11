// See https://aka.ms/new-console-template for more information
using FlightOrderSimulationConsole.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Dynamic;

Console.WriteLine("Hello, Transport.ly!");




int input = 0;
bool isNumber;
List<OrderComeIn> orders;
List<OrderDetailDisplay> orderDetails;
//print menu


while (input != 4)
{
    printMenu();

    //Parse input, Check if the inputs are as required
    isNumber = Int32.TryParse(Console.ReadLine(), out input);

    //if not meet the requirement, let user choose from the beginning 
    if (!isNumber || input > 4)
    {
        Console.WriteLine("Please enter a number from 1 to 4");
        continue;
    }

    //Access to functions based on user selection
    switch (input)
    {
        case 1:
            LoadFlightSchedule();
            break;
        case 2:
            Console.WriteLine("Please enter a specific day to check the Schedule");
            isNumber = Int32.TryParse(Console.ReadLine(), out input);
            if (isNumber)
                LoadFlightScheduleByDay(input);
            else
                continue;
            break;
        case 3:
            LoadFlightScheduleByFlight();
            break;
        case 4:
            Console.WriteLine("How many days do you want to schedule?(try 1 an 2 to see the differece)");
            isNumber = Int32.TryParse(Console.ReadLine(), out input);
            if (isNumber)
            {   //read orders information from json file.
                orders = ReadOrderFromJson();
                //schedule orders
                orderDetails =AddOrderToFlights(orders, input);

                //display order details
                foreach (var order in orderDetails)
                { 
                    //if order is already scheduled
                    if (order.DepartureDate != 0)
                    {
                        AirLine airline = FlightScheduledData.AirLines.Where(a => a.AirLineId == order.AirLineId).FirstOrDefault();
                        Console.WriteLine($"order : {order.OrderId}, FlightNumber : {order.FlightId}, " +
                            $"departure : {(City)airline.DepartAirport}, arrival : {(City)airline.ArriveAirport}, day {order.DepartureDate}");
                    }
                    //if order has not yet been scheduled
                    else
                        Console.WriteLine($"order : {order.OrderId}, FlightNumber : not Scheduled ");

                }

            }
            else
                continue;           
            break;
        default:
            break;
    }

}







/// <summary>
/// Print menu, allowing user to make choices
/// </summary>

static void printMenu()
{
    Console.WriteLine("-------------------------------------------------");
    Console.WriteLine("Please select from the menu");
    Console.WriteLine("1// Load Flight Schedule");
    Console.WriteLine("2// Check Schedule by day");
    Console.WriteLine("3// List out flight schedule");
    Console.WriteLine("4// Give a specific number of days to schedule the order");
    Console.WriteLine("-------------------------------------------------");

    Console.WriteLine("5// Exit");
}

/// <summary>
/// Load flight schedule (When user selects 1 from menu)
/// </summary>
static void LoadFlightSchedule()
{
    foreach (var schedule in FlightScheduledData.FlightScheduleList)
    {
        PrintDetail(schedule);
    }

}

/// <summary>
/// print flight infornations, Include flight number, arrival and destination
/// </summary>

static void PrintFlight(Flight flight, bool isByFlight = false, int day = 0)
{

    var airline = FlightScheduledData.AirLines.Where(a => a.AirLineId == flight.AirLineId).FirstOrDefault();
    if (airline != null)
    {

        var msg = $"Flight {flight.FlightId} :" +
           $" {(City)airline.DepartAirport}({airline.DepartAirport})" +
           $"to " +
           $"{(City)airline.ArriveAirport}({airline.ArriveAirport})";

        if (isByFlight)
            msg = $"Flight {flight.FlightId} :" +
               $" Departure :{airline.DepartAirport}," +
             $" Arrival :{airline.ArriveAirport},  day: {day}";

        Console.WriteLine(msg);
    }
}


/// <summary>
/// Display flight information for each day
/// </summary>

static void PrintDetail(FlightSchedule schedule, bool isByFlight = false, int day = 0)
{
    day = day == 0 ? schedule.DepartureDate : day;

    if (!isByFlight)
        Console.WriteLine($"Day {day}");
    schedule.Flights = schedule.Flights.OrderBy(f => f.FlightId).ToList();
    foreach (var flight in schedule.Flights)
    { 
        PrintFlight(flight, isByFlight, day);
    }
}

/// <summary>
/// Users can enter a special day to check the flight of the day
/// </summary>
static void LoadFlightScheduleByDay(int input)
{


    if (input % 2 == 0)
        PrintDetail(FlightScheduledData.FlightScheduleList.ElementAt(1), day: input);
    else
        PrintDetail(FlightScheduledData.FlightScheduleList.ElementAt(0), day: input);

}

/// <summary>
/// Sort by flight number to display detail information
/// </summary>
static void LoadFlightScheduleByFlight()
{
    List<Flight> flights = new List<Flight>();
    foreach (var flightList in FlightScheduledData.FlightScheduleList)
    {
        flightList.Flights.ForEach(item => flights.Add(item));
    }
    flights = flights.OrderBy(item => item.FlightId).ToList();

    foreach (var flight in flights)
    {
        int dayScheduled = 0;
        foreach (var schedule in FlightScheduledData.FlightScheduleList)

        {
            if (schedule.Flights.Any(d => d.FlightId == flight.FlightId))
            {
                dayScheduled = schedule.DepartureDate;
                break;
            }
        }

        PrintFlight(flight, isByFlight: true, day: dayScheduled);
    }

}

/// <summary>
/// read json file, I think the format of Json is...Confusing, but I still use the original file, 
/// here I use the Dynamic.ExpandoObject to get the order information 
/// </summary>

static List<OrderComeIn> ReadOrderFromJson()
{
    List<OrderComeIn> orders = new List<OrderComeIn>();

    //get file
    var path = Directory.GetCurrentDirectory();
    int first = path.IndexOf("bin");
    var jsonfile = path.Substring(0, first) + "files\\orders.json";

    //read file into dynamic object
    StreamReader r = new StreamReader(jsonfile);
    string jsonString = r.ReadToEnd();
    dynamic details = JsonConvert.DeserializeObject<ExpandoObject>(jsonString, new ExpandoObjectConverter());
   
    // convert to a dictionary, key is the order number, and value should be an other key value pair
    var expandoDict = details as IDictionary<string, object>;
   
    foreach (var i in expandoDict)
    {
        OrderComeIn order = new OrderComeIn() {
            OrderNumber = i.Key,
            Destination = (AirportAbbr)Enum.Parse(typeof(AirportAbbr), ((dynamic)i.Value).destination, true)

    };
        
        orders.Add(order);
    }

   //before return the result, sort order list by destination then by order number
    return orders.OrderBy(o=>o.Destination).ThenBy(o => o.OrderNumber).ToList();
}

/// <summary>
/// Assign each order to flight,
/// To show the difference,  users can choose how many days
/// they want to generate the flight schedule
/// </summary>
static List<OrderDetailDisplay> AddOrderToFlights(List<OrderComeIn> orders, int days)
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
    for (int i = 0; i < days; i++)
    {
        FlightSchedule schedule = new FlightSchedule();
        schedule.DepartureDate = i + 1;
        schedule.Flights = FlightScheduledData.FlightScheduleList.ElementAt(i % 2).Flights;

        //Arrange orders for each flight
        foreach (var flight in schedule.Flights)
        {
            //get destination of flight
            AirportAbbr destination = FlightScheduledData.AirLines.Where(a => a.AirLineId == flight.AirLineId).First().ArriveAirport;

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
    return orderDetails.OrderBy(o=>o.OrderId).ToList();
}