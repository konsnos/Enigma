using UnityEngine;

namespace Enigma
{
    public class Item : MonoBehaviour
    {
        [SerializeField]
        private int id;
        [SerializeField]
        private Sprite icon;
        [SerializeField]
        private string popUpMessage = "";
        [SerializeField]
        private Sprite popUpSprite;

        private GameObject instancedObject;

        public int Id
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
    }
}