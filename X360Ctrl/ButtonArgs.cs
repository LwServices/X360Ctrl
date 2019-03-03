using SharpDX.XInput;
using System;

namespace X360Ctrl
{
    public class ButtonArgs : EventArgs
    {
        public bool NewValue { get; set; }
        public bool OldValue { get; set; }

        public GamepadButtonFlags Button { get; set; }
    }
}