using ASP_WebAPI_Template.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ASP_WebAPI_Template.Models.GeoMessageTwo;

namespace ASP_WebAPI_Template.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[Controller]")]
   
    public class GeoMessageController : ControllerBase
    {
        private readonly GeoDbContext _context;

        public GeoMessageController(GeoDbContext context)
        {
            _context = context;
        }

     /// <summary>
     /// Här får ni alla messages
     /// </summary>
     /// <returns>Retunerar samtliga messages</returns>
        [HttpGet]

        public async Task<ActionResult<IEnumerable<GeoMessageDto>>> GetMessages()
        {
            return await _context.GeoMessages.Select(g =>
            new GeoMessageDto
            {
                Message = g.Body,
                Longitude = g.Longitude,
                Latitude = g.Latitude
            }
            ).ToListAsync();

        }

        /// <summary>
        ///  Ger en message utifrån ID som anges.
        /// </summary>
        /// <param name="id"> </param>
        /// <returns>retunerar message från ID</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<GeoMessageDto>> GetGeoMessage(int id)
        {
            var geoMessage = await _context.GeoMessages.Where(g => g.Id == id).Select(g =>
            new GeoMessageDto
            {
                Message = g.Body,
                Longitude = g.Longitude,
                Latitude = g.Latitude
            }
            ).FirstOrDefaultAsync();

            if (geoMessage == null)
            {
                return NotFound();
            }

            return Ok(geoMessage);
        }

        /// <summary>
        /// Här postar man ett message till db
        /// </summary>
        /// <param name="GeoMessage">
        /// <para>Här kan du skriva ett message som vi sparar till vår databas</para>
        /// </param>
        /// <returns>Message har blivit postat till db</returns>
        [Authorize]
        [HttpPost]
        [Consumes("application/json", new string[] { "application/xml" })]
        public async Task<ActionResult<GeoMessage>> PostGeoMessage( GeoMessageDto GeoMessage)
        {
            SecondaryGeoMessage geomessage = new SecondaryGeoMessage()
            {
                Latitude = GeoMessage.Latitude,
                Longitude = GeoMessage.Longitude,
                Body = GeoMessage.Message
            };
            _context.GeoMessages.Add(geomessage);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGeoMessage", new {Id = geomessage.Id }, GeoMessage);
        }
    }
}
