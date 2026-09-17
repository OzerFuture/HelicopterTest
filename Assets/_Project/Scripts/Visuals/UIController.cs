using TMPro;
using UnityEngine;
using Helicopter.HelPhysics;

namespace Helicopter.Visuals
{
    public class UIController : MonoBehaviour
    {
        [Header("UI Text References")]
        [SerializeField] private TMP_Text engineValueText;
        [SerializeField] private TMP_Text verticalVelocityText;
        [SerializeField] private TMP_Text horizontalSpeedText;
        [SerializeField] private TMP_Text radarAltitudeText;  
        [SerializeField] private TMP_Text seaLevelAltitudeText;  
        [SerializeField] private TMP_Text fpsText;

        [Header("References")]
        [SerializeField] private Rigidbody rb;
        [SerializeField] private HelicopterPhysics physics;

        private float fpsDeltaTime;

        private void Awake()
        {
            if (rb == null || physics == null)
            {
                Debug.LogError($"[{nameof(UIController)}] on '{gameObject.name}': Missing reference to Rigidbody or HelicopterPhysics! Disabling script.", this);
                enabled = false;
            }
        }

        private void Update()
        {
            UpdatePerformanceStats();
            UpdateFlightHUD();
        }

        private void UpdateFlightHUD()
        {
            UpdateText(engineValueText, "Engine Power", physics.GetEnginePower() * 100f, "%");

            UpdateText(verticalVelocityText, "Vertical Speed", rb.linearVelocity.y, "m/s");

            Vector2 horizVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.z);

            UpdateText(horizontalSpeedText, "Horizontal Speed", horizVelocity.magnitude, "m/s");

            UpdateText(seaLevelAltitudeText, "Altitude (MSL)", rb.gameObject.transform.position.y, "m");

            UpdateText(radarAltitudeText, "Altitude (AGL)", physics.GetAltitude(), "m");
        }

        private void UpdatePerformanceStats()
        {
            if (fpsText == null) return;

            fpsDeltaTime += (Time.unscaledDeltaTime - fpsDeltaTime) * 0.1f;
            float currentFps = 1.0f / fpsDeltaTime;

            fpsText.text = $"FPS: {Mathf.CeilToInt(currentFps)}";
        }

        private void UpdateText(TMP_Text textContainer, string label, float value, string units)
        {
            if (textContainer == null) return;

            textContainer.text = $"{label}: {value:F1} {units}";
        }
    }
}