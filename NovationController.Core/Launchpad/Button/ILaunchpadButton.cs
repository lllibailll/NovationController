using System.Drawing;

namespace NovationController.Core.Launchpad.Button
{
    public interface ILaunchpadButton
    {
        byte Channel { get; set; }
        Color Color { get; set; }
        byte Id { get; set; }
        bool IsPulsing { get; set; }
        LaunchpadButtonState State { get; set; }
        int X { get; set; }
        int Y { get; set; }
    }
}