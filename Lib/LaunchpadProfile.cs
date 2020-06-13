using System.Collections.Generic;
using Lib.Click;

namespace Lib
{
    public class LaunchpadProfile
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public LaunchpadCoord LaunchpadCoord { get; set; }
        
        private List<ClickableButton> _buttons = new List<ClickableButton>();

        public void AddButton(ClickableButton clickableButton)
        {
            _buttons.Add(clickableButton);
        }

        public List<ClickableButton> Buttons => _buttons;
    }
}