﻿using BackOnTrack.WebProxy;

namespace BackOnTrack.Services.WebProxy
{
    public class RunningWebProxy
    {
        private LocalWebProxy webProxy;
        public RunningWebProxy()
        {
            webProxy = new LocalWebProxy();
        }

        public void Start()
        {
            webProxy.StartProxy();
        }

        public void Quit()
        {
            webProxy.QuitProxy();
        }

        public void Dispose()
        {
            webProxy.Dispose();
        }
    }
}
