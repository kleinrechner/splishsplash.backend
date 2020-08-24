using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Kleinrechner.SplishSplash.Backend.Authentication.Abstractions;
using Kleinrechner.SplishSplash.Backend.Authentication.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using IAuthenticationService = Kleinrechner.SplishSplash.Backend.Authentication.Abstractions.IAuthenticationService;

namespace Kleinrechner.SplishSplash.Backend.Authentication
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        #region Fields

        private readonly IAuthenticationService _authenticationService;

        #endregion

        #region Ctor

        public BasicAuthenticationHandler(
            IAuthenticationService authenticationService,
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            _authenticationService = authenticationService;
        }

        #endregion

        #region Methods

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Missing Authorization Header");

            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
                var username = credentials[0];
                var password = credentials[1].GetMD5Hash();

                var loginUser = _authenticationService.GetLoginUsers().FirstOrDefault(x =>
                    x.LoginName.ToLower() == username.ToLower() && x.PasswordMD5Hash == password);

                if (loginUser != null)
                {
                    var claims = new[] {
                        new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                        new Claim(ClaimTypes.Name, loginUser.LoginName.ToLower()),
                        new Claim(ClaimTypes.Role, loginUser.Role)
                    };
                    var identity = new ClaimsIdentity(claims, Scheme.Name);
                    var principal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);

                    return AuthenticateResult.Success(ticket);
                }
                else
                {
                    return AuthenticateResult.Fail("Invalid Username or Password");
                }
            }
            catch
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }
        }

        #endregion
    }
}
