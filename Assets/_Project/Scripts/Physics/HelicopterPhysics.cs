using Helicopter.Core;
using UnityEngine;

namespace Helicopter.HelPhysics
{
    public class HelicopterPhysics : MonoBehaviour
    {

        [SerializeField] private HelicopterConfig mainConfig;

        private IInputService inputService;
        private Rigidbody rb;

        private float enginePower;
        private float altitude;

        private bool isOnGround = true;

        private Vector3 dragForce;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();

            if (rb == null)
            {
                Debug.LogError($"[{nameof(HelicopterPhysics)}] on '{gameObject.name}': Missing Rigidbody component! Disabling script.", this);
                enabled = false;
                return;
            }

            inputService = GetComponent<IInputService>() ?? GetComponentInParent<IInputService>();

            if (inputService == null)
            {
                Debug.LogError($"[{nameof(HelicopterPhysics)}] on '{gameObject.name}': No component implementing IInputService found in hierarchy! Disabling script.", this);
                enabled = false;
                return;
            }

            if (mainConfig == null)
            {
                Debug.LogError($"[{nameof(HelicopterPhysics)}] on '{gameObject.name}': HelicopterConfig scriptable object is not assigned in the Inspector! Disabling script.", this);
                enabled = false;
                return;
            }

            rb.mass = mainConfig.helicopterMass;
            rb.angularDamping = mainConfig.angularDamping;
            rb.centerOfMass = mainConfig.centerOfMassOffset;

            rb.solverIterations = 6;           
            rb.solverVelocityIterations = 1; 

            if (mainConfig.useCustomInertia)
            {
                rb.inertiaTensor = mainConfig.customInertiaTensor;
            }

        }

        private void FixedUpdate()
        {

            Vector2 tiltVector = inputService.GetPitchAndRollVector();

            float yawInputValue = inputService.GetYawValue();

            float climbInputValue = inputService.GetClimbValue();

            CheckIsOnGround();
            CheckAltitude();
            CheckEngineState(climbInputValue);

            ApplyDragForce();
            ApplyVerticalThrust(climbInputValue);
            ApplyTilt(tiltVector);
            ApplyYawRotation(yawInputValue);
        }

        private void StartHelicopterEngine()
        {
            enginePower += mainConfig.engineAcceleration * Time.fixedDeltaTime;

            enginePower = Mathf.Clamp(enginePower, 0, 1);
        }

        private void StopHelicopteEngine()
        {
            enginePower -= mainConfig.engineDeceleration * Time.fixedDeltaTime;

            enginePower = Mathf.Clamp(enginePower, 0, 1);
        }


        private void CheckEngineState(float climbInputValue)
        {
            if (climbInputValue > 0)
            {
                StartHelicopterEngine();
            }

            if (enginePower < 1 && climbInputValue == 0)
            {
                StopHelicopteEngine();
            }

            if (isOnGround && climbInputValue < 0)
            {
                rb.linearVelocity = Vector3.zero;
                StopHelicopteEngine();
            }
        }

        private void ApplyDragForce()
        {
            if (enginePower != 1)
                return;

            Vector3 localVel = transform.InverseTransformDirection(rb.linearVelocity);

            Vector3 localDrag = new Vector3(
                localVel.x * mainConfig.sideDrag,
                localVel.y * mainConfig.verticalDrag,
                localVel.z * mainConfig.forwardDrag
            );

            Vector3 worldDrag = transform.TransformDirection(localDrag);

            rb.AddForce(-worldDrag * rb.mass, ForceMode.Force);
        }

        private float GetAirDensityFactor()
        {
            float currentAltitude = transform.position.y;

            if (currentAltitude <= mainConfig.minCeiling) return 1f;

            float factor = 1f - ((currentAltitude - mainConfig.minCeiling) / (mainConfig.maxCeiling - mainConfig.minCeiling));

            return Mathf.Clamp01(factor);
        }

        private void ApplyTilt(Vector3 moveVector)
        {
            if (enginePower != 1)
                return;

            float targetTiltMagnitude = moveVector.magnitude * mainConfig.maxTiltAngle;

            Vector3 targetWorldUp = Vector3.up;

            if (moveVector.magnitude > 0.05f)
            {
                Vector3 localTiltDir = new Vector3(-moveVector.x, 0f, -moveVector.y).normalized;
                Vector3 worldTiltDir = transform.TransformDirection(localTiltDir);

                targetWorldUp = Quaternion.AngleAxis(targetTiltMagnitude, Vector3.Cross(worldTiltDir, Vector3.up)) * Vector3.up;
            }

            Vector3 autoLevelAxis = Vector3.Cross(transform.up, targetWorldUp);

            rb.AddTorque(autoLevelAxis * (mainConfig.tiltAcceleration * rb.mass), ForceMode.Force);

            rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, Vector3.zero, Time.fixedDeltaTime * mainConfig.autoStabilizationSpeed);
        }

        private void ApplyYawRotation(float yaw)
        {
            if (enginePower != 1)
                return;

            rb.AddTorque(transform.up * (rb.mass * yaw * mainConfig.yawAcceleration), ForceMode.Force);
        }

        private void ApplyVerticalThrust(float climbInputValue)
        {
            if (enginePower == 1)
            {                  
                Vector3 totalLiftForce = rb.mass*(climbInputValue * mainConfig.climbAcceleration + Physics.gravity.magnitude) * GetAirDensityFactor() * transform.up;

                rb.AddForce(totalLiftForce, ForceMode.Force);
            }
        }

        private void CheckIsOnGround()
        {
            Ray ray = new(transform.position - 1.3f*transform.up, Vector3.down);

            isOnGround = Physics.Raycast(ray, 2);
        }
        
        private void CheckAltitude()
        {
            Ray ray = new(transform.position - 1.3f*transform.up, Vector3.down);

            Physics.Raycast(ray, out RaycastHit hit, 300);

            altitude = hit.distance;
        }

        public float GetAltitude() => altitude;
        public float GetEnginePower() => enginePower;
    }
}
