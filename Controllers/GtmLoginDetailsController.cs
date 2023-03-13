using GoToMeetingApp.Handler;
using GoToMeetingApp.Logger;
using GoToMeetingApp.Models;
using GoToMeetingApp.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace GoToMeetingApp.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class GtmLoginDetailsController : ControllerBase
    {
        private ILoggerService _logger;
        private readonly IJWTManagerRepository _jWTManager;
        private readonly Gotomeeting_dbContext _DbContext;

        public GtmLoginDetailsController(IJWTManagerRepository jWTManager, Gotomeeting_dbContext dbContext, ILoggerService logger)
        {
            this._jWTManager = jWTManager;
            this._DbContext = dbContext;
            _logger = logger;
        }
        [HttpGet("GetLoginUsers")]
        public IActionResult GetLoginUsers()
        {
            try
            {
                List<GtmLoginDetails> Logins = _DbContext.GtmLoginDetails.ToList();
                if (Logins.Count() != 0)
                {
                    _logger.LogInfo("Login Details are listed");
                    return StatusCode(200, Logins.Where(status => status.IsActive == true).Count());

                }
                else
                {
                    _logger.LogInfo("No such Login User is available");
                    return StatusCode(404, "No Login data are available");
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarn("Contact To Admin..Server Error" + ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("authenticate")]
        public IActionResult Authenticate([FromBody] Users usersdata)
        {
            var token = _jWTManager.Authenticate(usersdata);

            if (token == null)
            {
                return Unauthorized();
            }

            return Ok(token);
        }
    }
}

