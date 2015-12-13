using UnityEngine;

namespace Enigma
{
    public class Item : MonoBehaviour
    {
        [SerializeField]
        private ItemIds.Item id;
        [SerializeField]
        private Sprite icon;
        [SerializeField]
        private string popUpMessage = "";

        private GameObject instancedObject;

        public ItemIds.Item Id
        {
            get { return id; }
        }

        public Sprite Icon
        {
            get { return icon; }
        }

        public string PopUpMessage
        {
            get { return popUpMessage; }
        }

        public void SetItem(ItemIds.Item newId, Sprite newIcon, string newMessage)
        {
            id = newId;
            icon = newIcon;
            popUpMessage = newMessage;
        }
    }
}