using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
using Enigma;
using System.Collections.Generic;

public class LevelHandler : MonoBehaviour 
{
    FirstPersonController fpsController;
    public List<GameObject> itemToAdd;

	void Awake ()
    {
        fpsController = GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>();
	}

    void Start()
    {
        Inventory.Singleton.Opened += InventoryOpened;
        Inventory.Singleton.Closed += InventoryClosed;

        for (int i = 0; i < itemToAdd.Count;i++ )
            Inventory.Singleton.AddItem((itemToAdd[i] as GameObject).GetComponent<Item>());
    }

    private void InventoryOpened()
    {
        fpsController.IsActive = false;
    }

    private void InventoryClosed()
    {
        fpsController.IsActive = true;
    }
}