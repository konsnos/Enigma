using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Enigma
{
    public class Inventory : MonoBehaviour
    {
        public static Inventory Singleton;

        public delegate void Refresh();
        public event Refresh OnAdded;
        public event Refresh Opened;
        public event Refresh Closed;

        [SerializeField]
        private GameObject contentParent;
        [SerializeField]
        private GameObject contentChild;
        /// <summary>
        /// Space between items in the inventory.
        /// </summary>
        [SerializeField]
        private int spaceBetweenItems = -8;
        [SerializeField]
        private int spriteHeight = -64;
        [SerializeField]
        private AudioSource takeItemSnd;

        /// <summary>
        /// Items owned by the character.
        /// </summary>
        private Hashtable items = new Hashtable();
        /// <summary>
        /// Game Objects added in inventory.
        /// </summary>
        private List<GameObject> itemContents;

        private bool isShown;

        public bool IsShown
        {
            get { return isShown; }
        }

        [SerializeField]
        private int openX = -120;
        [SerializeField]
        private float transitionSpeed = 0.4f;

        void Awake()
        {
            Singleton = this;

            isShown = false;

            itemContents = new List<GameObject>(10);
        }

        void Start()
        {
            closeInventory();
        }

        public void AddItem(Item newItem)
        {
            items.Add(newItem.Id, newItem);
            addSprite(newItem);

            takeItemSnd.Play();

            if (OnAdded != null)
                OnAdded();
        }

        public void RemoveItem(Item newItem)
        {
            items.Remove(newItem.Id);
            removeSprite(newItem);
        }

        private void addSprite(Item item)
        {
            GameObject newSprite = Instantiate(contentChild);
            newSprite.transform.SetParent(contentParent.transform, false);
            newSprite.GetComponent<Item>().SetItem(item.Id, item.Icon, item.PopUpMessage, item.PopUpSprite);
            newSprite.GetComponent<Image>().sprite = item.Icon;
            itemContents.Add(newSprite);

            arrangeAddedItems();
        }

        /// <summary>
        /// Removes item from the array of the inventory, and from the scene.
        /// </summary>
        /// <param name="item"></param>
        private void removeSprite(Item item)
        {
            foreach(GameObject existingSprite in itemContents)
            {
                if(existingSprite.GetComponent<Item>().Id == item.Id)
                {
                    Destroy(existingSprite);
                    itemContents.Remove(existingSprite);
                    break;
                }
            }

            arrangeAddedItems();
        }

        public bool UseItem(ItemIds.Item itemId)
        {
            if(items.ContainsKey(itemId))
            {
                Item tempItem = items[itemId] as Item;
                RemoveItem(tempItem);
                closeInventory();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Sorts items one under another.
        /// </summary>
        private void arrangeAddedItems()
        {
            int y = 0;
            for(int i = 0;i<itemContents.Count;i++)
            {
                Vector3 pos = itemContents[i].GetComponent<RectTransform>().localPosition;
                pos.y = y + spaceBetweenItems;
                itemContents[i].GetComponent<RectTransform>().localPosition = pos;
                y = y + spaceBetweenItems + spriteHeight;
            }

            Vector2 size = contentParent.GetComponent<RectTransform>().sizeDelta;
            size.y = Mathf.Abs(y + spaceBetweenItems);
            //contentParent.GetComponent<RectTransform>().position = new Vector3(-30f, 0f, 0f);
            contentParent.GetComponent<RectTransform>().sizeDelta = size;
        }

        public Hashtable GetItems()
        {
            return items.Clone() as Hashtable;
        }

        void Update()
        {
            if (!LevelHandler.Singleton.IsLockCipherActive)
            {
                if (Input.GetKeyUp(KeyCode.I))
                {
                    if (!isShown)
                        openInventory();
                    else
                        closeInventory();
                }
            }
        }

        /// <summary>
        /// Counts and returns enigma parts in the inventory.
        /// </summary>
        /// <returns></returns>
        public int GetAmountOfPartsOfEnigma()
        {
            int amount = 0;
            foreach(DictionaryEntry entry in items)
            {
                if ((entry.Value as Item).Id == ItemIds.Item.Enigma_Part1 || (entry.Value as Item).Id == ItemIds.Item.Enigma_Part2 || (entry.Value as Item).Id == ItemIds.Item.Enigma_Part1)
                    amount++;
            }

            return amount;
        }

        void openInventory()
        {
            LeanTween.moveX(this.gameObject.GetComponent<RectTransform>(), openX, transitionSpeed);

            isShown = true;

            if (Opened != null)
                Opened();
        }

        void closeInventory()
        {
            Debug.Log("[Inventory] Close inventory");
            LeanTween.moveX(this.gameObject.GetComponent<RectTransform>(), 0f, transitionSpeed);

            isShown = false;

            if (Closed != null)
                Closed();
        }
    }
}