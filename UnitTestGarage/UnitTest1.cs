using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GarageBooker;
using Microsoft.QualityTools.Testing.Fakes;
using System.Collections.Generic;
using System.Text;
namespace UnitTestGarage
{
    [TestClass]
    public class UnitTest1
    {
        IGarage _garage = null;
        [TestInitialize]
        public void SetUp()
        {
            _garage = new PlainTextSource();
        }
        [TestMethod]
        public void GarageFacroty_StoreNewAppointment_Return_error_Message_When_The_Model_Is_Null()
        {
            using (ShimsContext.Create())
            {
                GarageBooker.Fakes.ShimCommon.GetModelFromStringString = (row) =>
                {
                    return null;
                };

                var result = _garage.StoreNewAppointment("test");
                Assert.AreEqual(true, result.StartsWith("The data insered are incorrect"));
            }
        }
        [TestMethod]
        public void GarageFacroty_StoreNewAppointment_Return_error_Date_When_The_Model_Has_Date_In_The_Past()
        {
            using (ShimsContext.Create())
            {
                GarageBooker.Fakes.ShimCommon.GetModelFromStringString = (row) =>
                {
                    var bookingModel = new BookingModel();
                    bookingModel.StartTime = DateTime.Parse("01-06-2018 12:00");
                    return bookingModel;
                };

                var result = _garage.StoreNewAppointment("test");
                Assert.AreEqual(true, result.Contains("The appointment cannot be booked with time in the past"));
            }
        }
        [TestMethod]
        public void GarageFacroty_StoreNewAppointment_Return_error_Cannot_Be_Booked_When_The_Mechanic_Has_Already_Date_Booked()
        {
            using (ShimsContext.Create())
            {
                GarageBooker.Fakes.ShimCommon.GetModelFromStringString = (row) =>
                {
                    var bookingModel = new BookingModel();
                    bookingModel.Name = "Mario";
                    bookingModel.StartTime = DateTime.Today.Add(new TimeSpan(3, 13, 0, 0));
                    bookingModel.EndTime = DateTime.Today.Add(new TimeSpan(3, 15, 0, 0));
                    return bookingModel;
                };
                GarageBooker.Fakes.ShimCommon.ListAppointmentBookedPerDayStringInt32 = (filePath, maxNextDays) =>
                {
                    var listBookeAppoinments = new List<BookingModel>();
                    var bookingModel = new BookingModel();
                    bookingModel.Name = "Mario";
                    bookingModel.StartTime = DateTime.Today.Add(new TimeSpan(3,12,0,0));
                    bookingModel.EndTime = DateTime.Today.Add(new TimeSpan(3, 14, 0, 0));
                    listBookeAppoinments.Add(bookingModel);
                    return listBookeAppoinments;
                };
                var result = _garage.StoreNewAppointment("test");
                Assert.AreEqual(true, result.Contains("The appointment cannot be booked as"));
            }
        }
        [TestMethod]
        public void GarageFacroty_StoreNewAppointment_Return_error_Cannot_Be_Booked_When_The_Mechanic_Has_Already_Booked_Same_Time_Cheking_minutes()
        {
            using (ShimsContext.Create())
            {
                GarageBooker.Fakes.ShimCommon.GetModelFromStringString = (row) =>
                {
                    var bookingModel = new BookingModel();
                    bookingModel.Name = "Mario";
                    bookingModel.StartTime = DateTime.Today.Add(new TimeSpan(3, 13, 0, 0));
                    bookingModel.EndTime = DateTime.Today.Add(new TimeSpan(3, 15, 20, 0));
                    return bookingModel;
                };
                GarageBooker.Fakes.ShimCommon.ListAppointmentBookedPerDayStringInt32 = (filePath, maxNextDays) =>
                {
                    var listBookeAppoinments = new List<BookingModel>();
                    var bookingModel = new BookingModel();
                    bookingModel.Name = "Mario";
                    bookingModel.StartTime = DateTime.Today.Add(new TimeSpan(3, 15, 15, 0));
                    bookingModel.EndTime = DateTime.Today.Add(new TimeSpan(3, 16, 0, 0));
                    listBookeAppoinments.Add(bookingModel);
                    return listBookeAppoinments;
                };
                var result = _garage.StoreNewAppointment("test");
                Assert.AreEqual(true, result.Contains("The appointment cannot be booked as"));
            }
        }
        [TestMethod]
        public void GarageFacroty_StoreNewAppointment_Is_Valid_Different_Mechanic_Same_Date_Booked()
        {
            using (ShimsContext.Create())
            {
                GarageBooker.Fakes.ShimCommon.GetModelFromStringString = (row) =>
                {
                    var bookingModel = new BookingModel();
                    bookingModel.Name = "Mario";
                    bookingModel.StartTime = DateTime.Today.Add(new TimeSpan(3, 13, 0, 0));
                    bookingModel.EndTime = DateTime.Today.Add(new TimeSpan(3, 15, 0, 0));
                    return bookingModel;
                };
                GarageBooker.Fakes.ShimCommon.ListAppointmentBookedPerDayStringInt32 = (filePath, maxNextDays) =>
                {
                    var listBookeAppoinments = new List<BookingModel>();
                    var bookingModel = new BookingModel();
                    bookingModel.Name = "Luigi";
                    bookingModel.StartTime = DateTime.Today.Add(new TimeSpan(3, 12, 0, 0));
                    bookingModel.EndTime = DateTime.Today.Add(new TimeSpan(3, 14, 0, 0));
                    listBookeAppoinments.Add(bookingModel);
                    return listBookeAppoinments;
                };
                GarageBooker.Fakes.ShimHelper.WriteToFileStringString = (filePath, newAppoinment) =>
                {
                };
                var result = _garage.StoreNewAppointment("test");
                Assert.AreEqual(true, result.Contains("The appointment has be booked"));
            }
        }
        [TestMethod]
        public void GarageFacroty_StoreNewAppointment_Is_Valid_Same_Mechanic_Different_Time_Booked()
        {
            using (ShimsContext.Create())
            {
                GarageBooker.Fakes.ShimCommon.GetModelFromStringString = (row) =>
                {
                    var bookingModel = new BookingModel();
                    bookingModel.Name = "Mario";
                    bookingModel.StartTime = DateTime.Today.Add(new TimeSpan(3, 13, 0, 0));
                    bookingModel.EndTime = DateTime.Today.Add(new TimeSpan(3, 15, 0, 0));
                    return bookingModel;
                };
                GarageBooker.Fakes.ShimCommon.ListAppointmentBookedPerDayStringInt32 = (filePath, maxNextDays) =>
                {
                    var listBookeAppoinments = new List<BookingModel>();
                    var bookingModel = new BookingModel();
                    bookingModel.Name = "Mario";
                    bookingModel.StartTime = DateTime.Today.Add(new TimeSpan(3, 15, 0, 0));
                    bookingModel.EndTime = DateTime.Today.Add(new TimeSpan(3, 17, 0, 0));
                    listBookeAppoinments.Add(bookingModel);
                    return listBookeAppoinments;
                };
                GarageBooker.Fakes.ShimHelper.WriteToFileStringString = (filePath, newAppoinment) =>
                {
                };
                var result = _garage.StoreNewAppointment("test");
                Assert.AreEqual(true, result.Contains("The appointment has be booked"));
            }
        }
        [TestMethod]
        public void GarageFacroty_StoreNewAppointment_Is_Valid_Same_Mechanic_Different_Date_Booked()
        {
            using (ShimsContext.Create())
            {
                GarageBooker.Fakes.ShimCommon.GetModelFromStringString = (row) =>
                {
                    var bookingModel = new BookingModel();
                    bookingModel.Name = "Mario";
                    bookingModel.StartTime = DateTime.Today.Add(new TimeSpan(3, 13, 0, 0));
                    bookingModel.EndTime = DateTime.Today.Add(new TimeSpan(3, 15, 0, 0));
                    return bookingModel;
                };
                GarageBooker.Fakes.ShimCommon.ListAppointmentBookedPerDayStringInt32 = (filePath, maxNextDays) =>
                {
                    var listBookeAppoinments = new List<BookingModel>();
                    var bookingModel = new BookingModel();
                    bookingModel.Name = "Mario";
                    bookingModel.StartTime = DateTime.Today.Add(new TimeSpan(5, 15, 0, 0));
                    bookingModel.EndTime = DateTime.Today.Add(new TimeSpan(5, 17, 0, 0));
                    listBookeAppoinments.Add(bookingModel);
                    return listBookeAppoinments;
                };
                GarageBooker.Fakes.ShimHelper.WriteToFileStringString = (filePath, newAppoinment) =>
                {
                };
                var result = _garage.StoreNewAppointment("test");
                Assert.AreEqual(true, result.Contains("The appointment has be booked"));
            }
        }
        [TestMethod]
        public void GarageFacroty_StoreNewAppointment_Is_Valid_Checking_Minutes()
        {
            using (ShimsContext.Create())
            {
                GarageBooker.Fakes.ShimCommon.GetModelFromStringString = (row) =>
                {
                    var bookingModel = new BookingModel();
                    bookingModel.Name = "Mario";
                    bookingModel.StartTime = DateTime.Today.Add(new TimeSpan(3, 13, 0, 0));
                    bookingModel.EndTime = DateTime.Today.Add(new TimeSpan(3, 15, 20, 0));
                    return bookingModel;
                };
                GarageBooker.Fakes.ShimCommon.ListAppointmentBookedPerDayStringInt32 = (filePath, maxNextDays) =>
                {
                    var listBookeAppoinments = new List<BookingModel>();
                    var bookingModel = new BookingModel();
                    bookingModel.Name = "Mario";
                    bookingModel.StartTime = DateTime.Today.Add(new TimeSpan(3, 15, 20, 0));
                    bookingModel.EndTime = DateTime.Today.Add(new TimeSpan(3, 17, 0, 0));
                    listBookeAppoinments.Add(bookingModel);
                    return listBookeAppoinments;
                };
                GarageBooker.Fakes.ShimHelper.WriteToFileStringString = (filePath, newAppoinment) =>
                {
                };
                var result = _garage.StoreNewAppointment("test");
                Assert.AreEqual(true, result.Contains("The appointment has be booked"));
            }
        }

