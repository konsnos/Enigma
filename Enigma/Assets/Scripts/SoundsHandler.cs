using UnityEngine;
using System.Collections;

public class SoundsHandler : MonoBehaviour 
{
    [SerializeField]
    private AudioSource bg;
    [SerializeField]
    private AudioSource alarm;

    public void EnableAlarm()
    {
        bg.Stop();
        Invoke("playAlarm", 2f);
    }

    private void playAlarm()
    {
        alarm.Play();
    }
}
