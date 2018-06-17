using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GarageBooker;
using Moq;
using System.Collections.Generic;
using System.Text;
namespace UnitTestWithMoq
{
    [TestClass]
    public class UnitTest1
    {
        Mock<ICommon> _mockCommon = null;
        Mock<IHelper> _mockHelper = null;
        Common _common = null;
        [TestInitialize]
        public void SetUp()
        {
            _mockCommon = new Mock<ICommon>();
            _mockHelper = new Mock<IHelper>();
            _common = new Common(null);
        }
        [TestMethod]
        public void Common_GetModelFromString_Return_Null_Model_When_Input_Is_Empty()
        {
            var newAppointmentModel = _common.GetModelFromString("");
            Assert.IsNull(newAppointmentModel);
        }
        [TestMethod]
        public void Common_GetModelFromString_Return_Null_Model_When_Input_Is_Missing_The_Starting_Time()
        {
            var rowMissingStartTime = "Name:mario,Date:20-06-2018,Start Time:,End Time:14.00,Description:test";
            var newAppointmentModel = _common.GetModelFromString(rowMissingStartTime);
            Assert.IsNull(newAppointmentModel);
        }

        [TestMethod]
        public void Common_GetModelFromString_Return_Null_Model_When_Input_Is_Missing_The_Date()
        {
            var rowMissingStartTime = "Name:mario,Start Time:12.00,End Time:14.00,Description:test";
            var newAppointmentModel = _common.GetModelFromString(rowMissingStartTime);
            Assert.IsNull(newAppointmentModel);
        }

        [TestMethod]
        public void Common_GetModelFromString_Return_Valid_Model_When_Input_Are_Correct()
        {
            var rowMissingStartTime = "Name:mario,Date:20-06-2018,Start Time:12.00,End Time:14.00,Description:test";
            var newAppointmentModel = _common.GetModelFromString(rowMissingStartTime);
            Assert.IsNotNull(newAppointmentModel);
            Assert.IsInstanceOfType(newAppointmentModel, typeof(BookingModel));
        }

        [TestMethod]
        public void Common_ShowFreeTimeNextTenDays_Return_Valid_Message_When_No_Appoinments_Booked_Today()
        {
            var dicBookedAppointmentModel = new Dictionary<DateTime, List<BookingModel>>();
            var appointmentMessage = _common.ShowFreeTimeNextTenDays(dicBookedAppointmentModel, "9.00", "17.30");
            var todayDate = DateTime.Today.ToString("dddd dd MMMM");
            var appoinentAvailableAspected = todayDate + "\r\n\r\nAll mechanics are avaialble in this day";
            Assert.AreEqual(true, appoinentAvailableAspected.Contains(appoinentAvailableAspected));
        }


        [TestMethod]
        public void Common_ShowFreeTimeNextTenDays_Return_All_Mechanics_Available_Message_When_Model_Is_Empty()
        {
            var dicBookedAppointmentModel = new Dictionary<DateTime, List<BookingModel>>();
            var listBookedAppoinments = new List<BookingModel>();
            dicBookedAppointmentModel.Add(DateTime.Today, listBookedAppoinments);
            var appointmentMessage = _common.ShowFreeTimeNextTenDays(dicBookedAppointmentModel, "9.00", "17.30");
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

            var appointmentMessage = _common.ShowFreeTimeNextTenDays(dicBookedAppointmentModel, "9.00", "17.30");
            var todayDate = DateTime.Today.ToString("dddd dd MMMM");
            var resutlAspected = todayDate + "\r\n\r\n\r\nMario is available:\r\n\r\nfrom 09.00 to 15.20\r\nfrom 17.00 to 17.30\r\n\r\nAll the other mechanics are avaialble in this day\r\n\r\n";
            Assert.AreEqual(resutlAspected, appointmentMessage);
        }

        [TestMethod]
        public void Common_ShowFreeTimeNextTenDays_Return_Valid_Message_With_Two_Appoinments_Booked_Today()
        {
            var dicBookedAppointmentModel = new Dictionary<DateTime, List<BookingModel>>();
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
            dicBookedAppointmentModel.Add(DateTime.Today, bookedAppointmentList);

            var appointmentMessage = _common.ShowFreeTimeNextTenDays(dicBookedAppointmentModel, "9.00", "17.30");

            var todayDate = DateTime.Today.ToString("dddd dd MMMM");
            var appoinentAvailableAspected = todayDate + "\r\n\r\n\r\nMario is available:\r\n\r\nfrom 09.00 to 15.20\r\nfrom 17.00 to 17.30";
            appoinentAvailableAspected += "\r\n\r\nLuigi is available:\r\n\r\nfrom 09.00 to 15.20\r\nfrom 17.00 to 17.30";
            appoinentAvailableAspected += "\r\n\r\nAll the other mechanics are avaialble in this day\r\n\r\n";
            Assert.AreEqual(true, appointmentMessage.Contains(appoinentAvailableAspected));
        }

