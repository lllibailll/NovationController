using Core.Launchpad.Impl;

namespace Core.Launchpad
{
    public interface ILaunchpad
    {
        event OnButtonStateChangedEvent OnButtonStateChanged;
        void Clear();
    }
}