using System;
using System.Runtime.InteropServices;

namespace Lib.Integration.Media
{
    public static class MediaIntegration
    {
        private const int KEYEVENTF_EXTENTEDKEY = 1;
        private const int VK_MEDIA_NEXT_TRACK = 0xB0;// code to jump to next track
        private const int VK_MEDIA_PLAY_PAUSE = 0xB3;// code to play or pause a song
        private const int VK_MEDIA_PREV_TRACK = 0xB1;// code to jump to prev track
        
        [DllImport("user32.dll")]
        private static extern void keybd_event(byte virtualKey, byte scanCode, uint flags, IntPtr extraInfo);

        public static void PlayPause()
        {
            keybd_event(VK_MEDIA_PLAY_PAUSE, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);
        }

        public static void Next()
        {
            keybd_event(VK_MEDIA_NEXT_TRACK, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);
        }
        
        public static void Prev()
        {
            keybd_event(VK_MEDIA_PREV_TRACK, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);
        }
    }
}