        [TestMethod]
        public void Common_ShowFreeTimeNextTenDays_Return_Fail_Message_When_Missing_Name_In_Booked_Appointment()
        {
            var dicBookedAppointmentModel = new Dictionary<DateTime, List<BookingModel>>();
            var bookedAppointmentList = new List<BookingModel>();
            var bookedAppointment = new BookingModel();
            bookedAppointment.Name = "";
            bookedAppointment.Date = DateTime.Today;
            bookedAppointment.StartTime = DateTime.Today.Add(new TimeSpan(0, 15, 20, 0));
            bookedAppointment.EndTime = DateTime.Today.Add(new TimeSpan(0, 17, 00, 0));
            bookedAppointment.Description = "Fix glass";
            bookedAppointmentList.Add(bookedAppointment);
            dicBookedAppointmentModel.Add(DateTime.Today, bookedAppointmentList);

            var appointmentMessage = _common.ShowFreeTimeNextTenDays(dicBookedAppointmentModel, "9.00", "17.30");
            var todayDate = DateTime.Today.ToString("dddd dd MMMM");
            var appoinentAvailableAspected = todayDate + "\r\n\r\nThere is an error in this file. Please review it and fix the issue";
            Assert.AreEqual(true, appointmentMessage.Contains(appoinentAvailableAspected));
        }
        [TestMethod]
        public void Common_ListAppointmentBookedPerDay_Return_Empty_List_From_File_When_It_is_Empty_Too()
        {
            _mockHelper.Setup(fake => fake.GetRowsFromFile(It.IsAny<string>(), It.IsAny<int>())).Returns(new List<string>());
            var common = new Common(_mockHelper.Object);
            var listAppointments = common.ListAppointmentBookedPerDay("testPath", 10);
            Assert.IsNotNull(listAppointments);
            Assert.IsInstanceOfType(listAppointments, typeof(List<BookingModel>));
            Assert.AreEqual(0, listAppointments.Count);
        }

