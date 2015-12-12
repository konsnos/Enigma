using UnityEngine;
using System.Collections;
using Enigma.UserInterface;
using Enigma.MiniGames;

namespace Enigma
{
    public class CharacterHandler : MonoBehaviour
    {
        bool mouseAction;

        [SerializeField]
        private float cameraTransitionDuration = 0.5f;
        [SerializeField]
        private GameObject cameraPlaceholder;

        private LockCypher lockCypher;
        
        void Awake()
        {
            mouseAction = false;
        }

        void Update()
        {
            if(!LevelHandler.Singleton.IsLockCypherActive && Input.GetMouseButton(0) && !mouseAction)
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
                else
                {
                    lockCypher = hit.transform.GetComponent<LockCypher>();

                    if(lockCypher)
                    {
                        lockCypher.IsActive = true;
                        LevelHandler.Singleton.IsLockCypherActive = true;
                        // disable character movement, disable cursor, disable inventory.
                        LeanTween.move(Camera.main.gameObject, lockCypher.CamPlaceHolder.transform.position, cameraTransitionDuration);
                        LeanTween.rotate(Camera.main.gameObject, lockCypher.CamPlaceHolder.transform.rotation.eulerAngles, cameraTransitionDuration);
                        lockCypher.OnSolved += lockCypherSolved;
                        lockCypher.OnExitted += lockCypherExitted;
                    }
                }
            }
        }

        private void lockCypherSolved()
        {
            Debug.Log("[CharacterHandler] Lock cypher solved.");
            LevelHandler.Singleton.IsLockCypherActive = false;
            lockCypher.OnSolved -= lockCypherSolved;
            lockCypher.OnExitted -= lockCypherExitted;
            lockCypher.IsActive = false;
            LeanTween.move(Camera.main.gameObject, cameraPlaceholder.transform.position, cameraTransitionDuration);
            LeanTween.rotate(Camera.main.gameObject, cameraPlaceholder.transform.rotation.eulerAngles, cameraTransitionDuration);
        }

        private void lockCypherExitted()
        {
            Debug.Log("[CharacterHandler] Lock cypher exitted.");
            LevelHandler.Singleton.IsLockCypherActive = false;
            lockCypher.OnSolved -= lockCypherSolved;
            lockCypher.OnExitted -= lockCypherExitted;
            lockCypher.IsActive = false;
            LeanTween.move(Camera.main.gameObject, cameraPlaceholder.transform.position, cameraTransitionDuration);
            LeanTween.rotate(Camera.main.gameObject, cameraPlaceholder.transform.rotation.eulerAngles, cameraTransitionDuration);
        }
    }
}