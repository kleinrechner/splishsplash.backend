using System;
using System.Collections.Generic;
using System.Text;

namespace Kleinrechner.SplishSplash.Backend.Authentication.Models
{
    public class AuthenticationSettings
    {
        public const string Position = "AuthenticationSettings";

        public List<LoginUser> Users { get; set; }
    }
}
