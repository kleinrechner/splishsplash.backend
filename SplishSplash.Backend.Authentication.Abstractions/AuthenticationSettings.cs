using System;
using System.Collections.Generic;
using System.Text;

namespace Kleinrechner.SplishSplash.Backend.Authentication.Abstractions
{
    public class AuthenticationSettings
    {
        public const string SectionName = "AuthenticationSettings";

        public List<LoginUser> Users { get; set; }
    }
}
