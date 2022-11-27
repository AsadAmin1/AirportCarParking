using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParkingReservation.Core;
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
    public class AvailabilityController : ControllerBase
    {
        #region Private Fields

        private readonly ILogger<AvailabilityController> _logger;
        private readonly IParkingService _parkingService;
        private readonly IMapper _mapper;
        private readonly string _messageFormat = "{0}-{1}";

        #endregion

        #region Constructors

        public AvailabilityController(ILogger<AvailabilityController> logger, IParkingService parkingService, IMapper autoMapper)
        {
            _logger = logger;
            _parkingService = parkingService;
            _mapper = autoMapper;
        }

        #endregion

        #region Public Methods

        [HttpGet]
        [ProducesResponseType(typeof(AvailabilityResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetAvailabilityAsync([FromQuery]DateRange dateRange)
        {
            using (_logger.BeginScope(_messageFormat, GetType().Name, MethodBase.GetCurrentMethod().Name))
            {
                var domainDateRange = _mapper.Map<CoreModels.DatePeriods.DateRange>(dateRange);
                try
                {
                    var availability = await _parkingService.GetAvailabilityAsync(domainDateRange);

                    var response = _mapper.Map<AvailabilityResponse>(availability);

                    return Ok(response);
                }
                catch (Exception ex)
                {
                    if (ex is ElapsedDateException || ex is InvalidDatesException || ex is NoAvailabilityException)
                    {
                        return BadRequest(new AvailabilityResponse { Error = ErrorDetails.New(ex.Message) });
                    }

                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }
        }

        #endregion
    }
}
