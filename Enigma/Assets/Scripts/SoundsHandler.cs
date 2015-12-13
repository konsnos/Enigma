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

    public void FadeOutAlarm()
    {
        LeanTween.value(this.gameObject, 1f, 0f, 1f).setOnUpdate(updateVolume).setOnComplete(stopSound);
    }

    private void stopSound()
    {
        alarm.Stop();
    }

    private void updateVolume(float value)
    {
        alarm.volume = value;
    }
}
