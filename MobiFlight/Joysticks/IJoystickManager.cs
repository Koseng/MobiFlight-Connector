using System;
using System.Collections.Generic;

namespace MobiFlight
{
    public interface IJoystickManager
    {
        event EventHandler Connected;
        event ButtonEventHandler OnButtonPressed;
        void Connect();
        List<Joystick> GetExcludedJoysticks();
        List<Joystick> GetJoysticks();
        bool JoysticksConnected();
        void SetHandle(IntPtr handle);
        void Shutdown();
        void Startup();
        void Stop();
    }
}