﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BackOnTrack.SharedResources.Infrastructure.Helpers
{
    public class TempFolder : IDisposable
    {
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Create a temp directory named after your test in the %temp%\uTest\xxx directory
        /// which is deleted and all sub directories when the ITempDir object is disposed.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static TempFolder Create()
        {
            var stack = new StackTrace(1);
            var sf = stack.GetFrame(0);
            return new TempFolder(sf.GetMethod().Name);
        }

        public TempFolder(string dirName)
        {
            if (String.IsNullOrEmpty(dirName))
            {
                throw new ArgumentException("dirName");
            }
            Name = Path.Combine(Path.GetTempPath(), "uTests", dirName);
            Directory.CreateDirectory(Name);
        }

        public void Dispose()
        {

            if (Name.Length < 10)
            {
                throw new InvalidOperationException(String.Format("Directory name seesm to be invalid. Do not delete recursively your hard disc.", Name));
            }

            // delete all files in temp directory
            foreach (var file in Directory.EnumerateFiles(Name, "*.*", SearchOption.AllDirectories))
            {
                File.Delete(file);
            }

            // and then the directory
            Directory.Delete(Name, true);
        }
    }
}
