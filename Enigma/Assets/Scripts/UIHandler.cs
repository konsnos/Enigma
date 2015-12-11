using UnityEngine;
using System.Collections;

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

    void Awake()
    {
        Singleton = this;
    }

    public void ShowPopUp()
    {
        PopUpPanel.SetActive(true);

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
