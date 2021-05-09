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


    [Route("api/v1/geo-comments")]
    [ApiController]
    public class GeoMessageController : Controller
    {
        private readonly GeoDbContext _context;

        public GeoMessageController(GeoDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        /* Ska retunera de messages som finns när sidan laddas */
        public async Task<ActionResult<IEnumerable<GeoMessage>>> GetMessages()
        {
            return await _context.GeoMessages.ToListAsync();

        }
        // ("/v1/geo-comments/{id}")
        [HttpGet("{id}")]
        public async Task<ActionResult<GeoMessage>> GetGeoMessage(int id)
        {
            var geoMessage = await _context.GeoMessages.FindAsync(id);

            if (geoMessage == null)
            {
                return NotFound();
            }

            return Ok(geoMessage);
        }

        // ("/v1/geo-comments")

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<GeoMessage>> PostGeoMessage(GeoMessage GeoMessage)
        {
            _context.GeoMessages.Add(GeoMessage);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGeoMessage", new { id = GeoMessage.Id }, GeoMessage);
        }
    }
}
