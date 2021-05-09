using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ASP_WebAPI_Template.Models;

    public class GeoDbContext : DbContext
    {
        public GeoDbContext (DbContextOptions<GeoDbContext> options)
            : base(options)
        {
        }

         public DbSet<GeoMessage> GeoMessages { get; set; }
         public DbSet<MyUser> MyUsers { get; set; }

}
