﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace GarageBooker
{
    public interface ICommon
    {
        BookingModel GetModelFromString(string row);
        List<BookingModel> ListAppointmentBookedPerDay(string pathFile, int maxNextDays);
        string ShowFreeTimeNextTenDays(Dictionary<DateTime, List<BookingModel>> listBookedAppointments, string startBusiness, string closeBusiness);
    }
    
    public class Common : ICommon
    {
        private IHelper _helper = null;
        public Common(IHelper helper)
        {
            _helper = helper;
        }
        public List<BookingModel> ListAppointmentBookedPerDay(string pathFile, int maxNextDays)
        {
            var listAppointments = new List<BookingModel>();

            var listRowsFromFile = _helper.GetRowsFromFile(pathFile, maxNextDays);
            foreach (var row in listRowsFromFile)
            {
                var model = GetModelFromString(row);
                if (model == null)
                {
                    new BookingModel();
                }
                listAppointments.Add(model);
            }
           
            return listAppointments;
        }
        public string ShowFreeTimeNextTenDays(Dictionary<DateTime, List<BookingModel>> listBookedAppointments, string startBusiness, string closeBusiness)
        {
            var errorMessage = "There is an error in this file. Please review it and fix the issue";
            var showAvailableTime = new StringBuilder();
            foreach (var appointment in listBookedAppointments)
            {
                showAvailableTime.AppendLine(appointment.Key.ToString("dddd dd MMMM"));
                showAvailableTime.AppendLine("");
                if (appointment.Value.Count == 0)
                {
                    showAvailableTime.AppendLine("All mechanics are avaialble in this day");
                }
                else
                {
                    var listMechanics = new Dictionary<string, SortedDictionary<DateTime, DateTime>>();
                    
                    foreach (var booked in appointment.Value)
                    {
                        try
                        {
                            if (booked == null || string.IsNullOrEmpty(booked.Name))
                            {
                                showAvailableTime.AppendLine(errorMessage);
                                continue;
                            }
                            if (!listMechanics.Keys.Contains(booked.Name))
                            {
                                listMechanics.Add(booked.Name, new SortedDictionary<DateTime, DateTime>());
                            }
                            listMechanics[booked.Name].Add(booked.StartTime, booked.EndTime);
                        }
                        catch(Exception)
                        {
                            showAvailableTime.AppendLine(errorMessage);
                            continue;
                        }
                    }

                    var mechanics = ShowAvailableTimeEachMechanic(listMechanics, startBusiness, closeBusiness);
                    showAvailableTime.AppendLine(mechanics);
                    showAvailableTime.AppendLine("All the other mechanics are avaialble in this day");
                }
                showAvailableTime.AppendLine("");
            }
            return showAvailableTime.ToString();
        }

        public BookingModel GetModelFromString(string row)
        {
            if (string.IsNullOrEmpty(row))
            {
                return null;
            }
            var model = new BookingModel();
            var splitRow = row.Split(',');
            var totalItemsInRow = 5;
            if (splitRow.Length != totalItemsInRow)
            {
                return null;
            }
            try
            {
                foreach (var item in splitRow)
                {
                    var splitItem = item.Split(':');
                    if (splitItem[0].ToLower().Contains("name"))
                    {
                        model.Name = splitItem[1].Trim();
                    }
                    else if (splitItem[0].ToLower().Contains("date"))
                    {
                        var convertedDate = splitItem[1].Replace("/", "-").Trim();
                        model.Date = DateTime.ParseExact(convertedDate, "dd-MM-yyyy",
                                           System.Globalization.CultureInfo.InvariantCulture);
                    }
                    else if (splitItem[0].ToLower().Contains("start time"))
                    {
                        var time = splitItem[1].Replace(":", ".").Trim();

                        var timeFormat = Helper.ParseTime(time);
                        model.StartTime = model.Date.Add(timeFormat);

                    }
                    else if (splitItem[0].ToLower().Contains("end time"))
                    {
                        var time = splitItem[1].Replace(":", ".").Trim();
                        var timeFormat = Helper.ParseTime(time);
                        model.EndTime = model.Date.Add(timeFormat);
                    }
                    else if (splitItem[0].ToLower().Contains("description"))
                    {
                        model.Description = Helper.RemoveSpecialCharacters(splitItem[1].Trim());
                    }

                }
            }
            catch(Exception)
            {
                model = null;

            }
            return model;
        }

        private static string ShowAvailableTimeEachMechanic(Dictionary<string, SortedDictionary<DateTime, DateTime>> busyTime, string startBusiness, string closeBusiness)
        {
            var message = new StringBuilder();

            foreach (var mechanic in busyTime)
            {
                var startTimeAvailable = startBusiness;
                var stopTimeWork = closeBusiness;
                var mechanicName = mechanic.Key;
                message.AppendLine("");
                message.AppendLine(mechanicName + " is available:");
                message.AppendLine("");
                foreach (var time in mechanic.Value)
                {
                    var timeFormat = Helper.ParseTime(startTimeAvailable);
                    var startingLastAvaialble = new DateTime(time.Key.Year, time.Key.Month, time.Key.Day, 0, 0, 0);
                    startingLastAvaialble = startingLastAvaialble.Add(timeFormat);
                    if (startingLastAvaialble < time.Key)
                    {
                        message.AppendLine("from " + startingLastAvaialble.ToString("HH.mm") + " to " + time.Key.ToString("HH.mm"));
                        stopTimeWork = time.Value.ToString("HH.mm");
                    }
                    startTimeAvailable = time.Value.ToString("HH.mm");
                }
                if (stopTimeWork != closeBusiness)
                {
                    message.AppendLine("from " + stopTimeWork + " to " + closeBusiness);
                }

            }
            return message.ToString();
        }
    }
}
