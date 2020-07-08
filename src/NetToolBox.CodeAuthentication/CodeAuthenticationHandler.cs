﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace NetToolBox.CodeAuthentication
{
    public sealed class CodeAuthenticationHandler : AuthenticationHandler<CodeAuthenticationSchemeOptions>
    {
        public CodeAuthenticationHandler(IOptionsMonitor<CodeAuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var code = Request.Query["code"];
            if (this.Options.ValidCodes.Contains(code))
            {
                var identity = new ClaimsIdentity(Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
            else
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid authentication code"));
            }
        }
    }
}
