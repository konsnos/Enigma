using UnityEngine;
using System.Collections;

namespace Enigma.UserInterface
{
    public class ItemInteraction : MonoBehaviour
    {
        bool pointerDown;
        bool pointerExit;

        Item item;

        void Awake()
        {
            item = GetComponent<Item>();
        }

        void Start()
        {
            pointerDown = false;
            pointerExit = false;
        }

        public void OnPointerDown()
        {
            pointerDown = true;
            pointerExit = false;
            Debug.Log("[ItemInteraction] On pointer down");
        }

        public void OnPointerUp()
        {
            if(pointerDown && !pointerExit) // Show pop up
            {
                pointerDown = false;
                UIHandler.Singleton.ShowPopUp(item.PopUpMessage, item.Icon);
                Debug.Log("[ItemInteraction] On pointer up. Show pop up");
            }
            // drop to interact check is done from UIhandler.
        }

        public void OnPointerExit()
        {
            if (pointerDown)
            {
                pointerDown = false;
                pointerExit = true;
                UIHandler.Singleton.InitDrag(item);
            }
        }
    }
}