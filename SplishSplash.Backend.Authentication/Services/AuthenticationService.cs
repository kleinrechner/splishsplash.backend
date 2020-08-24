using System;
using System.Collections.Generic;
using System.Text;
using Kleinrechner.SplishSplash.Backend.Authentication.Abstractions;
using Microsoft.Extensions.Options;

namespace Kleinrechner.SplishSplash.Backend.Authentication.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        #region Fields

        private readonly IOptions<AuthenticationSettings> _authenticationSettings;

        #endregion

        #region Ctor
        public AuthenticationService(IOptions<AuthenticationSettings> authenticationSettings)
        {
            _authenticationSettings = authenticationSettings;
        }

        #endregion

        #region Methods

        public List<LoginUser> GetLoginUsers()
        {
            return _authenticationSettings.Value.Users;
        }

        #endregion
    }
}
