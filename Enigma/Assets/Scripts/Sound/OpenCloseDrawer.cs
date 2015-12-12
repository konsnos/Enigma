using UnityEngine;
using System.Collections;

namespace Enigma.Sound
{
    public class OpenCloseDrawer : MonoBehaviour
    {
        public static OpenCloseDrawer Singleton;

        private AudioSource audioSrc;

        void Awake()
        {
            Singleton = this;
            audioSrc = GetComponent<AudioSource>();
        }

        public void PlaySound()
        {
            audioSrc.Play();
        }
    }
}