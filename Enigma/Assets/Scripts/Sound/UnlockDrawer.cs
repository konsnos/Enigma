using UnityEngine;
using System.Collections;

namespace Enigma.Sound
{
    public class UnlockDrawer : MonoBehaviour
    {
        public static UnlockDrawer Singleton;

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