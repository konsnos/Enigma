using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Enigma.Sound;

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
        [SerializeField]
        private GameObject crosshair;

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

        private bool popUpIsOpen;

        /// <summary>
        /// Black background for outro.
        /// </summary>
        [SerializeField]
        private GameObject outroGo;
        /// <summary>
        /// Audio for outro.
        /// </summary>
        private AudioSource outroAs;
        /// <summary>
        /// Reference to canvas group for alpha animation.
        /// </summary>
        private CanvasGroup outroCanvasGroup;
        /// <summary>
        /// Rect transform for credits.
        /// </summary>
        [SerializeField]
        private RectTransform outroRT;

        private float popUpShowTS;
        private float popUpCloseIgnoreDuration = 0.5f;

        public bool PopUpIsOpen
        {
            get { return popUpIsOpen; }
        }

        void Awake()
        {
            popUpIsOpen = false;
            Singleton = this;
            dragActive = false;

            outroAs = outroGo.GetComponent<AudioSource>();
            outroCanvasGroup = outroGo.GetComponent<CanvasGroup>();
        }

        void Start()
        {
            CharacterHandler.Singleton.OnGameWon += PlayOutro;
        }

        public void PlayOutro()
        {
            Debug.Log("[UIHandler] PlayOutro");
            outroGo.SetActive(true);
            LeanTween.value(outroGo, 0f, 1f, 1f).setOnUpdate(outroUpdate);
            LeanTween.moveY(outroRT, outroRT.sizeDelta.y, 60f);
            outroAs.Play();

            Invoke("exitGame", outroAs.clip.length);
        }

        private void outroUpdate(float value)
        {
            outroCanvasGroup.alpha = value;
        }

        private void exitGame()
        {
            Application.Quit();
        }

        public void SetCrosshairActive(bool value)
        {
            crosshair.SetActive(value);
        }

        void Update()
        {
            if(dragActive)
            {
                dragT.position = Input.mousePosition;
                if (Input.GetMouseButtonUp(0))
                {
                    Click.Singleton.PlaySound();
                    RaycastHit[] hits = CharacterHandler.GetHits(Input.mousePosition, 1 << Layers.Interaction, 3f);

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
            popUpIsOpen = true;

            popUpText.text = message;
            if (image != null)
            {
                popUpImg.color = new Color(1f, 1f, 1f, 1f);
                popUpImg.sprite = image;
                popUpImg.SetNativeSize();
            }
            else
            {
                popUpImg.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(10, 10);
                popUpImg.color = new Color(1f, 1f, 1f, 0f);
            }

            popUpShowTS = Time.time + popUpCloseIgnoreDuration;

            if (OnShow != null)
                OnShow();
        }

        public void HidePopUp()
        {
            if (Time.time > popUpShowTS)
            {
                Debug.Log("[UIHandler] Hide pop up");
                PopUpPanel.SetActive(false);
                popUpIsOpen = false;
                CharacterHandler.ActivateIgnoreMouseBtn();

                if (OnHide != null)
                    OnHide();
            }
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