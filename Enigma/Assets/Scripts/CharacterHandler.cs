using UnityEngine;
using System.Collections;
using Enigma.UserInterface;

namespace Enigma
{
    public class CharacterHandler : MonoBehaviour
    {
        bool mouseAction;
        
        void Awake()
        {
            mouseAction = false;
        }

        void Update()
        {
            if(Input.GetMouseButton(0) && !mouseAction)
            {
                raycastForInteraction();
            }
            else
            {
                if (mouseAction)
                    mouseAction = false;
            }
        }

        private void raycastForInteraction()
        {
            RaycastHit[] hits;
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f));

            LayerMask mask = 1 << Layers.Interaction;
            hits = Physics.RaycastAll(ray, 2f, mask);
            Item tempItem;
            foreach(RaycastHit hit in hits)
            {
                tempItem = hit.transform.GetComponent<Item>();
                if(tempItem)
                {
                    Inventory.Singleton.AddItem(tempItem);
                    if(tempItem.PopUpMessage != "" || tempItem.PopUpSprite != null)
                        UIHandler.Singleton.ShowPopUp(tempItem.PopUpMessage, tempItem.PopUpSprite);
                    Destroy(hit.transform.gameObject);
                    break;
                }
            }
        }
    }
}