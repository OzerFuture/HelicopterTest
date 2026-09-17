using UnityEngine;
using Helicopter.HelPhysics;

namespace Helicopter.Visuals
{
    public class RotorVisualController : MonoBehaviour
    {

        [Header("Configurations & References")]
        [SerializeField] private HelicopterConfig mainConfig;
        [SerializeField] private HelicopterPhysics helPhysics;

        [Header("Propeller Transforms")]
        [SerializeField] private Transform upPropellerTransform;
        [SerializeField] private Transform backPropellerTransform;

        [Header("Renderers & Visuals")]
        [SerializeField] private Renderer rotorCylinderRenderer;
        [SerializeField] private Renderer rotorMainRenderer;
        [SerializeField] private float maxShaderSpeed = 50f;

        [Header("Audio")]
        [SerializeField] private AudioClip rotorSound;

        private float rotationAngle;

        private Material rotorMaterial;
        private Material rotorMainMaterial;
        private AudioSource audioSource;

        private static readonly int SpeedProperty = Shader.PropertyToID("_RotationSpeed");
        private static readonly int BlurProperty = Shader.PropertyToID("_BlurAmount");
        private static readonly int AlphaProperty = Shader.PropertyToID("_MasterAlpha");
        private static readonly int ColorProperty = Shader.PropertyToID("_BaseColor");

        private void Awake()
        {
            if (rotorCylinderRenderer != null)
            {
                rotorMaterial = rotorCylinderRenderer.material;
            }

            if (rotorMainRenderer != null)
            {
                rotorMainMaterial = rotorMainRenderer.material;
            }

            helPhysics = GetComponent<HelicopterPhysics>();

            audioSource = GetComponent<AudioSource>();

            PlayRotorSound();
        }

        private void PlayRotorSound()
        {
            audioSource.clip = rotorSound;

            audioSource.Play();

            audioSource.loop = true;
        }

        private void Update()
        {
            RotatePropeller();

            SetEngineThrottle(helPhysics.GetEnginePower());
        }

        private void RotatePropeller()
        {
            rotationAngle = helPhysics.GetEnginePower() * mainConfig.maxRpm;

            upPropellerTransform.Rotate(0, rotationAngle, 0, Space.Self);

            backPropellerTransform.Rotate(0, 0, rotationAngle, Space.Self);
        }

        public void SetEngineThrottle(float normalizedThrottle)
        {
            if (rotorMaterial == null) return;

            UpdateShaderEffects(normalizedThrottle);

            UpdateAudioState(normalizedThrottle);
        }

        private void UpdateShaderEffects(float normalizedThrottle)
        {
            float currentSpeed = normalizedThrottle * maxShaderSpeed;
            float currentBlur = Mathf.Lerp(0.1f, 0.9f, normalizedThrottle);
            float currentAlpha = 2 * Mathf.Clamp(normalizedThrottle - 0.5f, 0, 1);

            rotorMaterial.SetFloat(AlphaProperty, currentAlpha);
            rotorMaterial.SetFloat(SpeedProperty, currentSpeed);
            rotorMaterial.SetFloat(BlurProperty, currentBlur);

            Color newRotorColor = rotorMainMaterial.color;

            newRotorColor.a = Mathf.Clamp01(2*(1 - normalizedThrottle));

            rotorMainMaterial.SetColor(ColorProperty, newRotorColor);
        }

        private void UpdateAudioState(float normalizedThrottle)
        {
            audioSource.volume = normalizedThrottle;
            audioSource.pitch = normalizedThrottle;
        }
    }
}
