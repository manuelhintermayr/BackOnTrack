﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOnTrack.WebProxy
{
    class Program
    {
        static void Main(string[] args)
        {
            var proxy = new LocalWebProxy();
            proxy.StartProxy();
            Console.Read();
            proxy.QuitProxy();
        }


    }
}
