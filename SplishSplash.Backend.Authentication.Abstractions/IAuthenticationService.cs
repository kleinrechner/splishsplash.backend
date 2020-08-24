using System;
using System.Collections.Generic;
using System.Text;

namespace Kleinrechner.SplishSplash.Backend.Authentication.Abstractions
{
    public interface IAuthenticationService
    {
        List<LoginUser> GetLoginUsers();
    }
}
