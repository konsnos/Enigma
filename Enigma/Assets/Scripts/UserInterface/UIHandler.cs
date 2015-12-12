using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Enigma.UserInterface
{
    public class UIHandler : MonoBehaviour
    {
        public static UIHandler Singleton;

        public delegate void Refresh();
        public event Refresh OnShow;
        public event Refresh OnHide;

        /// <summary>
        /// Background to UI to show that player can't react with.
        /// </summary>
        [SerializeField]
        private GameObject PopUpPanel;
        /// <summary>
        /// Image reference for the pop up image.
        /// </summary>
        [SerializeField]
        private Image popUpImg;
        /// <summary>
        /// Image reference for the pop up text.
        /// </summary>
        [SerializeField]
        private Text popUpText;

        /*********** DRAG *******/
        private bool dragActive;
        /// <summary>
        /// item that is being dragged.
        /// </summary>
        private Item dragItem;
        /// <summary>
        /// Image of the dragged item.
        /// </summary>
        [SerializeField]
        private Image dragImg;
        /// <summary>
        /// RectTransform of the dragged item.
        /// </summary>
        [SerializeField]
        private RectTransform dragT;

        void Awake()
        {
            Singleton = this;
            dragActive = false;
        }

        void Update()
        {
            if(dragActive)
            {
                dragT.position = Input.mousePosition;
                if (Input.GetMouseButtonUp(0))
                {
                    RaycastHit[] hits = CharacterHandler.GetHits(Input.mousePosition);

                    foreach(RaycastHit hit in hits)
                    {
                        Debug.Log("[UIHandler] hit object " + hit.transform.gameObject.name);
                        switch(hit.transform.gameObject.name)
                        {
                            case "LockedDrawer":
                                if(dragItem.Id == ItemIds.Item.Locked_Drawer_Key)
                                {
                                    if(Inventory.Singleton.UseItem(ItemIds.Item.Locked_Drawer_Key))
                                    {
                                        DrawerHandler drawerHandler = hit.transform.gameObject.GetComponent<DrawerHandler>();
                                        drawerHandler.Unlock();
                                        drawerHandler.Interact();
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    }

                    ResetDrag();
                }
            }
        }

        public void ShowPopUp(string message, Sprite image)
        {
            PopUpPanel.SetActive(true);

            popUpText.text = message;
            if (image != null)
            {
                popUpImg.sprite = image;
                popUpImg.SetNativeSize();
                popUpImg.gameObject.SetActive(true);
            }
            else
                popUpImg.gameObject.SetActive(false);

            if (OnShow != null)
                OnShow();
        }

        public void HidePopUp()
        {
            Debug.Log("[UIHandler] Hide pop up");
            PopUpPanel.SetActive(false);

            if (OnHide != null)
                OnHide();
        }

        /// <summary>
        /// Checks if the game object panel is active.
        /// </summary>
        /// <returns>True if the panel is active.</returns>
        public bool IsPanelActive()
        {
            return PopUpPanel.activeSelf;
        }

        public void InitDrag(Item item)
        {
            dragItem = item;
            dragT.gameObject.SetActive(true);
            dragImg.sprite = dragItem.Icon;
            dragT.position = Input.mousePosition;
            dragActive = true;
        }

        public void ResetDrag()
        {
            dragActive = false;
            dragT.gameObject.SetActive(false);
        }
    }
}