        [TestMethod]
        public void GarageFacroty_ShowAvailableAppointments_Return_error_Message_When_The_Model_Is_Null()
        {

            PrivateObject maxDaysObj = new PrivateObject(_garage);
            maxDaysObj.SetField("maxNextDays",1);
            using (ShimsContext.Create())
            {
                GarageBooker.Fakes.ShimCommon.ListAppointmentBookedPerDayStringInt32 = (filePath, maxNextDays) =>
                {
                    throw new Exception();
                };

                var result = _garage.ShowAvailableAppointments();
                
                var errorMessageAspected = "There is a fatal error in the file";
                Assert.AreEqual(true, result.StartsWith(errorMessageAspected));
            }
        }
        [TestMethod]
        public void GarageFacroty_ShowAvailableAppointments_Return_Valid_Message_When_No_Appoinments_Booked_Today()
        {

            PrivateObject maxDaysObj = new PrivateObject(_garage);
            maxDaysObj.SetField("maxNextDays", 1);
            using (ShimsContext.Create())
            {
                GarageBooker.Fakes.ShimCommon.ListAppointmentBookedPerDayStringInt32 = (filePath, maxNextDays) =>
                {
                    return new List<BookingModel>();
                };

                var result = _garage.ShowAvailableAppointments();
                var todayDate = DateTime.Today.ToString("dddd dd MMMM");
                var appoinentAvailableAspected = todayDate + "\r\n\r\nAll mechanics are avaialble in this day";
                Assert.AreEqual(true, result.Contains(appoinentAvailableAspected));
            }
        }
        [TestMethod]
        public void GarageFacroty_ShowAvailableAppointments_Return_Valid_Message_With_Appoinments_Booked_Today()
        {

            PrivateObject maxDaysObj = new PrivateObject(_garage);
            maxDaysObj.SetField("maxNextDays", 1);
            using (ShimsContext.Create())
            {
                GarageBooker.Fakes.ShimCommon.ListAppointmentBookedPerDayStringInt32 = (filePath, maxNextDays) =>
                {
                    var bookedAppointmentList = new List<BookingModel>();
                    var bookedAppointment = new BookingModel();
                    bookedAppointment.Name = "Mario";
                    bookedAppointment.Date = DateTime.Today;
                    bookedAppointment.StartTime = DateTime.Today.Add(new TimeSpan(0, 15, 20, 0));
                    bookedAppointment.EndTime = DateTime.Today.Add(new TimeSpan(0, 17, 00, 0));
                    bookedAppointment.Description = "Fix glass";
                    bookedAppointmentList.Add(bookedAppointment);
                    return bookedAppointmentList;
                };

                var result = _garage.ShowAvailableAppointments();
                var appoinentAvailableAspected = "\r\nMario is available:\r\n\r\nfrom 09.00 to 15.20\r\nfrom 17.00 to 17.30\r\n";
                Assert.AreEqual(true, result.Contains(appoinentAvailableAspected));
            }
        }
        [TestMethod]
        public void GarageFacroty_ShowAvailableAppointments_Return_Valid_Message_With_Two_Appoinments_Booked_Today()
        {

            PrivateObject maxDaysObj = new PrivateObject(_garage);
            maxDaysObj.SetField("maxNextDays", 1);
            using (ShimsContext.Create())
            {
                GarageBooker.Fakes.ShimCommon.ListAppointmentBookedPerDayStringInt32 = (filePath, maxNextDays) =>
                {
                    var bookedAppointmentList = new List<BookingModel>();
                    var bookedAppointment = new BookingModel();
                    bookedAppointment.Name = "Mario";
                    bookedAppointment.Date = DateTime.Today;
                    bookedAppointment.StartTime = DateTime.Today.Add(new TimeSpan(0, 15, 20, 0));
                    bookedAppointment.EndTime = DateTime.Today.Add(new TimeSpan(0, 17, 00, 0));
                    bookedAppointment.Description = "Fix glass";
                    bookedAppointmentList.Add(bookedAppointment);
                    var bookedAppointmentLuigi = new BookingModel();
                    bookedAppointmentLuigi.Name = "Luigi";
                    bookedAppointmentLuigi.Date = DateTime.Today;
                    bookedAppointmentLuigi.StartTime = DateTime.Today.Add(new TimeSpan(0, 15, 20, 0));
                    bookedAppointmentLuigi.EndTime = DateTime.Today.Add(new TimeSpan(0, 17, 00, 0));
                    bookedAppointmentLuigi.Description = "Fix engine";
                    bookedAppointmentList.Add(bookedAppointmentLuigi);
                    return bookedAppointmentList;
                };

                var result = _garage.ShowAvailableAppointments();
                var todayDate = DateTime.Today.ToString("dddd dd MMMM");
                var appoinentAvailableAspected = todayDate+"\r\n\r\n\r\nMario is available:\r\n\r\nfrom 09.00 to 15.20\r\nfrom 17.00 to 17.30";
                appoinentAvailableAspected +="\r\n\r\nLuigi is available:\r\n\r\nfrom 09.00 to 15.20\r\nfrom 17.00 to 17.30";
                appoinentAvailableAspected += "\r\n\r\nAll the other mechanics are avaialble in this day\r\n\r\n";
                Assert.AreEqual(true, result.Contains(appoinentAvailableAspected));
            }
        }
        [TestMethod]
        public void GarageFacroty_ShowAvailableAppointments_Return_Fail_Message_When_Missing_Name_In_Booked_Model()
        {
            PrivateObject maxDaysObj = new PrivateObject(_garage);
            maxDaysObj.SetField("maxNextDays", 1);
            using (ShimsContext.Create())
            {
                GarageBooker.Fakes.ShimCommon.ListAppointmentBookedPerDayStringInt32 = (filePath, maxNextDays) =>
                {
                    var bookedAppointmentList = new List<BookingModel>();
                    var bookedAppointment = new BookingModel();
                    bookedAppointment.Name = "";
                    bookedAppointment.Date = DateTime.Today;
                    bookedAppointment.StartTime = DateTime.Today.Add(new TimeSpan(0, 15, 20, 0));
                    bookedAppointment.EndTime = DateTime.Today.Add(new TimeSpan(0, 17, 00, 0));
                    bookedAppointment.Description = "Fix glass";
                    bookedAppointmentList.Add(bookedAppointment);
                    return bookedAppointmentList;
                };

                var result = _garage.ShowAvailableAppointments();
                var todayDate = DateTime.Today.ToString("dddd dd MMMM");
                var appoinentAvailableAspected = todayDate + "\r\n\r\nThere is an error in this file. Please review it and fix the issue";
                Assert.AreEqual(true, result.Contains(appoinentAvailableAspected));
            }
        }
        [TestMethod]
        public void Common_GetModelFromString_Return_Null_Model_When_Input_Is_Empty()
        {
            var newAppointmentModel = Common.GetModelFromString("");
            Assert.IsNull(newAppointmentModel);
        }


