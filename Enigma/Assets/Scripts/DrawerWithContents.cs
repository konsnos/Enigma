using UnityEngine;
using System.Collections;

namespace Enigma
{
    public class DrawerWithContents : DrawerHandler
    {
        [SerializeField]
        private GameObject content;

        void Start()
        {
            content.GetComponent<Collider>().enabled = IsActive;
        }

        public override void Interact()
        {
            base.Interact();

            if(content)
                content.GetComponent<Collider>().enabled = IsActive;
        }
    }
}