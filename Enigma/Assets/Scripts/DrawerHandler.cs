using UnityEngine;
using System.Collections;
using Enigma.Sound;

namespace Enigma
{
    public class DrawerHandler : MonoBehaviour
    {
        private Vector3 originalPosition;
        [SerializeField]
        private Vector3 interactionPosition;
        [SerializeField]
        private float interactionDuration;

        [SerializeField]
        private bool isActive;

        [SerializeField]
        private bool isLocked;

        public bool IsLocked
        {
            get { return isLocked; }
        }

        void Awake()
        {
            isActive = false;
            originalPosition = this.gameObject.transform.localPosition;
        }

        public void Unlock()
        {
            isLocked = false;
            UnlockDrawer.Singleton.PlaySound();
        }

        /// <summary>
        /// Interacts with the object if not locked.
        /// </summary>
        public void Interact()
        {
            if (!isLocked)
            {
                LeanTween.cancel(this.gameObject);
                if (isActive)
                {
                    LeanTween.moveLocal(this.gameObject, originalPosition, interactionDuration);
                    Debug.Log("[DrawerHandler] Closing drawer.");
                }
                else
                {
                    LeanTween.moveLocal(this.gameObject, interactionPosition, interactionDuration);
                    Debug.Log("[DrawerHandler] Opening drawer.");
                }
                OpenCloseDrawer.Singleton.PlaySound();

                isActive = !isActive;
            }
        }
    }
}