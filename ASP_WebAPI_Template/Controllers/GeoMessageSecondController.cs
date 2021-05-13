﻿using ASP_WebAPI_Template.Data;
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
        /// Retunerar v1 + v2 meddelanden.
        /// </summary>
        /// <param name="minLon"><para>Minsta värdet för longitud, Decimaltal</para></param>
        /// <param name="minLat"><para>Minsta värdet för latitud, Decimaltal</para></param>
        /// <param name="maxLon"><para>Högsta värdet för longitud, Decimaltal</para></param>
        /// <param name="maxLat"><para>Högsta värdet för latitud, Decimaltal</para></param>
        /// <returns>Retunerar samtliga messages</returns>

        [HttpGet]
        /* Vi behöver lösa så v1 även syns här, Går det att tillkalla v1 databasen i Get? */
        public async Task<ActionResult<IEnumerable<GeoMessageTwo.SecondaryGeoMessageDto>>> GetMessagesTwo([FromQuery] double minLon, double maxLon, double minLat, double maxLat)
        {

            if (minLon == 0 && maxLon == 0 && minLat == 0 && maxLat == 0)
            {
                var msgv1 = await _context.GeoMessages.ToListAsync();
                var msgv2 = await _context.GeoMessagesTwo.ToListAsync();

                var v1AndV2 = Messagesv1(msgv1).Concat(Messagesv2(msgv2));
                return Ok(v1AndV2);
            }

            else
            {
                return Ok();

            }


        }

        /// <summary>
        ///  Ger ett message utifrån ID som anges.
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

            if (geoMessagev2 == null)
            {
                return NotFound();
            }

            return Ok(geoMessagev2);
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
        public async Task<ActionResult<GeoMessage>> PostGeoMessage(SecondaryGeoMessagePost GeoMessage)
        {
            MyAuthenticationHandler handler;
            SecondaryGeoMessage geomessage = new SecondaryGeoMessage()
            {
                Latitude = GeoMessage.Latitude,
                Longitude = GeoMessage.Longitude,
                Title = GeoMessage.Title,
                Body = GeoMessage.Body,
                Author = null
            };
            _context.GeoMessagesTwo.Add(geomess);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGeoMessage", new { Id = geomess.Id }, GeoMessageTwo);
        }


        /*  Tänker att vi använder dessa för kunna använda bägge versions  */
        private  IEnumerable<GeoMessageTwo.SecondaryGeoMessageDto> Messagesv1(IEnumerable<GeoMessage> messagesv1)
        {
            foreach (var message in messagesv1)
            {
                var messageDtov1 = new GeoMessageTwo.SecondaryGeoMessageDto
                {
                    Message = new GeoMessageTwo.Message { Title = "Existerar endast i v2 och uppåt", Body = "Existerar endast i v2 och uppåt", Author = "Existerar endast i v2 och uppåt" },
                    Longitude = message.Longitude,
                    Latitude = message.Latitude
                };
                yield return messageDtov1;
            }
        }
        private  IEnumerable<GeoMessageTwo.SecondaryGeoMessageDto> Messagesv2(IEnumerable<GeoMessageTwo.SecondaryGeoMessage> messagesv2)
        {
            foreach (var message in messagesv2)
            {
                var messageDtov2 = new GeoMessageTwo.SecondaryGeoMessageDto
                {
                    Message = new GeoMessageTwo.Message { Title = message.Title, Body = message.Body, Author = message.Author },
                    Longitude = message.Longitude,
                    Latitude = message.Latitude
                };
                yield return messageDtov2;
            }
        }

    }
}
