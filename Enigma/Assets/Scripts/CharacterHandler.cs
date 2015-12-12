using UnityEngine;
using System.Collections;
using Enigma.UserInterface;
using Enigma.MiniGames;

namespace Enigma
{
    public class CharacterHandler : MonoBehaviour
    {
        public delegate void EnigmaEvent();
        public event EnigmaEvent OnGameWon;

        [SerializeField]
        private float cameraTransitionDuration = 0.5f;
        [SerializeField]
        private GameObject cameraPlaceholder;

        private LockCypher lockCypher;

        private static bool ignoreMousebtn;

        void Awake()
        {
            ignoreMousebtn = false;
        }

        void Update()
        {
            if (ignoreMousebtn)
                ignoreMousebtn = false;
            else
            {
                if (!Inventory.Singleton.IsShown && !UIHandler.Singleton.PopUpIsOpen && !LevelHandler.Singleton.IsLockCipherActive)
                {
                    if (Input.GetMouseButtonUp(0))
                        raycastForInteraction();
                }
            }
        }

        public static void ActivateIgnoreMouseBtn()
        {
            ignoreMousebtn = true;
        }

        private void makeAlarm()
        {
            LevelHandler.Singleton.EnableAlarm();
        }

        public static RaycastHit[] GetHits(Vector3 screenPoint)
        {
            RaycastHit[] hits;
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f));

            LayerMask mask = 1 << Layers.Interaction;
            hits = Physics.RaycastAll(ray, 2f, mask);
            Debug.Log("[CharacterHandler] Hits " + hits.Length);
            return hits;
        }

        private void raycastForInteraction()
        {
            RaycastHit[] hits = GetHits(new Vector3(Screen.width / 2, Screen.height / 2, 0f));
            Item tempItem;
            foreach(RaycastHit hit in hits)
            {
                Debug.Log("[CharacterHandler] hit object " + hit.transform.gameObject.name);
                tempItem = hit.transform.GetComponent<Item>();
                if(tempItem)
                {
                    Inventory.Singleton.AddItem(tempItem);
                    if (tempItem.PopUpMessage != "" || tempItem.PopUpSprite != null)
                    {
                        if (tempItem.Id == ItemIds.Item.Enigma_Part1 || tempItem.Id == ItemIds.Item.Enigma_Part2 || tempItem.Id == ItemIds.Item.Enigma_Part3)
                        {
                            int enigmaParts = Inventory.Singleton.GetAmountOfPartsOfEnigma();
                            switch (enigmaParts)
                            {
                                case 0:
                                    UIHandler.Singleton.ShowPopUp("I found a missing part! Good!", tempItem.PopUpSprite);
                                    break;
                                case 1:
                                    UIHandler.Singleton.ShowPopUp("One more piece to go! Great!", tempItem.PopUpSprite);
                                    Invoke("makeAlarm", Random.Range(3f, 5f));
                                    break;
                                case 2:
                                    UIHandler.Singleton.ShowPopUp("I have all parts now! Let’s take the machine...", tempItem.PopUpSprite);
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            UIHandler.Singleton.ShowPopUp(tempItem.PopUpMessage, tempItem.PopUpSprite);
                        }
                    }
                    Destroy(hit.transform.gameObject);
                    break;
                }
                else
                {
                    lockCypher = hit.transform.GetComponent<LockCypher>();

                    if(lockCypher)
                    {
                        lockCypher.IsActive = true;
                        LevelHandler.Singleton.UpdateLockCipherActive(true);
                        // disable character movement, disable cursor, disable inventory.
                        LeanTween.move(Camera.main.gameObject, lockCypher.CamPlaceHolder.transform.position, cameraTransitionDuration);
                        LeanTween.rotate(Camera.main.gameObject, lockCypher.CamPlaceHolder.transform.rotation.eulerAngles, cameraTransitionDuration);
                        lockCypher.OnSolved += lockCypherSolved;
                        lockCypher.OnExitted += lockCypherExitted;
                        break;
                    }
                    else
                    {
                        switch(hit.transform.gameObject.name)
                        {
                            case "SM_EnigmaMachine":
                                if (Inventory.Singleton.GetAmountOfPartsOfEnigma() == 3)
                                    OnGameWon();
                                else
                                    UIHandler.Singleton.ShowPopUp("Some parts are missing! I have to find them. Quickly!", null);
                                return;
                            case "LockedDrawer":
                                if (hit.transform.gameObject.GetComponent<DrawerHandler>().IsLocked)
                                    UIHandler.Singleton.ShowPopUp("It's locked...", null);
                                else
                                    hit.transform.gameObject.GetComponent<DrawerHandler>().Interact();
                                return;
                            case "LeverHandle":
                                hit.transform.gameObject.GetComponent<LeverHandler>().Use();
                                Levers.Singleton.checkIfSolved();
                                return;
                            case "LockedBox_Shelf1":
                                if (hit.transform.gameObject.GetComponent<DrawerHandler>().IsLocked)
                                    UIHandler.Singleton.ShowPopUp("It has some kind of mechanical lock.", null);
                                else
                                    hit.transform.gameObject.GetComponent<DrawerHandler>().Interact();
                                return;
                            case "LockedBox_Shelf2":
                                if (!hit.transform.gameObject.GetComponent<DrawerHandler>().IsLocked)
                                    hit.transform.gameObject.GetComponent<DrawerHandler>().Interact();
                                return;
                            default:
                                break;
                        }
                    }
                    
                }
            }
        }

        private void lockCypherSolved()
        {
            Debug.Log("[CharacterHandler] Lock cypher solved.");
            LevelHandler.Singleton.UpdateLockCipherActive(false);
            lockCypher.OnSolved -= lockCypherSolved;
            lockCypher.OnExitted -= lockCypherExitted;
            lockCypher.IsActive = false;
            LeanTween.move(Camera.main.gameObject, cameraPlaceholder.transform.position, cameraTransitionDuration);
            LeanTween.rotate(Camera.main.gameObject, cameraPlaceholder.transform.rotation.eulerAngles, cameraTransitionDuration);
        }

        private void lockCypherExitted()
        {
            Debug.Log("[CharacterHandler] Lock cypher exitted.");
            LevelHandler.Singleton.UpdateLockCipherActive(false);
            lockCypher.OnSolved -= lockCypherSolved;
            lockCypher.OnExitted -= lockCypherExitted;
            lockCypher.IsActive = false;
            LeanTween.move(Camera.main.gameObject, cameraPlaceholder.transform.position, cameraTransitionDuration);
            LeanTween.rotate(Camera.main.gameObject, cameraPlaceholder.transform.rotation.eulerAngles, cameraTransitionDuration);
        }
    }
}