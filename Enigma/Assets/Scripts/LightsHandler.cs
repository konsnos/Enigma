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
        private GameObject[] emmissionLights;

        [SerializeField]
        private float alarmMaxIntensity;

        [SerializeField]
        private float alarmDuration = 1f;

        /// before build enable emmission.

        public void EnableAlarms()
        {
            foreach (Light light in normalLights)
                light.gameObject.SetActive(false);
            // Hide emmision
            foreach(GameObject goLight in emmissionLights)
            {
                Renderer rend = goLight.GetComponent<Renderer>();
                if(rend)
                {
                    foreach(Material mat in rend.materials)
                    {
                        mat.SetColor("_EmissionColor", Color.black);
                    }
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
            foreach (Light light in alarmLights)
            {
                light.intensity = value;
            }
        }
    }
}