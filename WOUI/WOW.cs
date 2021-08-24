using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace WOUI
{
    class WOW // Wrapper:Offline Wrapper
    {
        private static ProcessStartInfo httpServerSI = new ProcessStartInfo
        {
            WorkingDirectory = @"wrapper-offline\server",
            // Which("http-server") doesn't work on my system for some reason. probably some choco/npm env stuff
            FileName = Util.Which("npx"),
            Arguments = "http-server -p 4664 -c-1 -S -C the.crt -K the.key -a localhost"
        };
        private static ProcessStartInfo wrapperSI = new ProcessStartInfo
        {
            WorkingDirectory = @"wrapper-offline\wrapper",
            FileName = Util.Which("npm"),
            Arguments = "start"
        };

        private Process wrapper;
        private Process httpServer;

        public bool running { get; private set; }

        public WOW()
        {
            running = true;

            wrapper = new Process { StartInfo = wrapperSI };
            httpServer = new Process { StartInfo = httpServerSI };

            wrapper.Exited += Process_Exited;
            httpServer.Exited += Process_Exited;

            wrapper.Start();
            httpServer.Start();
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            Process s = (Process)sender;

            Console.WriteLine(
                $"{s.StartInfo.FileName} exited with code {s.ExitCode} without being explicitly closed!\n" +
                 "Check the logs in the main window!");
        }

        public void Stop()
        {
            running = false;
            StopProc(wrapper);
            StopProc(httpServer);
        }

        private void StopProc(Process p)
        {
            if (p != null)
            {
                p.Exited -= Process_Exited;
                while (!p.HasExited) p.Kill(true);
            }
        }

    }
}
