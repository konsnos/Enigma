using UnityEngine;
using System.Collections;

namespace Enigma.Sound
{
    public class ClickWrong : MonoBehaviour
    {
        public static ClickWrong Singleton;

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