using UnityEngine;
using System.Collections;

namespace Enigma.Sound
{
    public class Click : MonoBehaviour
    {
        public static Click Singleton;

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