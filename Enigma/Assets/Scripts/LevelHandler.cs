using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
using Enigma;
using Enigma.UserInterface;
using System.Collections.Generic;

public class LevelHandler : MonoBehaviour 
{
    public static LevelHandler Singleton;

    FirstPersonController fpsController;
    /// <summary>
    /// Used to dynamically add items to the inventory.
    /// </summary>
    public List<GameObject> itemToAdd;

    [SerializeField]
    private LightsHandler lightsHandler;
    [SerializeField]
    private SoundsHandler soundsHandler;

    [SerializeField]
    private GameObject helpTxt;

    private bool isMiniGameActive;
    public bool cypherActive;
    public bool hiddenObjectGameActive;

    public bool IsMiniGameActive
    {
        get { return isMiniGameActive; }
    }

    public void UpdateMiniGameActive(bool value)
    {
        isMiniGameActive = value;
        updateCursor();
    }

	void Awake ()
    {
        Singleton = this;
        isMiniGameActive = false;
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

        updateCursor();
        fpsController.IsActive = false;
        helpTxt.SetActive(true);

        //Invoke("EnableAlarm", 3f);
    }

    void Update()
    {
        if (helpTxt.activeSelf && Input.anyKeyDown)
        {
            helpTxt.SetActive(false);
            fpsController.IsActive = true;
        }
    }

    /// <summary>
    /// Play lights and sounds.
    /// </summary>
    public void EnableAlarm()
    {
        lightsHandler.EnableAlarms();
        soundsHandler.EnableAlarm();
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
        Debug.Log("[LevelHandler] Panel activated.");
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
        else if(isMiniGameActive)
        {
            if (cypherActive)
                Cursor.visible = false;
            else
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