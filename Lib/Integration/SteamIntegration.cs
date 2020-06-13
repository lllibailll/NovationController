using System.Diagnostics;

namespace Lib.Integration
{
    public static class SteamIntegration
    {
        public static void StartApp(long id)
        {
            var ps = new ProcessStartInfo(@$"steam://rungameid/{id}")
            { 
                UseShellExecute = true, 
                Verb = "open" 
            };
            Process.Start(ps);
        }
    }
}