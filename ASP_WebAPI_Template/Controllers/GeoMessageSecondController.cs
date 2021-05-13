using ASP_WebAPI_Template.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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

        /// <summary>
        /// Retunerar v1 + v2 meddelanden.
        /// </summary>
        /// <returns>Retunerar samtliga messages</returns>
        [HttpGet]
        /* Vi behöver lösa så v1 även syns här, Går det att tillkalla v1 databasen i Get? */
        public async Task<ActionResult<IEnumerable<GeoMessageTwo.SecondaryGeoMessageDto>>> GetMessagesTwo()
        {

            var messagesv2 = await _context.GeoMessagesTwo.Select(g =>
            new GeoMessageTwo.SecondaryGeoMessageDto
            {
                Message = new GeoMessageTwo.Message()
                {
                    Title = g.Title,
                    Body = g.Body,
                    Author = g.Author,
                },
                Longitude = g.Longitude,
                Latitude = g.Latitude
            }
             ).ToListAsync();

            return messagesv2;


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

        /// <summary>
        /// Postar message till v2 db
        /// </summary>
        /// <param name="GeoMessageTwo">
        /// <para>Här kan du skriva ett message som vi sparar till vår v2 databas</para>
        /// </param>
        /// <returns>
        /// Message har blivit postat till v2 db
        /// </returns>
        [Authorize]
        [HttpPost]
        [Consumes("application/json", new string[] { "application/xml" })]
        public async Task<ActionResult<GeoMessageTwo.SecondaryGeoMessage>> PostGeoMessagev2(GeoMessageTwo.SecondaryGeoMessage GeoMessageTwo)
        {

            GeoMessageTwo.SecondaryGeoMessage geomess = new GeoMessageTwo.SecondaryGeoMessage()
            {
                Title = GeoMessageTwo.Title,
                Body = GeoMessageTwo.Body,
                Author = GeoMessageTwo.Author,
                Longitude = GeoMessageTwo.Longitude,
                Latitude = GeoMessageTwo.Latitude
            };
            _context.GeoMessagesTwo.Add(geomess);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGeoMessage", new { Id = geomess.Id }, GeoMessageTwo);
        }


    }
}
