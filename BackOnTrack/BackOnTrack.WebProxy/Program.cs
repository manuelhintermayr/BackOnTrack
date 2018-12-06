using System;
using System.Collections.Generic;

namespace BackOnTrack.WebProxy
{
    class Program
    {
        static void Main(string[] args)
        {
            var proxy = new LocalWebProxy();
            proxy.SetList(new List<string>(){"google.com"});
            proxy.StartProxy();
            Console.Read();
            proxy.QuitProxy();
        }

    }
}
