using UnityEngine;
using System.Collections;

namespace Enigma.MiniGames
{
    public class HiddenObjectGame : MiniGameBase
    {
        public event Refresh OnExitted;
        public event Refresh OnSolved;

        /// <summary>
        /// When true the mini game is active.
        /// </summary>
        private bool isActive;

        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        private bool gameStarted;
        float startTS;
        [SerializeField]
        float gameDuration = 20f;
        [SerializeField]
        float wrongPickPenalty = 1f;

        [SerializeField]
        private HiddenObjectItemSlot[] hiddenObject;
        [SerializeField]
        private Checklist[] checklist;

        /// <summary>
        /// Position of the camera when this minigame is played.
        /// </summary>
        [SerializeField]
        private GameObject camPlaceHolder;

        void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                if (OnExitted != null)
                    OnExitted();
            }
        }

        public Vector3 GetCamPosition()
        {
            return camPlaceHolder.transform.position;
        }

        public Quaternion GetCamRotation()
        {
            return camPlaceHolder.transform.rotation;
        }
    }
}