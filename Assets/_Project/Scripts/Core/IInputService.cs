using UnityEngine;

namespace Helicopter.Core
{
    public interface IInputService
    {
        float GetYawValue();

        float GetClimbValue();

        Vector2 GetPitchAndRollVector();
    }
}
