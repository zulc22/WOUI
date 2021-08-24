using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WOUI
{
    class Util
    {
        public static string Which(string cliName)
        {
            string path = Environment.GetEnvironmentVariable("path");
            char pathSeperator;
            string[] validExtensions;

            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32NT:
                    pathSeperator = ';';
                    validExtensions = new string[] { ".exe", ".com", ".bat", ".cmd" };
                    break;
                default:
                case PlatformID.Unix:
                    pathSeperator = ':';
                    validExtensions = new string[] { "" };
                    break;
            }

            foreach (string p in path.Split(pathSeperator))
                foreach (string e in validExtensions)
                    if (File.Exists(p + cliName + e)) return p + cliName + e;

            // return nothing if we didn't find something earlier
            return "";
        }
    }
}
