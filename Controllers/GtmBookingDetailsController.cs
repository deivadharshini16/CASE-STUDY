using GoToMeetingApp.Logger;
using GoToMeetingApp.Models;
using GoToMeetingApp.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GoToMeetingApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GtmBookingDetailsController : ControllerBase
    {
        private ILoggerService _logger;
        private readonly IJWTManagerRepository _jWTManager;
        private readonly Gotomeeting_dbContext _DbContext;
        public GtmBookingDetailsController(IJWTManagerRepository jWTManager, Gotomeeting_dbContext dbContext, ILoggerService logger)
        {
            this._jWTManager = jWTManager;
            this._DbContext = dbContext;
            _logger = logger;

        }
        [HttpGet]
        public IEnumerable<GtmBookingDetails> Get()
        {
            return _DbContext.GtmBookingDetails.ToList();
        }
        [HttpGet("{BookingId}")]
        public GtmBookingDetails Get(int BookingId)
        {
            return _DbContext.GtmBookingDetails.FirstOrDefault(o => o.BookingId == BookingId);
        }

        public ILoggerService Get_logger()
        {
            return _logger;
        }

        [HttpPost("AddBookingDetails")]
        public IActionResult AddGtmBookingDetails([FromBody] GtmBookingDetails gtmBookingDetails, ILoggerService _logger)
        {
            string result = string.Empty;
            try
            {
                var _bookingDetails = _DbContext.GtmBookingDetails.FirstOrDefault(o => o.BookingId == gtmBookingDetails.BookingId);
                if (_bookingDetails != null)
                {
                    _bookingDetails.BookingId = gtmBookingDetails.BookingId;
                    _bookingDetails.FirstName = gtmBookingDetails.FirstName;
                    _bookingDetails.LastName = gtmBookingDetails.LastName;
                    _bookingDetails.ConfirmationStatus = gtmBookingDetails.ConfirmationStatus;
                    _DbContext.SaveChanges();
                    _logger.LogInfo("BookingDetails updated Successfully.");
                    return Created("BookingDetails updated", gtmBookingDetails);

                }
                else
                {
                    GtmBookingDetails bookingDetails = new GtmBookingDetails()
                    {
                        BookingId = gtmBookingDetails.BookingId,
                        FirstName = gtmBookingDetails.FirstName,
                        LastName = gtmBookingDetails.LastName,
                        ConfirmationStatus = gtmBookingDetails.ConfirmationStatus,
                    };
                    _DbContext.GtmBookingDetails.Add(gtmBookingDetails);
                    _DbContext.SaveChanges();
                    _logger.LogInfo("Booking Details added Successfully.");
                    return Created("Booking Details added", gtmBookingDetails);
                }

            }
            catch (Exception ex)
            {
                _logger.LogWarn("Contact To Admin..Server Error" + ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("{BookingId}")]
        public IActionResult Delete(int BookingId)
        {
            GtmBookingDetails id = _DbContext.GtmBookingDetails.Find(BookingId);
            if (id == null)
            {
                return StatusCode(404, "BookingDetails Not Found");
            }
            else
            {
                _DbContext.GtmBookingDetails.Remove(id);
                _DbContext.SaveChanges();
                return Ok("Deleted Successfully");
            }

        }
    }
}




