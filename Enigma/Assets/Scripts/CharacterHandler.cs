using UnityEngine;
using System.Collections;
using Enigma.UserInterface;
using Enigma.MiniGames;
using Enigma.Sound;

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
        private HiddenObjectGame hiddenObjectGame;
        [SerializeField]
        private Item itemEarnedFromHiddenObjectGame;
        [SerializeField]
        private GameObject flashlightGo;
        private Light flashlightLight;
        private AudioSource flashlightSnd;

        private bool flashlightOn;

        private static bool ignoreMousebtn;

        void Awake()
        {
            ignoreMousebtn = false;
            flashlightOn = false;

            flashlightLight = flashlightGo.GetComponent<Light>();
            flashlightSnd = flashlightGo.GetComponent<AudioSource>();
        }

        void Update()
        {
            if (ignoreMousebtn)
                ignoreMousebtn = false;
            else
            {
                if (!Inventory.Singleton.IsShown && !UIHandler.Singleton.PopUpIsOpen && !LevelHandler.Singleton.IsMiniGameActive)
                {
                    if (Input.GetMouseButtonUp(0))
                    {
                        raycastForInteraction();
                        Click.Singleton.PlaySound();
                    }
                    else if (Input.GetKeyUp(KeyCode.F))
                    {
                        if (Inventory.Singleton.ItemExists(ItemIds.Item.Flashlight))
                        {
                            flashlightOn = !flashlightOn;
                            flashlightLight.enabled = flashlightOn;
                            flashlightSnd.Play();
                        }
                    }
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

        public static RaycastHit[] GetHits(Vector3 screenPoint, LayerMask mask, float range)
        {
            RaycastHit[] hits;
            Ray ray = Camera.main.ScreenPointToRay(screenPoint);

            hits = Physics.RaycastAll(ray, range, mask);
            Debug.Log("[CharacterHandler] Hits " + hits.Length);
            return hits;
        }

        private void showMissingPartPopUp(Item tempItem)
        {
            int enigmaParts = Inventory.Singleton.GetAmountOfPartsOfEnigma();
            switch (enigmaParts)
            {
                case 1:
                    UIHandler.Singleton.ShowPopUp("I found a missing part! Good!", tempItem.Icon);
                    break;
                case 2:
                    UIHandler.Singleton.ShowPopUp("One more piece to go! Great!", tempItem.Icon);
                    Invoke("makeAlarm", Random.Range(3f, 5f));
                    break;
                case 3:
                    UIHandler.Singleton.ShowPopUp("I have all parts now! Let’s take the machine...", tempItem.Icon);
                    break;
                default:
                    break;
            }
        }

        private void raycastForInteraction()
        {
            RaycastHit[] hits = GetHits(new Vector3(Screen.width / 2, Screen.height / 2, 0f), 1 << Layers.Interaction, 3f);
            Item tempItem;
            foreach(RaycastHit hit in hits)
            {
                Debug.Log("[CharacterHandler] hit object " + hit.transform.gameObject.name);
                tempItem = hit.transform.GetComponent<Item>();
                if(tempItem)
                {
                    Inventory.Singleton.AddItem(tempItem);
                    if (tempItem.PopUpMessage != "" || tempItem.Icon != null)
                    {
                        if (tempItem.Id == ItemIds.Item.Enigma_Part1 || tempItem.Id == ItemIds.Item.Enigma_Part2 || tempItem.Id == ItemIds.Item.Enigma_Part3)
                        {
                            showMissingPartPopUp(tempItem);
                        }
                        else
                        {
                            UIHandler.Singleton.ShowPopUp(tempItem.PopUpMessage, tempItem.Icon);
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
                        LevelHandler.Singleton.UpdateMiniGameActive(true);
                        LevelHandler.Singleton.cypherActive = true;
                        // disable character movement, disable cursor, disable inventory.
                        LeanTween.move(Camera.main.gameObject, lockCypher.GetCamPosition(), cameraTransitionDuration);
                        LeanTween.rotate(Camera.main.gameObject, lockCypher.GetCamRotation().eulerAngles, cameraTransitionDuration);
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
                                {
                                    Debug.Log("[CharacterController] Game won!!");
                                    if(OnGameWon != null)
                                        OnGameWon();
                                } 
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
                            case "SM_SteelDoor":
                                UIHandler.Singleton.ShowPopUp("I have to take the machine first... It’s important.", null);
                                return;
                            case "SM_Window":
                                initiateHiddenObjectGame(hit.transform.gameObject);
                                return;
                            default:
                                break;
                        }
                    }
                    
                }
            }
        }

        private void initiateHiddenObjectGame(GameObject go)
        {
            hiddenObjectGame = go.GetComponent<HiddenObjectGame>();

            if (hiddenObjectGame)
            {
                if (!hiddenObjectGame.IsSolved)
                {
                    hiddenObjectGame.IsActive = true;
                    LevelHandler.Singleton.UpdateMiniGameActive(true);
                    LevelHandler.Singleton.hiddenObjectGameActive = true;
                    // disable character movement, disable cursor, disable inventory.
                    LeanTween.move(Camera.main.gameObject, hiddenObjectGame.GetCamPosition(), cameraTransitionDuration);
                    LeanTween.rotate(Camera.main.gameObject, hiddenObjectGame.GetCamRotation().eulerAngles, cameraTransitionDuration);
                    hiddenObjectGame.OnSolved += hiddenObjectGameSolved;
                    hiddenObjectGame.OnExitted += hiddenObjectGameExitted;
                }
            }
        }

        private void lockCypherSolved()
        {
            Debug.Log("[CharacterHandler] Lock cypher solved.");
            LevelHandler.Singleton.UpdateMiniGameActive(false);
            LevelHandler.Singleton.cypherActive = false;
            lockCypher.OnSolved -= lockCypherSolved;
            lockCypher.OnExitted -= lockCypherExitted;
            lockCypher.IsActive = false;
            LeanTween.move(Camera.main.gameObject, cameraPlaceholder.transform.position, cameraTransitionDuration);
            LeanTween.rotate(Camera.main.gameObject, cameraPlaceholder.transform.rotation.eulerAngles, cameraTransitionDuration);
        }

        private void lockCypherExitted()
        {
            Debug.Log("[CharacterHandler] Lock cypher exitted.");
            LevelHandler.Singleton.UpdateMiniGameActive(false);
            LevelHandler.Singleton.cypherActive = false;
            lockCypher.OnSolved -= lockCypherSolved;
            lockCypher.OnExitted -= lockCypherExitted;
            lockCypher.IsActive = false;
            LeanTween.move(Camera.main.gameObject, cameraPlaceholder.transform.position, cameraTransitionDuration);
            LeanTween.rotate(Camera.main.gameObject, cameraPlaceholder.transform.rotation.eulerAngles, cameraTransitionDuration);
        }

        private void hiddenObjectGameSolved()
        {
            Debug.Log("[CharacterHandler] Hidden object game solved.");
            LevelHandler.Singleton.UpdateMiniGameActive(false);
            LevelHandler.Singleton.hiddenObjectGameActive = false;
            hiddenObjectGame.OnSolved -= lockCypherSolved;
            hiddenObjectGame.OnExitted -= lockCypherExitted;
            hiddenObjectGame.IsActive = false;
            LeanTween.move(Camera.main.gameObject, cameraPlaceholder.transform.position, cameraTransitionDuration);
            LeanTween.rotate(Camera.main.gameObject, cameraPlaceholder.transform.rotation.eulerAngles, cameraTransitionDuration);

            Inventory.Singleton.AddItem(itemEarnedFromHiddenObjectGame);
            showMissingPartPopUp(itemEarnedFromHiddenObjectGame);
        }

        private void hiddenObjectGameExitted()
        {
            Debug.Log("[CharacterHandler] Lock cypher exitted.");
            LevelHandler.Singleton.UpdateMiniGameActive(false);
            LevelHandler.Singleton.hiddenObjectGameActive = false;
            hiddenObjectGame.OnSolved -= lockCypherSolved;
            hiddenObjectGame.OnExitted -= lockCypherExitted;
            hiddenObjectGame.IsActive = false;
            LeanTween.move(Camera.main.gameObject, cameraPlaceholder.transform.position, cameraTransitionDuration);
            LeanTween.rotate(Camera.main.gameObject, cameraPlaceholder.transform.rotation.eulerAngles, cameraTransitionDuration);
        }
    }
}