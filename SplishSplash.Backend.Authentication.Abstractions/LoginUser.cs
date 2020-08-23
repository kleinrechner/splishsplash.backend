using System;
using System.Collections.Generic;
using System.Text;

namespace Kleinrechner.SplishSplash.Backend.Authentication.Abstractions
{
    public class LoginUser
    {
        #region Fields

        public string LoginName { get; set; }
        
        public string PasswordMD5Hash { get; set; }

        public string Role { get; set; }

        #endregion

        #region Ctor
        #endregion

        #region Methods
        #endregion
    }
}
