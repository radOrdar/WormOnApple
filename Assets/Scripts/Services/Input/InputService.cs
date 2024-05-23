using UnityEngine;

namespace Services.Input
{
    public class InputService : IInputService
    {
        private Joystick _joystick;

        public InputService(Joystick joystick)
        {
            _joystick = joystick;
        }

        public Vector2 JoystickDirection() => _joystick.Direction;
    }
}