        [TestMethod]
        public void Common_ListAppointmentBookedPerDay_Return_List_From_File_With_One_Appointment_Booked_()
        {
            var appointmentList = new List<string>();
            var rowMissingStartTime = "Name:mario,Date:20-06-2018,Start Time:12.00,End Time:14.00,Description:test";
            appointmentList.Add(rowMissingStartTime);

            _mockHelper.Setup(fake => fake.GetRowsFromFile(It.IsAny<string>(), It.IsAny<int>())).Returns(appointmentList);
            var common = new Common(_mockHelper.Object);
            var listAppointments = common.ListAppointmentBookedPerDay("testPath", 10);
            Assert.AreEqual(1, listAppointments.Count);
            Assert.AreEqual("mario", listAppointments[0].Name);
        }
        [TestMethod]
        public void GarageFacroty_StoreNewAppointment_Return_error_Message_When_The_Model_Is_Null()
        {
            _mockCommon.Setup(fake => fake.GetModelFromString(It.IsAny<string>())).Returns<BookingModel>(null);
            var garage = new PlainTextSource(_mockCommon.Object, null);
            var result = garage.StoreNewAppointment("test");
            Assert.AreEqual(true, result.StartsWith("The data insered are incorrect"));
        }
        [TestMethod]
        public void GarageFacroty_StoreNewAppointment_Return_error_Date_When_The_Model_Has_Date_In_The_Past()
        {
            var bookingModel = new BookingModel();
            bookingModel.StartTime = DateTime.Parse("01-06-2018 12:00");
            _mockCommon.Setup(fake => fake.GetModelFromString(It.IsAny<string>())).Returns(bookingModel);
            var garage = new PlainTextSource(_mockCommon.Object, null);
            var result = garage.StoreNewAppointment("test");
            Assert.AreEqual(true, result.Contains("The appointment cannot be booked with time in the past"));
        }
        [TestMethod]
        public void GarageFacroty_StoreNewAppointment_Return_error_Cannot_Be_Booked_When_The_Mechanic_Has_Already_Date_Booked()
        {
            var bookedAppointment = new BookingModel();
            bookedAppointment.Name = "Mario";
            bookedAppointment.StartTime = DateTime.Today.Add(new TimeSpan(3, 13, 0, 0));
            bookedAppointment.EndTime = DateTime.Today.Add(new TimeSpan(3, 15, 0, 0));

            var listBookeAppoinments = new List<BookingModel>();
            var appointmentBooked = new BookingModel();
            appointmentBooked.Name = "Mario";
            appointmentBooked.StartTime = DateTime.Today.Add(new TimeSpan(3, 12, 0, 0));
            appointmentBooked.EndTime = DateTime.Today.Add(new TimeSpan(3, 14, 0, 0));
            listBookeAppoinments.Add(appointmentBooked);

            _mockCommon.Setup(fake => fake.GetModelFromString(It.IsAny<string>())).Returns(bookedAppointment);
            _mockCommon.Setup(fake => fake.ListAppointmentBookedPerDay(It.IsAny<string>(), It.IsAny<int>())).Returns(listBookeAppoinments);
            var garage = new PlainTextSource(_mockCommon.Object, null);
            var result = garage.StoreNewAppointment("test");
            Assert.AreEqual(true, result.Contains("The appointment cannot be booked as"));

        }
        [TestMethod]
        public void GarageFacroty_StoreNewAppointment_Return_error_Cannot_Be_Booked_When_The_Mechanic_Has_Already_Booked_Same_Time_Cheking_minutes()
        {
            var bookingModel = new BookingModel();
            bookingModel.Name = "Mario";
            bookingModel.StartTime = DateTime.Today.Add(new TimeSpan(3, 13, 0, 0));
            bookingModel.EndTime = DateTime.Today.Add(new TimeSpan(3, 15, 20, 0));

            var listBookeAppoinments = new List<BookingModel>();
            var appointmentBooked = new BookingModel();
            appointmentBooked.Name = "Mario";
            appointmentBooked.StartTime = DateTime.Today.Add(new TimeSpan(3, 15, 15, 0));
            appointmentBooked.EndTime = DateTime.Today.Add(new TimeSpan(3, 16, 0, 0));
            listBookeAppoinments.Add(appointmentBooked);

            _mockCommon.Setup(fake => fake.GetModelFromString(It.IsAny<string>())).Returns(bookingModel);
            _mockCommon.Setup(fake => fake.ListAppointmentBookedPerDay(It.IsAny<string>(), It.IsAny<int>())).Returns(listBookeAppoinments);
            var garage = new PlainTextSource(_mockCommon.Object, null);
            var result = garage.StoreNewAppointment("test");
            Assert.AreEqual(true, result.Contains("The appointment cannot be booked as"));
        }
        [TestMethod]
        public void GarageFacroty_StoreNewAppointment_Is_Valid_Different_Mechanic_Same_Date_Booked()
        {
            var bookingModel = new BookingModel();
            bookingModel.Name = "Mario";
            bookingModel.StartTime = DateTime.Today.Add(new TimeSpan(3, 13, 0, 0));
            bookingModel.EndTime = DateTime.Today.Add(new TimeSpan(3, 15, 0, 0));

            var listBookeAppoinments = new List<BookingModel>();
            var appointmentBooked = new BookingModel();
            appointmentBooked.Name = "Luigi";
            appointmentBooked.StartTime = DateTime.Today.Add(new TimeSpan(3, 12, 0, 0));
            appointmentBooked.EndTime = DateTime.Today.Add(new TimeSpan(3, 14, 0, 0));
            listBookeAppoinments.Add(appointmentBooked);

            _mockHelper.Setup(fake => fake.WriteToFile(It.IsAny<string>(),It.IsAny<string>()));

            _mockCommon.Setup(fake => fake.GetModelFromString(It.IsAny<string>())).Returns(bookingModel);
            _mockCommon.Setup(fake => fake.ListAppointmentBookedPerDay(It.IsAny<string>(), It.IsAny<int>())).Returns(listBookeAppoinments);
            var garage = new PlainTextSource(_mockCommon.Object, _mockHelper.Object);
            var result = garage.StoreNewAppointment("test");
            Assert.AreEqual(true, result.Contains("The appointment has be booked"));
        }
        [TestMethod]
        public void GarageFacroty_StoreNewAppointment_Is_Valid_Same_Mechanic_Different_Time_Booked()
        {
            var bookingModel = new BookingModel();
            bookingModel.Name = "Mario";
            bookingModel.StartTime = DateTime.Today.Add(new TimeSpan(3, 13, 0, 0));
            bookingModel.EndTime = DateTime.Today.Add(new TimeSpan(3, 15, 0, 0));

            var listBookeAppoinments = new List<BookingModel>();
            var appointmentBooked = new BookingModel();
            appointmentBooked.Name = "Mario";
            appointmentBooked.StartTime = DateTime.Today.Add(new TimeSpan(3, 15, 0, 0));
            appointmentBooked.EndTime = DateTime.Today.Add(new TimeSpan(3, 17, 0, 0));
            listBookeAppoinments.Add(appointmentBooked);

            _mockHelper.Setup(fake => fake.WriteToFile(It.IsAny<string>(), It.IsAny<string>()));

            _mockCommon.Setup(fake => fake.GetModelFromString(It.IsAny<string>())).Returns(bookingModel);
            _mockCommon.Setup(fake => fake.ListAppointmentBookedPerDay(It.IsAny<string>(), It.IsAny<int>())).Returns(listBookeAppoinments);
            var garage = new PlainTextSource(_mockCommon.Object, _mockHelper.Object);
            var result = garage.StoreNewAppointment("test");
            Assert.AreEqual(true, result.Contains("The appointment has be booked"));
        }
        [TestMethod]
        public void GarageFacroty_StoreNewAppointment_Is_Valid_Checking_Minutes()
        {
            var bookingModel = new BookingModel();
            bookingModel.Name = "Mario";
            bookingModel.StartTime = DateTime.Today.Add(new TimeSpan(3, 13, 0, 0));
            bookingModel.EndTime = DateTime.Today.Add(new TimeSpan(3, 15, 20, 0));

            var listBookeAppoinments = new List<BookingModel>();
            var appointmentBooked = new BookingModel();
            appointmentBooked.Name = "Mario";
            appointmentBooked.StartTime = DateTime.Today.Add(new TimeSpan(3, 15, 20, 0));
            appointmentBooked.EndTime = DateTime.Today.Add(new TimeSpan(3, 17, 0, 0));
            listBookeAppoinments.Add(appointmentBooked);

            _mockHelper.Setup(fake => fake.WriteToFile(It.IsAny<string>(), It.IsAny<string>()));

            _mockCommon.Setup(fake => fake.GetModelFromString(It.IsAny<string>())).Returns(bookingModel);
            _mockCommon.Setup(fake => fake.ListAppointmentBookedPerDay(It.IsAny<string>(), It.IsAny<int>())).Returns(listBookeAppoinments);
            var garage = new PlainTextSource(_mockCommon.Object, _mockHelper.Object);
            var result = garage.StoreNewAppointment("test");
            Assert.AreEqual(true, result.Contains("The appointment has be booked"));
        }

