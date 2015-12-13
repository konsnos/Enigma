using UnityEngine;
using System.Collections;

namespace Enigma.MiniGames
{
    public class HiddenObjectItemSlot : MonoBehaviour
    {
        [SerializeField]
        private ItemSlotIds.Item Id;
        /// <summary>
        /// Collider of the object to disable if the object is removed.
        /// </summary>
        private Collider col;
        [SerializeField]
        private GameObject itemGo;
        /// <summary>
        /// Position of the camera when this minigame is played.
        /// </summary>
        [SerializeField]
        private GameObject camPlaceHolder;

        void Awake()
        {
            col = itemGo.GetComponent<Collider>();
        }

        public void ResetSlot()
        {
            itemGo.SetActive(true);
        }

        public void Remove()
        {
            itemGo.SetActive(false);
        }
    }
}