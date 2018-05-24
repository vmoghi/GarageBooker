using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GarageBooker
{
    abstract class GarageFactory
    {
        public abstract IGarage BuildBooking();
    }

    class BookingPlain : GarageFactory
    {
        public override IGarage BuildBooking()
        {
            return new PlainTextSource();
        }
    }

    public interface IGarage
    {
        string StoreNewAppointment(string line);
        string ShowAvailableAppointments();
    }
    public class PlainTextSource : IGarage
    {
        private const string startBusiness="09.00";
        private const string closeBusiness = "17.30";
        private const int maxNextDays = 10;
        private const string pathFile = "../../Files";

        public string StoreNewAppointment(string newAppoinmentData)
        {
            var statusBooking = "The appointment has be booked";
            var newAppointment = Common.GetModelFromString(newAppoinmentData);
            if (newAppointment==null)
            {
                statusBooking = "The data insered are incorrect. Please try again follow the correct format.";
                return statusBooking;
            }
            var bookingDate = newAppointment.Date.ToString("dd-MM-yyyy"); ;
            var filePath = Path.Combine(pathFile, bookingDate + ".txt");

            if(newAppointment.StartTime<DateTime.Now)
            {
                statusBooking = "The appointment cannot be booked with time in the past";
                return statusBooking;
            }

            Helper.CheckFileExists(filePath);
            var listBooked = Common.ListAppointmentBookedPerDay(filePath, maxNextDays);
            foreach (var appointment in listBooked)
            {
                if (appointment.Name.ToLower() == newAppointment.Name.ToLower() && appointment.StartTime <= newAppointment.StartTime && appointment.EndTime >= newAppointment.StartTime)
                {
                    statusBooking = "The appointment cannot be booked as " + appointment.Name + " has already an appointment between " + appointment.StartTime.ToString("HH:mm") + " and " + appointment.EndTime.ToString("HH:mm");
                    return statusBooking;
                }
            }
            using (StreamWriter tw = File.AppendText(filePath))
            {
                tw.WriteLine(newAppoinmentData);
                tw.WriteLine("");
                tw.Flush();
            }

            return statusBooking;
        }

        public string ShowAvailableAppointments()
        {
            var showAvailableTime = "";
            
                var listBookedAppointments = new Dictionary<DateTime, List<BookingModel>>();

                for (int i = 0; i < maxNextDays; i++)
                {
                    var bookingDate = DateTime.Now.AddDays(i);
                    var filePath = Path.Combine(pathFile, bookingDate.ToString("dd-MM-yyyy") + ".txt");
                    try
                    {
                        var listDayAppointments = Common.ListAppointmentBookedPerDay(filePath, maxNextDays);
                        listBookedAppointments.Add(bookingDate, listDayAppointments);
                    }
                    catch (Exception)
                    {
                        showAvailableTime = "There is a  fatal error in the file "+filePath;
                    }
                }
                showAvailableTime = Common.ShowFreeTimeNextTenDays(listBookedAppointments, startBusiness, closeBusiness);
            
            return showAvailableTime;
        }

       


        









       

        

       
    }
}
