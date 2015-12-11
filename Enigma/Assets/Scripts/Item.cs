using UnityEngine;

namespace Enigma
{
    public class Item : MonoBehaviour
    {
        [SerializeField]
        private int id;
        [SerializeField]
        private Sprite icon;

        private GameObject instancedObject;

        public int Id
        {
            get { return id; }
        }

        public Sprite Icon
        {
            get { return icon; }
        }
    }
}