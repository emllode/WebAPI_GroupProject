using ASP_WebAPI_Template.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace ASP_WebAPI_Template.Data
{
    public class MyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly GeoDbContext _context;
        public MyAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            GeoDbContext context)
            : base(options, logger, encoder, clock)
        {
            _context = context;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string Name = Request.Query["Username"];
            string Password = Request.Query["Password"];

            MyUser User = _context.MyUsers.Where(u => u.FirstName == Name && u.LastName == Password).FirstOrDefault();

            if (User == null) return AuthenticateResult.Fail("Invalid user");

            var Identity = new ClaimsIdentity(Scheme.Name);
            var Principal = new ClaimsPrincipal(Identity);
            var Ticket = new AuthenticationTicket(Principal, Scheme.Name);
            return AuthenticateResult.Success(Ticket);
            

        }
    }
}
