using SharpDX.XInput;
using System;
using System.Timers;

namespace X360Ctrl
{
    public class XInputController
    {
        private const int MaxAnalogValue = 32768;
        private const int MaxTriggerValue = 255;

        private readonly Controller _controller;
        private Gamepad _gamepad;

        private GamepadButtonFlags _lastState;

        private Timer _timer;

        public XInputController(UserIndex index)
        {
            _controller = new Controller(index);
        }

        public bool A => (_gamepad.Buttons & GamepadButtonFlags.A) != 0;
        public bool B => (_gamepad.Buttons & GamepadButtonFlags.B) != 0;
        public bool Back => (_gamepad.Buttons & GamepadButtonFlags.Back) != 0;
        public bool DPadDown => (_gamepad.Buttons & GamepadButtonFlags.DPadDown) != 0;
        public bool DPadLeft => (_gamepad.Buttons & GamepadButtonFlags.DPadLeft) != 0;
        public bool DPadRight => (_gamepad.Buttons & GamepadButtonFlags.DPadRight) != 0;
        public bool DPadUp => (_gamepad.Buttons & GamepadButtonFlags.DPadUp) != 0;
        public bool IsConnected => _controller.IsConnected;
        public bool LeftShoulder => (_gamepad.Buttons & GamepadButtonFlags.LeftShoulder) != 0;
        public bool LeftThumb => (_gamepad.Buttons & GamepadButtonFlags.LeftThumb) != 0;
        public bool RightShoulder => (_gamepad.Buttons & GamepadButtonFlags.RightShoulder) != 0;
        public bool RightThumb => (_gamepad.Buttons & GamepadButtonFlags.RightThumb) != 0;
        public bool Start => (_gamepad.Buttons & GamepadButtonFlags.Start) != 0;
        public bool X => (_gamepad.Buttons & GamepadButtonFlags.X) != 0;

        public bool Y => (_gamepad.Buttons & GamepadButtonFlags.Y) != 0;

        public double LeftThumbX => _gamepad.LeftThumbX / (double)(2 * MaxAnalogValue) + 0.5;
        public double LeftThumbY => _gamepad.LeftThumbY / (double)(2 * MaxAnalogValue) + 0.5;
        public double LeftTrigger => _gamepad.LeftTrigger / (double)MaxTriggerValue;
        public double RightThumbX => _gamepad.RightThumbX / (double)(2 * MaxAnalogValue) + 0.5;
        public double RightThumbY => _gamepad.RightThumbY / (double)(2 * MaxAnalogValue) + 0.5;
        public double RightTrigger => _gamepad.RightTrigger / (double)MaxTriggerValue;

        public event EventHandler ButtonPressed;

        public event EventHandler Updated;

        public double Normalization(int current, int min, int max)
        {
            return (double)current / (max - min);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"IsConnected: {IsConnected,6}" + Environment.NewLine +
                   $"A: {A,6}, B: {B,6}, X: {X,6}, Y: {Y,6}" + Environment.NewLine +
                   $"Back: {Back,6}, Start: {Start,6} " + Environment.NewLine +
                   $"DPadDown: {DPadDown,6}, DPadLeft: {DPadLeft,6}, DPadRight: {DPadRight,6}, DPadUp: {DPadUp,6}" + Environment.NewLine +
                   $"LeftShoulder: {LeftShoulder,6}, RightShoulder: {RightShoulder,6},LeftThumb: {LeftThumb,6}, , RightThumb: {RightThumb,6}" + Environment.NewLine +
                   $"LeftThumbX: {LeftThumbX}" + Environment.NewLine +
                   $"LeftThumbY: {LeftThumbY}" + Environment.NewLine +
                   $"LeftTrigger: {LeftTrigger}" + Environment.NewLine +
                   $"RightThumbX: {RightThumbX}" + Environment.NewLine +
                   $"RightThumbY: {RightThumbY}" + Environment.NewLine +
                   $"RightTrigger: {RightTrigger}" + Environment.NewLine;
        }

        /// <summary>
        /// </summary>
        public void StartAutoUpdate()
        {
            _timer = new Timer
            {
                Interval = 100,
                AutoReset = true,
                Enabled = true
            };
            _timer.Elapsed += (sender, args) => Update();
            _timer.Start();
        }

        /// <summary>
        /// </summary>
        public void StopAutoUpdate()
        {
            _timer.Stop();
        }

        /// <summary>
        /// </summary>
        public bool Update()
        {
            if (!_controller.IsConnected)
            {
                // No Updates Controller is not connected
                return false;
            }

            // Get State
            _gamepad = _controller.GetState().Gamepad;
            DetectButtonModified();

            //Fire Event Updated
            OnUpdated();

            return true;
        }

        private void DetectButtonModified()
        {
            if (_gamepad.Buttons != _lastState)
            {
                var gamepadButtonFlags = _gamepad.Buttons ^ _lastState;
                if ((_gamepad.Buttons & gamepadButtonFlags) != 0) FireButtonPressed(gamepadButtonFlags);
            }

            _lastState = _gamepad.Buttons;
        }

        /// <summary>
        /// </summary>
        /// <param name="button"></param>
        protected virtual void FireButtonPressed(GamepadButtonFlags button)
        {
            ButtonPressed?.Invoke(this, new ButtonArgs { Button = button, NewValue = true, OldValue = false });
        }

        protected virtual void OnUpdated()
        {
            Updated?.Invoke(this, EventArgs.Empty);
        }
    }
}