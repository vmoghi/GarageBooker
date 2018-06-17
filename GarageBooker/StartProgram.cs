using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageBooker
{
    class StartProgram
    {
        static void Main(string[] args)
        {
            string userInput = "";
            IHelper helper = new InstanceHelper();
            ICommon common = new Common(helper);
            var buildGarage = new BookingPlain();
            IGarage garage = buildGarage.BuildBooking(common, helper);

            Console.WriteLine("Type 'new' to store a new appoinment, 'view' to show available time or 'exit' to stop");

            while (userInput != "exit")
            {
                Console.Write("> ");
                userInput = Console.ReadLine().ToLower();

                switch (userInput)
                {
                    case "exit":
                        break;
                    case "new":
                        {
                            StoreNewAppoinment(garage);
                            break;
                        }
                    case "view":
                        {
                            ShowBookedAppointments(garage);
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("\"{0}\" is not a recognized command.", userInput);
                            break;
                        }
                }
            }
        }
        static void StoreNewAppoinment(IGarage garage)
        {
            var newAppoinment = new StringBuilder();
           
            Console.WriteLine("Type name:");
            var name = "Name:" + Console.ReadLine() + ",";
            newAppoinment.Append(name);
            Console.WriteLine("Type date(dd-mm-yyyy):");
            var date = "Date:" + Console.ReadLine() + ",";
            newAppoinment.Append(date);
            Console.WriteLine("Type start time(hh.mm):");
            var startTime = "Start Time:" + Console.ReadLine()+",";
            newAppoinment.Append(startTime);
            Console.WriteLine("Type end time (hh.mm):");
            var endTime = "End Time:" + Console.ReadLine() + ",";
            newAppoinment.Append(endTime);

            Console.WriteLine("Type a description:");
            var description = "Description:" + Console.ReadLine();
            newAppoinment.Append(description);

            var bookedNewAppoinment = garage.StoreNewAppointment(newAppoinment.ToString());
            Console.WriteLine(bookedNewAppoinment);
        }
        static void ShowBookedAppointments(IGarage garage)
        {
            var showAvailability = garage.ShowAvailableAppointments();
            Console.WriteLine("Show timetable available mechanics:");
            Console.WriteLine("_______________________________________________________________");
            Console.WriteLine(showAvailability);
            Console.WriteLine("_______________________________________________________________");
            Console.WriteLine("");
        }
    }
}
