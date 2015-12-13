using UnityEngine;
using UnityEngine.UI;

namespace Enigma.MiniGames
{
    public class Checklist : MonoBehaviour
    {
        public ItemSlotIds.Item id;
        public string name;
        private bool found;

        public bool Found
        {
            get { return found; }
        }

        [SerializeField]
        private Text text;
        [SerializeField]
        private GameObject strikethrough;

        /// <summary>
        /// Assigns new name and removes strikethrough.
        /// </summary>
        /// <param name="newName"></param>
        public void ChangeName(ItemSlotIds.Item newId)
        {
            found = false;
            id = newId;
            name = ItemSlotIds.GetName(id);
            text.text = name;
            strikethrough.SetActive(false);
        }

        public void ResetChecklist()
        {
            found = false;
            text.text = name = "";
            strikethrough.SetActive(false);
        }

        public void ItemFound()
        {
            found = true;
            strikethrough.SetActive(true);
        }
    }
}