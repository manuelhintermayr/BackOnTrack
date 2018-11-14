using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using BackOnTrack.Annotations;

namespace BackOnTrack.Services.ProgramConfiguration
{
    public class CurrentProgramConfiguration : ICloneable
    {
        public bool ProxyEnabled { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

    }
}
