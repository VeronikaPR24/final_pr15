using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace final_pr15.Service
{
    public static class AuthService
    {
        public static bool IsAdmin { get; private set; }

        public static bool LoginAsAdmin(string pin)
        {
            if (pin == "1234")
            {
                IsAdmin = true;
                return true;
            }
            return false;
        }

        public static void LoginAsGuest()
        {
            IsAdmin = false;
        }
    }
}
