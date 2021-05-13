using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_WebAPI_Template.Models
{
    public class GeoMessageTwo
    {

        //Spara till db
        public class SecondaryGeoMessage
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Author { get; set; }
            public string Body { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }
        }

        //publicera på APi
        public class SecondaryGeoMessageDto
        {
            public Message Message { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }
        }

        public class Message
        {
            public string Body { get; set; }
            public string Title { get; set; }
            public string Author { get; set; }
        }

        public class SecondaryGeoMessagePost
        {
            public string Title { get; set; }
            public string Body { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }
        }
    }
}
