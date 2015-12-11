using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

    void Awake()
    {
        Singleton = this;
    }

    public void ShowPopUp(string message, Sprite image)
    {
        PopUpPanel.SetActive(true);

        popUpText.text = message;
        if (image != null)
        {
            popUpImg.sprite = image;
            popUpImg.SetNativeSize();
        }

        if (OnShow != null)
            OnShow();
    }

    public void HidePopUp()
    {
        PopUpPanel.SetActive(false);

        if (OnHide != null)
            OnHide();
    }

    /// <summary>
    /// Checks if the game object panel is active.
    /// </summary>
    /// <returns>True if the panel is active.</returns>
    public bool IsPanelActive()
    {
        return PopUpPanel.activeSelf;
    }
}