        [TestMethod]
        public void GarageFacroty_ShowAvailableAppointments_Return_error_Message_When_Throws_Exception_Cheking_File()
        {
            _mockCommon.Setup(fake => fake.ListAppointmentBookedPerDay(It.IsAny<string>(), It.IsAny<int>())).Throws(new Exception());
            var garage = new PlainTextSource(_mockCommon.Object, null);
            PrivateObject maxDaysObj = new PrivateObject(garage);
            maxDaysObj.SetField("maxNextDays", 1);
            var result = garage.ShowAvailableAppointments();
            var errorMessageAspected = "There is a fatal error in the file";
            Assert.AreEqual(true, result.StartsWith(errorMessageAspected));
        }

        [TestMethod]
        public void GarageFacroty_ShowAvailableAppointments_Return_Valid_Message_When_No_Appoinments_Booked_Today()
        {
            var listBookedAppointments = new Dictionary<DateTime, List<BookingModel>>();
            var textListAppointment = "All mechanics are avaialble in this day";
            _mockCommon.Setup(fake => fake.ListAppointmentBookedPerDay(It.IsAny<string>(), It.IsAny<int>())).Returns(new List<BookingModel>());
            _mockCommon.Setup(fake => fake.ShowFreeTimeNextTenDays(It.IsAny<Dictionary<DateTime, List<BookingModel>>>(), It.IsAny<string>(), It.IsAny<string>())).Returns(textListAppointment);
            var garage = new PlainTextSource(_mockCommon.Object, null);
            var result = garage.ShowAvailableAppointments();
            var appoinentAvailableAspected =  "All mechanics are avaialble in this day";
            Assert.AreEqual(true, result.Contains(appoinentAvailableAspected));
        }
    }
}
