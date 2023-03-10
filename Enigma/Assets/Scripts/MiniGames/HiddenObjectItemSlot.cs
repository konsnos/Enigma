using UnityEngine;
using System.Collections;

namespace Enigma.MiniGames
{
    public class HiddenObjectItemSlot : MonoBehaviour
    {
        [SerializeField]
        private ItemSlotIds.Item id;
        /// <summary>
        /// Collider of the object to disable if the object is removed.
        /// </summary>
        private Collider col;
        [SerializeField]
        private GameObject itemGo;

        public ItemSlotIds.Item Id
        {
            get { return id; }
        }

        void Awake()
        {
            col = GetComponent<Collider>();
        }

        public void ResetSlot()
        {
            itemGo.SetActive(true);
            col.enabled = true;
        }

        public void Remove()
        {
            itemGo.SetActive(false);
            col.enabled = false;
        }
    }
}