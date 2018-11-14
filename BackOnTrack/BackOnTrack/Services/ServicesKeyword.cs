using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackOnTrack.Services.UserConfiguration;

namespace BackOnTrack.Services
{
    public class ServicesKeyword
    {
        public UserConfigurationSupplier UserConfiguration; 
        public ServicesKeyword()
        {
            UserConfiguration = new UserConfigurationSupplier();;
        }
    }
}
