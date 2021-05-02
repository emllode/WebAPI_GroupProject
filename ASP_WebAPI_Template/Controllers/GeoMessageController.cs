using ASP_WebAPI_Template.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_WebAPI_Template.Controllers
{

    [Route("api/")]
    [ApiController]
    public class GeoMessageController : Controller
    {
        private readonly GeoDbContext _context;

        public GeoMessageController(GeoDbContext context)
        {
            _context = context;
        }


        [HttpGet("/v1/geo-comments/{id}")]
        /* Ska retunera de messages som finns när sidan laddas */
        public async Task<ActionResult<IEnumerable<GeoMessage>>> GetMessages()
        { 
            //  return await _contextGeoMessage.ToListAsync();

            return null;
        }


        [HttpGet("/v1/geo-comments")] 
        



        [HttpPost("/v1/geo-comments")]


        public IActionResult Index()
        {
            
            return View();
        }
    }
}
