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
        [SerializeField]
        private Sprite popUpSprite;

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

        public Sprite PopUpSprite
        {
            get { return popUpSprite; }
        }

        public void SetItem(ItemIds.Item newId, Sprite newIcon, string newMessage, Sprite newPopUpSprite)
        {
            id = newId;
            icon = newIcon;
            popUpMessage = newMessage;
            popUpSprite = newPopUpSprite;
        }
    }
}