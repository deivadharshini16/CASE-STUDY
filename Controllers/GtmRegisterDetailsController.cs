using GoToMeetingApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace GoToMeetingApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GtmRegisterDetailsController : ControllerBase
    {
        private readonly Gotomeeting_dbContext _Context;
        public GtmRegisterDetailsController(Gotomeeting_dbContext Context)
        {
            _Context = Context;
        }
        [HttpGet("GetMember")]
        public List<GtmRegisterDetails> GetMember()
        {
            List<GtmRegisterDetails> members = _Context.GtmRegisterDetails.ToList();
            return members;
        }
        [HttpPost("Newuser")]
        public IActionResult Post([FromBody] GtmBookingDetails NewUser)
        {
            _Context.GtmBookingDetails.Add(NewUser);
            _Context.SaveChanges();
            return Created("Registering Details Added", NewUser);
        }
    }
}

        
