using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOnTrack.Services.UserConfiguration
{
    public class UserConfigurationSupplier
    {
        public bool CheckPassword(string password)
        {
            return password == "admin";
        }

        public bool ConfigurationIsAlreadyCreated()
        {
            return true;
        }
    }
}
