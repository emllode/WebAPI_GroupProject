using ASP_WebAPI_Template.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace ASP_WebAPI_Template.Data
{
    public class MyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly GeoDbContext _context;
        public static MyUser User { get; set; }
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
            string[] UserAndPas = ValidateUser(Request);
            if (UserAndPas == null) return AuthenticateResult.Fail("No authentication");
            User = _context.MyUsers.Where(u => u.FirstName == UserAndPas[0] && u.LastName == UserAndPas[1]).FirstOrDefault();

            if (User == null) return AuthenticateResult.Fail("Invalid user");

            var Identity = new ClaimsIdentity(Scheme.Name);
            var Principal = new ClaimsPrincipal(Identity);
            var Ticket = new AuthenticationTicket(Principal, Scheme.Name);
            return AuthenticateResult.Success(Ticket);
        }
        public static string[] ValidateUser(HttpRequest request)
        {
            string Auth = request.Headers["Authorization"];
            if (Auth == null || !Auth.StartsWith("Basic")) return null;
            Auth = Auth.Remove(0, 6);
            var encoding = Encoding.GetEncoding("iso-8859-1");
            Auth = encoding.GetString(Convert.FromBase64String(Auth));
            return Auth.Split(":", StringSplitOptions.None);
        }
    }

}
