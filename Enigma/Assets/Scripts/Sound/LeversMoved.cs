using UnityEngine;
using System.Collections;

namespace Enigma.Sound
{
    public class LeversMoved : MonoBehaviour
    {
        public static LeversMoved Singleton;

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