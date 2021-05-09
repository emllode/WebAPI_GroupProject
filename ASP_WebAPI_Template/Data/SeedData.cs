using ASP_WebAPI_Template.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_WebAPI_Template.Data
{
    public class SeedData
    {
        public static void Seeder(GeoDbContext context)
        {

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            /* Lägga till token till varje user */
            var SeedUsers = new List<MyUser>()
            {
                new MyUser { FirstName="Test", LastName="Test"},
                new MyUser { FirstName="Test2", LastName="Test2"}

            };

            context.MyUsers.AddRange(SeedUsers);
            context.SaveChanges();
        }
    }
}

