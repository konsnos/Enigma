using UnityEngine;
using System.Collections;

namespace Enigma
{
    public class LightsHandler : MonoBehaviour
    {
        [SerializeField]
        private Light[] normalLights;
        [SerializeField]
        private Light[] alarmLights;
        [SerializeField]
        private Renderer[] alarmEmmissionLights;
        [SerializeField]
        private Renderer[] emmissionLights;

        [SerializeField]
        private float alarmMaxIntensity;

        [SerializeField]
        private float alarmDuration = 1f;

        /// before build enable emmission.
        void Start()
        {
            Color initCol = new Color(0.1f, 0.1f, 0.1f);
            foreach (Renderer rend in alarmEmmissionLights)
            {
                foreach(Material mat in rend.sharedMaterials)
                {
                    mat.SetColor("_EmissionColor", initCol);
                }
            }

            foreach (Renderer rend in emmissionLights)
            {
                foreach (Material mat in rend.sharedMaterials)
                {
                    mat.SetColor("_EmissionColor", Color.white);
                }
            }
        }

        public void EnableAlarms()
        {
            foreach (Light light in normalLights)
                light.gameObject.SetActive(false);
            // Hide emmision
            foreach (Renderer rend in emmissionLights)
            {
                foreach(Material mat in rend.sharedMaterials)
                {
                    mat.SetColor("_EmissionColor", Color.black);
                }
            }

            Invoke("openAlarmLights", 2f);
        }

        private void openAlarmLights()
        {
            foreach (Light light in alarmLights)
            {
                light.gameObject.SetActive(true);
                light.intensity = 0f;
            }

            LeanTween.value(this.gameObject, 0f, alarmMaxIntensity, alarmDuration).setOnUpdate(updateLightIntensity).setLoopPingPong();
        }

        private void updateLightIntensity(float value)
        {
            if (alarmEmmissionLights.Length > 0)
            {
                float colorValue = value / alarmMaxIntensity;
                Color emmissionCol = new Color(colorValue, colorValue, colorValue);
                alarmEmmissionLights[0].sharedMaterial.SetColor("_EmissionColor", emmissionCol);
            }
            foreach (Light light in alarmLights)
            {
                light.intensity = value;
            }
        }
    }
}