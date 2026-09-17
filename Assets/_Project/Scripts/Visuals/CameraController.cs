using UnityEngine;

namespace Helicopter.Visuals
{
    public class CameraController : MonoBehaviour
    {
        [Header("Target & Offsets")]
        [SerializeField] private Transform helTransform;
        [SerializeField] private Vector3 offset = new Vector3(0.3f, 15f, -53f);

        [Header("Follow Settings")]
        [SerializeField] private float smoothSpeed = 5f;
        [SerializeField] private float rotationSpeed = 5f;

        private Vector3 currentVelocity;

        private void LateUpdate()
        {
            if (helTransform == null) return;

            Vector3 flatForward = helTransform.forward;
            flatForward.y = 0f;

            if (flatForward.sqrMagnitude < 0.001f)
            {
                flatForward = helTransform.up;
                flatForward.y = 0f;
            }
            flatForward.Normalize();

            Quaternion targetYaw = Quaternion.LookRotation(flatForward, Vector3.up);
            Vector3 desiredPosition = helTransform.position + (targetYaw * offset);

            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, 1f / smoothSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetYaw, rotationSpeed * Time.deltaTime);
        }
    }
}
