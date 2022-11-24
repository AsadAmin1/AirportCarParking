using ParkingReservation.Core.Exceptions;
using ParkingReservation.Core.Interfaces;
using ParkingReservation.Core.TestHelpers;

namespace ParkingReservation.Core.Tests
{
    public class ParkingManagementServiceTests
    {
        private IAvailabilityService _availabilityService;
        private IBookingService _bookingService;
        private readonly List<IBookable> _bookableItems = TestBookableItems.Items;

        [SetUp]
        public void SetUp()
        {
            _availabilityService = new AvailabilityService();
            _bookingService = new BookingService(TestBookableItems.Items);
        }

        [Test]
        public async Task GetAvailability_WithNoBookings_Returns10Spaces()
        {
            var dateRange = TestBookingDates.Jan1To9_1300_1300;

            var sut = new ParkingService(_availabilityService, _bookingService);
            var actual = await sut.GetAvailabilityAsync(dateRange);

            Assert.That(actual.Spaces, Is.EqualTo(_bookableItems.Count));
        }

        [Test]
        public void GetAvailability_WithElapsedDate_ThrowsElapsedDateException()
        {
            var expected = "Start Date can not be in the past.";
            
            var dateRange = TestBookingDates.ElapsedDate;
            var sut = new ParkingService(_availabilityService, _bookingService);
            var ex = Assert.ThrowsAsync<ElapsedDateException>(async () =>
            {
                await sut.GetAvailabilityAsync(dateRange);
            });

            Assert.That(ex.Message, Is.EqualTo(expected));
        }

        [Test]
        public void GetAvailability_WithInvalidDates_ThrowsInvalidDatesException()
        {
            var expected = "Start Date must be after than the End Date.";

            var dateRange = TestBookingDates.InvalidDates;
            var sut = new ParkingService(_availabilityService, _bookingService);
            var ex = Assert.ThrowsAsync<InvalidDatesException>(async () =>
            {
                await sut.GetAvailabilityAsync(dateRange);
            });

            Assert.That(ex.Message, Is.EqualTo(expected));
        }

        [Test]
        public async Task GetAvailability_WithExistingMatchingBooking_Returns9Spaces()
        {
            var expected = _bookableItems.Count - 1;

            var dateRange = TestBookingDates.Jan1To9_1300_1300;
            var sut = new ParkingService(_availabilityService, _bookingService);
            await sut.AddReservationAsync(dateRange);

            var actual = await sut.GetAvailabilityAsync(dateRange);

            Assert.That(actual.Spaces, Is.EqualTo(expected));
        }

        [Test]
        public async Task GetAvailability_WithExistingNotMatchingBooking_Returns10Spaces()
        {
            var expected = _bookableItems.Count;

            var firstWeek = TestBookingDates.Jan1To9_1300_1300;
            var sut = new ParkingService(_availabilityService, _bookingService);
            await sut.AddReservationAsync(firstWeek);

            var secondWeek = TestBookingDates.Jan10To19_1300_1300;
            var actual = await sut.GetAvailabilityAsync(secondWeek);

            Assert.That(expected, Is.EqualTo(actual.Spaces));
        }
    }
}