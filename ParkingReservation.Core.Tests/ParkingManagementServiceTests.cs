using ParkingReservation.Core.Exceptions;
using ParkingReservation.Core.Interfaces;
using ParkingReservation.Core.TestHelpers;

namespace ParkingReservation.Core.Tests
{
    public class ParkingManagementServiceTests
    {
        private IAvailabilityService _availabilityService;
        private IBookingService _bookingService;
        private IPricingService _pricingService;
        private readonly List<IBookable> _bookableItems = TestBookableItems.Items;

        [SetUp]
        public void SetUp()
        {
            _availabilityService = new AvailabilityService();
            _bookingService = new BookingService(TestBookableItems.Items);
            _pricingService = new PricingService(TestPricingRules.PriceRules);
        }

        [Test]
        public async Task GetAvailability_WithNoBookingsWinter_Returns10Spaces()
        {
            var expectedPrice = 168m;

            var dateRange = TestBookingDates.WinterDates.FirstWeek1PMto1PM;

            var sut = new ParkingService(_availabilityService, _bookingService, _pricingService);
            var actual = await sut.GetAvailabilityAsync(dateRange);

            Assert.That(actual.Spaces, Is.EqualTo(_bookableItems.Count));
            Assert.That(actual.Price, Is.EqualTo(expectedPrice));
        }

        [Test]
        public async Task GetAvailability_WithNoBookingSummer_Returns10Spaces336()
        {
            var expectedPrice = 336m;

            var dateRange = TestBookingDates.SummerDates.FirstWeek1PMtoPM;

            var sut = new ParkingService(_availabilityService, _bookingService, _pricingService);
            var actual = await sut.GetAvailabilityAsync(dateRange);

            Assert.That(actual.Spaces, Is.EqualTo(_bookableItems.Count));
            Assert.That(actual.Price, Is.EqualTo(expectedPrice));
        }

        [Test]
        public void GetAvailability_WithElapsedDate_ThrowsElapsedDateException()
        {
            var expected = "Start Date can not be in the past.";
            
            var dateRange = TestBookingDates.ElapsedDate;
            var sut = new ParkingService(_availabilityService, _bookingService, _pricingService);
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
            var sut = new ParkingService(_availabilityService, _bookingService, _pricingService);
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

            var dateRange = TestBookingDates.WinterDates.FirstWeek1PMto1PM;
            var sut = new ParkingService(_availabilityService, _bookingService, _pricingService);
            await sut.AddReservationAsync(dateRange);

            var actual = await sut.GetAvailabilityAsync(dateRange);

            Assert.That(actual.Spaces, Is.EqualTo(expected));
        }

        [Test]
        public async Task GetAvailability_WithExistingNotMatchingBooking_Returns10Spaces()
        {
            var expected = _bookableItems.Count;

            var firstWeek = TestBookingDates.WinterDates.FirstWeek1PMto1PM;
            var sut = new ParkingService(_availabilityService, _bookingService, _pricingService);
            await sut.AddReservationAsync(firstWeek);

            var secondWeek = TestBookingDates.WinterDates.SecondWeek1PMto1PM;
            var actual = await sut.GetAvailabilityAsync(secondWeek);

            Assert.That(expected, Is.EqualTo(actual.Spaces));
        }
    }
}