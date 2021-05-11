using ASP_WebAPI_Template.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_WebAPI_Template.Controllers
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[Controller]")]

    public class GeoMessageSecondController : Controller
    {
        private readonly GeoDbContext _context;

        public GeoMessageSecondController(GeoDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        /* Ska retunera de messages som finns när sidan laddas */
        public async Task<ActionResult<IEnumerable<GeoMessageDto>>> GetMessages()
        {
            return await _context.GeoMessages.Select(g =>
            new GeoMessageDto
            {
                Message = g.Message,
                Longitude = g.Longitude,
                Latitude = g.Latitude
            }
            ).ToListAsync();

        }
        // ("/v1/geo-comments/{id}")
        [HttpGet("{id}")]
        public async Task<ActionResult<GeoMessageDto>> GetGeoMessage(int id)
        {
            var geoMessage = await _context.GeoMessages.Where(g => g.Id == id).Select(g =>
            new GeoMessageDto
            {
                Message = g.Message,
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

        // ("/v1/geo-comments")

        [Authorize]
        [HttpPost]
        [Consumes("application/json", new string[] { "application/xml" })]
        public async Task<ActionResult<GeoMessage>> PostGeoMessage(GeoMessageDto GeoMessage)
        {
            GeoMessage geomessage = new GeoMessage()
            {
                Latitude = GeoMessage.Latitude,
                Longitude = GeoMessage.Longitude,
                Message = GeoMessage.Message
            };
            _context.GeoMessages.Add(geomessage);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGeoMessage", new { Id = geomessage.Id }, GeoMessage);
        }


    }
}
