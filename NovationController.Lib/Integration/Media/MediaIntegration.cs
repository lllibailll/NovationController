using System;
using System.Runtime.InteropServices;
using NovationController.Lib.Click;

namespace NovationController.Lib.Integration.Media
{
    public class MediaIntegration : BaseIntegration
    {
        private const int KEYEVENTF_EXTENTEDKEY = 1;
        private const int VK_MEDIA_NEXT_TRACK = 0xB0;// code to jump to next track
        private const int VK_MEDIA_PLAY_PAUSE = 0xB3;// code to play or pause a song
        private const int VK_MEDIA_PREV_TRACK = 0xB1;// code to jump to prev track

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte virtualKey, byte scanCode, uint flags, IntPtr extraInfo);

        public MediaIntegration(Lib.NovationController novationController, string name, string actionPrefix) : base(novationController, name, actionPrefix)
        {
            
        }
        
        protected override void LoadConfig()
        {
            
        }

        private static void PlayPause()
        {
            keybd_event(VK_MEDIA_PLAY_PAUSE, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);
        }

        private static void Next()
        {
            keybd_event(VK_MEDIA_NEXT_TRACK, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);
        }

        private static void Prev()
        {
            keybd_event(VK_MEDIA_PREV_TRACK, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);
        }

        protected override void SetupClickAction(ClickableButton clickableButton, string[] data)
        {
            clickableButton.ClickCallbacks.Add(() =>
            {
                switch (data[1])
                {
                    case "Prev":
                    {
                        Prev();
                        break;
                    }
                    
                    case "Next":
                    {
                        Next();
                        break;
                    }
                    
                    case "PlayPause":
                    {
                        PlayPause();
                        break;
                    }
                }
            });
        }

        protected override void SetupLoadAction(ClickableButton clickableButton, string[] data)
        {
            
        }
        
        protected override void SetupColorControllerAction(ClickableButton clickableButton, string[] data)
        {
            
        }
    }
}