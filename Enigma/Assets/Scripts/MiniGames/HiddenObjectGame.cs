using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using Enigma.Sound;

namespace Enigma.MiniGames
{
    public class HiddenObjectGame : MiniGameBase
    {
        public event Refresh OnExitted;
        public event Refresh OnSolved;

        /// <summary>
        /// When true the mini game is active.
        /// </summary>
        private bool isActive;

        public bool IsActive
        {
            get { return isActive; }
            set 
            { 
                isActive = value;
                if(isActive)
                {
                    delayedStart();
                }

                panel.SetActive(isActive);
            }
        }

        private bool gameStarted;
        float startTS;
        /// <summary>
        /// Game duration in seconds.
        /// </summary>
        [SerializeField]
        float gameDuration = 20f;
        [SerializeField]
        float wrongPickPenalty = 1f;

        [SerializeField]
        private HiddenObjectItemSlot[] hiddenObject;

        /***** UI *********/
        [SerializeField]
        private GameObject panel;
        [SerializeField]
        private Checklist[] checklist;
        /// <summary>
        /// Timer to update.
        /// </summary>
        [SerializeField]
        private Text timer;

        /// <summary>
        /// Position of the camera when this minigame is played.
        /// </summary>
        [SerializeField]
        private GameObject camPlaceHolder;
        [SerializeField]
        private Light light;

        private void calculateTime()
        {
            float remainingTime = gameDuration - (Time.time - startTS);

            int seconds = Mathf.FloorToInt(remainingTime);
            int milli = Mathf.FloorToInt((remainingTime - seconds) * 100);
            timer.text = (seconds > 9 ? seconds.ToString() : "0" + seconds.ToString()) + ":" + (milli > 9 ? milli.ToString() : "0" + milli.ToString());
        }

        private void returnTimerToWhite()
        {
            timer.color = Color.white;
        }

        void Update()
        {
            if (isActive)
            {
                if (gameStarted)
                {
                    calculateTime();

                    // check hits
                    if (Input.GetMouseButtonUp(0))
                    {
                        RaycastHit[] hits = CharacterHandler.GetHits(Input.mousePosition, 1 << Layers.HiddenObjectIntereaction, 10f);
                        HiddenObjectItemSlot itemSlot;
                        foreach (RaycastHit hit in hits)
                        {
                            Debug.Log("[HiddenObjectGame] Hit " + hit.transform.gameObject.name);
                            itemSlot = hit.transform.gameObject.GetComponent<HiddenObjectItemSlot>();
                            Debug.Log("[HiddenObjectGame] Hit id " + itemSlot.Id);
                            if (itemSlot)
                            {
                                bool found = false;
                                for (int i = 0; i < checklist.Length; i++)
                                {
                                    if (checklist[i].id == itemSlot.Id)
                                    {
                                        Debug.Log("[HiddenObjectGame] Checklist found " + checklist[i].id);
                                        checklist[i].ItemFound();
                                        itemSlot.Remove();
                                        Click.Singleton.PlaySound();
                                        found = true;
                                        break;
                                    }
                                }
                                if (found)
                                {
                                    if (checkIfSolved())
                                    {
                                        gameEnded();
                                        isSolved = true;
                                        if (OnSolved != null)
                                            OnSolved();
                                    }
                                }
                                else
                                {
                                    startTS -= wrongPickPenalty;
                                    timer.color = Color.red;
                                    CancelInvoke("returnTimerToWhite");
                                    Invoke("returnTimerToWhite", 0.5f);
                                    ClickWrong.Singleton.PlaySound();
                                }
                                break;
                            }
                        }
                    }

                    if ((Time.time - startTS) >= gameDuration)
                    {
                        gameEnded();
                    }
                }
                else
                {
                    if (Input.GetMouseButtonUp(0))
                    {
                        RaycastHit[] hits = CharacterHandler.GetHits(Input.mousePosition, 1 << Layers.Interaction, 10f);
                        foreach (RaycastHit hit in hits)
                        {
                            if (hit.transform.gameObject.name == "SM_Rope")
                                startGame();
                        }
                    }
                }

                if (Input.GetKeyUp(KeyCode.Escape))
                {
                    gameEnded();
                    if (OnExitted != null)
                        OnExitted();
                }
            }
        }

        /// <summary>
        /// Checks if mini game is solved.
        /// </summary>
        /// <returns>True if mini game is solved.</returns>
        private bool checkIfSolved()
        {
            for(int i = 0;i<checklist.Length;i++)
            {
                if (!checklist[i].Found)
                    return false;
            }
            return true;
        }

        private void gameEnded()
        {
            gameStarted = false;
            timer.text = "00:00";
            light.enabled = false;
        }

        private void delayedStart()
        {
            Invoke("startGame", 0.5f); // Time is the transition duration to place the camera into position. Next time the mini game begins
        }

        private void startGame()
        {
            gameStarted = true;
            startTS = Time.time;
            light.enabled = true;

            List<ItemSlotIds.Item> itemsAvailableToFind = new List<ItemSlotIds.Item>() { ItemSlotIds.Item.Flashlight, ItemSlotIds.Item.Battery_Yellow, ItemSlotIds.Item.Battery_Blue, ItemSlotIds.Item.Battery_Red, ItemSlotIds.Item.Folder, ItemSlotIds.Item.Book_Blue };
            checklist[0].ChangeName(ItemSlotIds.Item.Enigma_Buttons);
            int amountOfItemsToFind = 4;
            for(int i = 1;i<amountOfItemsToFind;i++)
            {
                int randomIndex = Random.Range(0, itemsAvailableToFind.Count - 1);
                checklist[i].ChangeName(itemsAvailableToFind[randomIndex]);
                itemsAvailableToFind.RemoveAt(randomIndex);
            }
        }

        public Vector3 GetCamPosition()
        {
            return camPlaceHolder.transform.position;
        }

        public Quaternion GetCamRotation()
        {
            return camPlaceHolder.transform.rotation;
        }
    }
}