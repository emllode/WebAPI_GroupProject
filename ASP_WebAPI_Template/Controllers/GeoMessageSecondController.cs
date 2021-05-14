using ASP_WebAPI_Template.Data;
using ASP_WebAPI_Template.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        /// Returnerar alla meddelanden inom ett område.
        /// </summary>
        /// <param name="minLon"><para>Minimum värde för longitude</para></param>
        /// <param name="minLat"><para>Minimum värde för latitude</para></param>
        /// <param name="maxLon"><para>Maximum värdet för longitud</para></param>
        /// <param name="maxLat"><para>Maximum värdet för latitud</para></param>
        /// <returns>Retunerar samtliga messages</returns>

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GeoMessageTwo.SecondaryGeoMessageDto>>> GetMessagesTwo([FromQuery] double minLon, double maxLon, double minLat, double maxLat)
        {

            if (minLon != 0 && maxLon != 0 && minLat != 0 && maxLat != 0)
            {
                var geomessagesWithLatAndLon = await _context.GeoMessages.Where(g => 
                g.Longitude <= maxLon && 
                g.Longitude >= minLon && 
                g.Latitude <= maxLat && 
                g.Latitude >= minLat)
                    .Select(g =>
                new SecondaryGeoMessageDto
                {
                    Message = new Message
                    {
                        Author = g.Author,
                        Body = g.Body,
                        Title = g.Title
                    },
                    Latitude = g.Latitude,
                    Longitude = g.Longitude
                }).ToListAsync();
                if (geomessagesWithLatAndLon == null) return NotFound();
                return Ok(geomessagesWithLatAndLon);
            }

                var messageWithCords = await _context.GeoMessages.ToListAsync();
                return Ok(messageWithCords);


        }

        /// <summary>
        ///  Returnerar ett meddelande utifrån ID som anges.
        /// </summary>
        /// <param name="id"> </param>
        /// <returns>retunerar message från ID</returns>
        // ("/v1/geo-comments/{id}")
        [HttpGet("{id}")]
        public async Task<ActionResult<SecondaryGeoMessageDto>> GetGeoMessage(int id)
        {
            var geoMessage = await _context.GeoMessages.Where(g => g.Id == id).Select(g =>
            new SecondaryGeoMessageDto
            {
                Message = new Message { Title = g.Title, Author = g.Author, Body = g.Body },
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
        /// Postar ett meddelande
        /// </summary>
        /// <param name="GeoMessageTwo">
        /// <para>Här kan du skriva ett meddelande som vi sparar till vår databas</para>
        /// </param>
        /// <returns>
        /// Meddelandet har skapats
        /// </returns>
        [Authorize]
        [HttpPost]
        [Consumes("application/json", new string[] { "application/xml" })]
        public async Task<ActionResult<GeoMessage>> PostGeoMessage(SecondaryGeoMessagePost GeoMessage)
        {
            SecondaryGeoMessage geomessage = new SecondaryGeoMessage()
            {
                Latitude = GeoMessage.Latitude,
                Longitude = GeoMessage.Longitude,
                Title = GeoMessage.Title,
                Body = GeoMessage.Body,
                Author = MyAuthenticationHandler.User.FirstName + " " +  MyAuthenticationHandler.User.LastName
            };
            _context.GeoMessages.Add(geomessage);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGeoMessage", new { Id = geomessage.Id }, geomessage);
        }

    }
}
