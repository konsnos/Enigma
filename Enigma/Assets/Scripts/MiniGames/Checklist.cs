using UnityEngine;
using UnityEngine.UI;

namespace Enigma.MiniGames
{
    public class Checklist : MonoBehaviour
    {
        public string name;
        public bool found;

        [SerializeField]
        private Text text;
        [SerializeField]
        private GameObject strikethrough;

        public void ChangeName(string newName)
        {
            name = newName;
            text.text = name;
        }

        public void ResetChecklist()
        {
            text.text = name = "";
            strikethrough.SetActive(false);
        }

        public void ItemFound()
        {
            strikethrough.SetActive(true);
        }
    }
}