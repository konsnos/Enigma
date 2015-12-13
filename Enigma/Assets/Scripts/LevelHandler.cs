using Enigma;
using Enigma.UserInterface;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

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
    private bool moviePlaying;
    public bool MoviePlaying
    {
        get { return moviePlaying; }
    }
    [SerializeField]
    private bool enableAlarm = false;
    [SerializeField]
    private bool enableEnd = false;
    [SerializeField]
    private bool skipIntro = false;

    [SerializeField]
    private RawImage movImage;
    [SerializeField]
    private AudioSource movAudioSrc;
    [SerializeField]
    private MovieTexture introMov;
    [SerializeField]
    private AudioClip introAudioClip;
    [SerializeField]
    private MovieTexture outroMov;
    [SerializeField]
    private AudioClip outroAudioClip;

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
        CharacterHandler.Singleton.OnGameWon += playOutroVideo;

        for (int i = 0; i < itemToAdd.Count;i++ )
            Inventory.Singleton.AddItem((itemToAdd[i] as GameObject).GetComponent<Item>());

        if(enableAlarm)
            Invoke("EnableAlarm", 3f);
        if(enableEnd)
            Invoke("playOutroVideo", 8f);

        if(skipIntro)
            movOnFadedOut();
        else
        {
            movImage.texture = introMov;
            movAudioSrc.clip = introAudioClip;
            introMov.Play();
            movAudioSrc.Play();
            moviePlaying = true;
            Invoke("fadeOutMov", introAudioClip.length);
        }

        updateCursor();
        fpsController.IsActive = false;
        helpTxt.SetActive(true);
    }

    private void playOutroVideo()
    {
        Debug.Log("[LevelHandler] playing outro video.");

        soundsHandler.FadeOutAlarm();

        fadeInMov();
        movImage.texture = outroMov;
        movAudioSrc.clip = outroAudioClip;
        outroMov.Play();
        movAudioSrc.Play();
        moviePlaying = true;
        updateCursor();

        Invoke("backToMenu", outroAudioClip.length);
    }

    private void backToMenu()
    {
        SceneManager.LoadScene("menu");
    }

    void fadeInMov()
    {
        movImage.gameObject.SetActive(true);
        LeanTween.value(movImage.gameObject, 0f, 1f, 0.5f).setOnUpdate(movOnUpdateFade);
    }

    void fadeOutMov()
    {
        LeanTween.value(movImage.gameObject, 1f, 0f, 0.5f).setOnUpdate(movOnUpdateFade).setOnComplete(movOnFadedOut);
    }

    void movOnUpdateFade(float value)
    {
        movImage.color = new Color(1f, 1f, 1f, value);
    }

    void movOnFadedOut()
    {
        movImage.gameObject.SetActive(false);
        moviePlaying = false;
        updateCursor();
    }

    void Update()
    {
        if (!moviePlaying && helpTxt.activeSelf && Input.anyKeyDown)
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
        Debug.Log("Movie playing " + moviePlaying + ", Inventory " + Inventory.Singleton.IsShown + ", Panel active " + UIHandler.Singleton.IsPanelActive() + ", in mini game "  + isMiniGameActive);
        if(moviePlaying)
        {
            Cursor.visible = false;
            fpsController.IsActive = false;
            UIHandler.Singleton.SetCrosshairActive(false);
        }
        else if (Inventory.Singleton.IsShown || UIHandler.Singleton.IsPanelActive())
        {
            Cursor.visible = true;
            fpsController.IsActive = false;
            UIHandler.Singleton.SetCrosshairActive(false);
        }
        else if(isMiniGameActive)
        {
            if (cypherActive)
                Cursor.visible = false;
            else
                Cursor.visible = true;
            fpsController.IsActive = false;
            UIHandler.Singleton.SetCrosshairActive(false);
        }
        else
        {
            Cursor.visible = false;
            fpsController.IsActive = true;
            UIHandler.Singleton.SetCrosshairActive(true);
        }
    }
}