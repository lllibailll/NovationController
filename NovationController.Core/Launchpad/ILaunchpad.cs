using NovationController.Core.Launchpad.Impl;

namespace NovationController.Core.Launchpad
{
    public interface ILaunchpad
    {
        event OnButtonStateChangedEvent OnButtonStateChanged;
        void Clear();
    }
}