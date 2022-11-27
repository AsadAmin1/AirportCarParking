using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CoreModels = ParkingReservation.Core.Models;
using ParkingReservation.Core.Exceptions;
using ParkingReservation.Core.Interfaces;
using System.Threading.Tasks;
using ParkingReservation.Api.ApiModels;
using System.Reflection;
using AutoMapper;
using System.Net;

namespace ParkingReservation.Api.v1.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("/api/v{version:apiVersion}/[controller]")]
    public class BookingController : ControllerBase
    {
        #region Private Fields

        private readonly ILogger<BookingController> _logger;
        private readonly IParkingService _parkingService;
        private readonly IMapper _mapper;
        private readonly string _messageFormat = $"{0}-{1}";

        #endregion

        #region Constructors

        public BookingController(ILogger<BookingController> logger, IParkingService parkingService, IMapper mapper) {
            _logger = logger;
            _parkingService = parkingService;
            _mapper = mapper;
        }

        #endregion

        #region Public Methods

        [HttpPost]
        [ProducesResponseType(typeof(ReservationResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ReservationResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CreateReservationAsync([FromBody] DateRange dateRange)
        {
            using (_logger.BeginScope(_messageFormat, GetType().Name, MethodBase.GetCurrentMethod().Name))
            {
                var domainDateRange = _mapper.Map<CoreModels.DatePeriods.DateRange>(dateRange);

                try
                {
                    var reservationResult = await _parkingService.AddReservationAsync(domainDateRange);
                    if (reservationResult != null)
                    {
                        var reservation = _mapper.Map<ReservationResponse>(reservationResult);
                        return new CreatedResult($"parkingreservation/bookings/{reservation.Reference}", reservation);
                    }

                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
                catch (Exception ex)
                {
                    if (ex is NoAvailabilityException)
                    {
                        var response = new ReservationResponse { Error = ErrorDetails.New(ex.Message) };
                        return new NotFoundObjectResult(response);
                    }

                    return new StatusCodeResult(StatusCodes.Status424FailedDependency);
                }
            }
        }

        [HttpDelete("{bookingReference}")]
        [ProducesResponseType(typeof(ReservationResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ReservationResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CancelReservationAsync(string bookingReference)
        {
            using (_logger.BeginScope(_messageFormat, GetType().Name, MethodBase.GetCurrentMethod().Name))
            {
                try
                {
                    await _parkingService.CancelReservationAsync(bookingReference);
                    return Ok();
                }
                catch (Exception ex)
                {
                    if (ex is BookingNotFoundException)
                    {
                        var response = new ReservationResponse { Error = ErrorDetails.New(ex.Message) };
                        return new NotFoundObjectResult(response);
                    }

                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }
        }

        [HttpPatch]
        [ProducesResponseType(typeof(ReservationResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ReservationResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> AmendReservationAsync([FromBody] AmendReservationRequest amendReservationRequest)
        {
            using (_logger.BeginScope(_messageFormat, GetType().Name, MethodBase.GetCurrentMethod().Name))
            {
                try
                {
                    var request = _mapper.Map<CoreModels.AmendReservation>(amendReservationRequest);
                    var reservation = await _parkingService.AmendReservationAsync(request);
                    var response = _mapper.Map<ReservationResponse>(reservation);

                    return Ok(response);
                }
                catch (Exception ex)
                {
                    if (ex is NoAvailabilityException) {
                        var response = new ReservationResponse { Error = ErrorDetails.New(ex.Message) };
                        return new NotFoundObjectResult(response);
                    }

                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }
        }

        #endregion
    }
}
