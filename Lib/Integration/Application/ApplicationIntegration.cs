using System.Diagnostics;

namespace Lib.Integration.Application
{
    public static class ApplicationIntegration
    {
        public static void StartApp(string path)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = path,
                UseShellExecute = true
            };
            Process.Start(startInfo);
        }

        public static void OpenWeb(string path)
        {
            var ps = new ProcessStartInfo(path)
            { 
                UseShellExecute = true, 
                Verb = "open" 
            };
            Process.Start(ps);
        }
    }
}