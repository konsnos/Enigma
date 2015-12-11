using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
using Enigma;
using System.Collections.Generic;

public class LevelHandler : MonoBehaviour 
{
    FirstPersonController fpsController;
    /// <summary>
    /// Used to dynamically add items to the inventory.
    /// </summary>
    public List<GameObject> itemToAdd;

	void Awake ()
    {
        fpsController = GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>();
	}

    void Start()
    {
        Inventory.Singleton.Opened += InventoryOpened;
        Inventory.Singleton.Closed += InventoryClosed;
        UIHandler.Singleton.OnShow += PanelActivated;
        UIHandler.Singleton.OnHide += PanelDeactivated;

        for (int i = 0; i < itemToAdd.Count;i++ )
            Inventory.Singleton.AddItem((itemToAdd[i] as GameObject).GetComponent<Item>());
    }

    private void InventoryOpened()
    {
        updateCursor();
    }

    private void InventoryClosed()
    {
        updateCursor();
    }

    private void PanelActivated()
    {
        updateCursor();
    }

    private void PanelDeactivated()
    {
        updateCursor();
    }

    private void updateCursor()
    {
        Debug.Log("Inventory " + Inventory.Singleton.IsShown + ", Panel active " + UIHandler.Singleton.IsPanelActive());
        if (Inventory.Singleton.IsShown || UIHandler.Singleton.IsPanelActive())
        {
            Cursor.visible = true;
            fpsController.IsActive = false;
        }
        else
        {
            Cursor.visible = false;
            fpsController.IsActive = true;
        }
    }
}