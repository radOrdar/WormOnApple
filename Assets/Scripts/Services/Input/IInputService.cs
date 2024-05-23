using UnityEngine;

namespace Services.Input
{
    public interface IInputService : IService
    {
        Vector2 JoystickDirection();
    }
}