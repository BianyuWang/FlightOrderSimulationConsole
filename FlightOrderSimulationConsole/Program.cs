// See https://aka.ms/new-console-template for more information
using FlightOrderSimulationConsole.IService;
using FlightOrderSimulationConsole.Models;
using FlightOrderSimulationConsole.Service;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Dynamic;

Console.WriteLine("Hello, Transport.ly!");




int input = 0,   dayInput=0;
bool isNumber;
List<OrderComeIn> orders;
List<OrderDetailDisplay> orderDetails;
IDataAccessService DataAccessService = new DataAccessService();


IPrintService PrintService = new PrintService(DataAccessService);
IOrderFlightService OrderFlightService = new OrderFlightService(DataAccessService);
while (input != 5)
{
    PrintService.PrintMenu();

    //Parse input, Check if the inputs are as required
    isNumber = Int32.TryParse(Console.ReadLine(), out input);

    //if not meet the requirement, let user choose from the beginning 
    if (!isNumber || input > 5)
    {
        Console.WriteLine("Please enter a number from 1 to 4");
        continue;
    }

    //Access to functions based on user selection
    switch (input)
    {
        case 1:
            foreach (var schedule in DataAccessService.GetFlightScheduleList())
            {
                PrintService.PrintDetail(schedule);
            }
            break;
        case 2:
            Console.WriteLine("Please enter a specific day to check the Schedule");
          
            isNumber = Int32.TryParse(Console.ReadLine(), out dayInput);
            if (isNumber)
               PrintService.PrintDetail(DataAccessService.GetFlightScheduleByDay(dayInput % 2), day: dayInput);
            else
                continue;
            break;
        case 3:
            PrintService.PrintFlightScheduleByFlight();
            break;
        case 4:
            Console.WriteLine("How many days do you want to schedule?(try 1 an 2 to see the differece)");
            isNumber = Int32.TryParse(Console.ReadLine(), out dayInput);
            if (isNumber)
            {   //read orders information from json file.
                orders = OrderFlightService.ReadOrderFromJson();
                //schedule orders
                orderDetails = OrderFlightService.AddOrderToFlights(orders, dayInput);

                //display order details
                foreach (var order in orderDetails)
                {
                    PrintService.PrintOrder(order);

                }

            }
            else
                continue;           
            break;
        default:
            break;
    }

}








