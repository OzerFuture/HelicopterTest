using UnityEngine;
using Helicopter.Core;

namespace Helicopter.HelInput
{
    public class InputController : MonoBehaviour, IInputService
    {
        private InputSystem_Actions inputSystem;

        private Vector2 pitchAndRollVector;

        private float yawValue;

        private float climbValue;


        private void Awake()
        {
            inputSystem ??= new InputSystem_Actions();
        }

        private void OnEnable()
        {
            inputSystem?.Enable();
        }


        private void Update()
        {
            ReadClimbValue();
            ReadPitchAndRollVector();
            ReadYawValue();

            if(inputSystem.UI.Cancel.inProgress)
            {
                Application.Quit();
            }
        }

        private void ReadYawValue()
        {
            yawValue = inputSystem.Player.Yaw.ReadValue<float>();
        }

        private void ReadClimbValue()
        {
            climbValue = inputSystem.Player.Climb.ReadValue<float>();
        }

        private void ReadPitchAndRollVector()
        {
            pitchAndRollVector = inputSystem.Player.Move.ReadValue<Vector2>();
        }


        public float GetYawValue()
        {
            return yawValue;
        }

        public float GetClimbValue()
        {
            return climbValue;
        }

        public Vector2 GetPitchAndRollVector()
        {
            return pitchAndRollVector;
        }


        private void OnDisable()
        {
            inputSystem?.Disable();
        }
    }

}
