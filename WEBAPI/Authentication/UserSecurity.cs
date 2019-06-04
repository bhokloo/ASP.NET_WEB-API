using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClassLibrary;

namespace WEBAPI.Authentication
{
    public class UserSecurity
    {
        public static bool Login(string username, string password)
        {
            using (Entities checkUser = new Entities())
            {
                return checkUser.Auths.Any(x => x.username.Equals(username, StringComparison.OrdinalIgnoreCase) &&
                x.password == password);
            }
        }
    }
}