        [TestMethod]
        public void Common_GetModelFromString_Return_Null_Model_When_Input_Is_Empty_The_Starting_Time()
        {
            var rowMissingStartTime = "Name:mario,Date:20-06-2018,Start Time:,End Time:14.00,Description:test";
            var newAppointmentModel = Common.GetModelFromString(rowMissingStartTime);
            Assert.IsNull(newAppointmentModel);
        }
        [TestMethod]
        public void Common_GetModelFromString_Return_Null_Model_When_Input_Is_Missing_The_Date()
        {
            var rowMissingStartTime = "Name:mario,Start Time:12.00,End Time:14.00,Description:test";
            var newAppointmentModel = Common.GetModelFromString(rowMissingStartTime);
            Assert.IsNull(newAppointmentModel);
        }

        [TestMethod]
        public void Common_GetModelFromString_Return_Valid_Model_When_Input_Are_Correct()
        {
            var rowMissingStartTime = "Name:mario,Date:20-06-2018,Start Time:12.00,End Time:14.00,Description:test";
            var newAppointmentModel = Common.GetModelFromString(rowMissingStartTime);
            Assert.IsNotNull(newAppointmentModel);
            Assert.IsInstanceOfType(newAppointmentModel, typeof(BookingModel));
        }

        [TestMethod]
        public void Common_ShowFreeTimeNextTenDays_Return_All_Mechanics_Available_Message_When_Model_Is_Empty()
        {
            var dicBookedAppointmentModel = new Dictionary<DateTime, List<BookingModel>>();
            var listBookedAppoinments=new List<BookingModel>();
            dicBookedAppointmentModel.Add(DateTime.Today,listBookedAppoinments);
            var appointmentMessage = Common.ShowFreeTimeNextTenDays(dicBookedAppointmentModel,"9.00","17.30");
            var todayDate = DateTime.Today.ToString("dddd dd MMMM");
            var resutlAspected = todayDate + "\r\n\r\nAll mechanics are avaialble in this day\r\n\r\n";
            Assert.AreEqual(resutlAspected, appointmentMessage);
            
        }

        [TestMethod]
        public void Common_ShowFreeTimeNextTenDays_Return_Message_When_There_Is_An_Appoinment_Booked()
        {
            var dicBookedAppointmentModel = new Dictionary<DateTime, List<BookingModel>>();
            var listBookedAppoinments = new List<BookingModel>();
            var bookedAppointment = new BookingModel();
            bookedAppointment.Name = "Mario";
            bookedAppointment.Date = DateTime.Today;
            bookedAppointment.StartTime = DateTime.Today.Add(new TimeSpan(0, 15, 20, 0));
            bookedAppointment.EndTime = DateTime.Today.Add(new TimeSpan(0, 17, 00, 0));
            bookedAppointment.Description = "Fix glass";
            listBookedAppoinments.Add(bookedAppointment);
            dicBookedAppointmentModel.Add(DateTime.Today, listBookedAppoinments);
            
            var appointmentMessage = Common.ShowFreeTimeNextTenDays(dicBookedAppointmentModel, "9.00", "17.30");
            var todayDate = DateTime.Today.ToString("dddd dd MMMM");
            var resutlAspected = todayDate+"\r\n\r\n\r\nMario is available:\r\n\r\nfrom 09.00 to 15.20\r\nfrom 17.00 to 17.30\r\n\r\nAll the other mechanics are avaialble in this day\r\n\r\n";
            Assert.AreEqual(resutlAspected, appointmentMessage);

        }
        [TestMethod]
        public void Common_ListAppointmentBookedPerDay_Return_Empty_List_From_File_When_It_is_Empty_Too()
        {
            using (ShimsContext.Create())
            {
                GarageBooker.Fakes.ShimHelper.GetRowsFromFileStringInt32 = (pathFile, maxNextDays) =>
                {
                    return new List<string>();
                };
                var listAppointments = Common.ListAppointmentBookedPerDay("testPath", 10);
                Assert.IsNotNull(listAppointments);
                Assert.IsInstanceOfType(listAppointments, typeof(List<BookingModel>));
                Assert.AreEqual(0, listAppointments.Count);
            }
        }

        [TestMethod]
        public void Common_ListAppointmentBookedPerDay_Return_List_From_File_With_One_Appointment_Booked_()
        {
            using (ShimsContext.Create())
            {
                GarageBooker.Fakes.ShimHelper.GetRowsFromFileStringInt32 = (pathFile, maxNextDays) =>
                {
                    var appointmentList = new List<string>();
                    var rowMissingStartTime = "Name:mario,Date:20-06-2018,Start Time:12.00,End Time:14.00,Description:test";
                    appointmentList.Add(rowMissingStartTime);
                    return appointmentList;
                };
                var listAppointments = Common.ListAppointmentBookedPerDay("testPath", 10);

                Assert.AreEqual(1, listAppointments.Count);
                Assert.AreEqual("mario", listAppointments[0].Name);

            }
        }
    }
}
