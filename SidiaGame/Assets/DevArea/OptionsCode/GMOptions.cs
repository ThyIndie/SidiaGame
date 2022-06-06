using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GMOptions : MonoBehaviour
{
    public Slider volume;


    private void Start()
    {
        if (!PlayerPrefs.HasKey("volume"))
        {
            PlayerPrefs.SetFloat("volume", 0.25f);
            volume.value = 0.25f;
            AudioListener.volume = 0.25f;
        }
        else
        {
            float isvolume = PlayerPrefs.GetFloat("volume");
            volume.value = isvolume;
            AudioListener.volume = isvolume;
        }
    }
    public void SetScreen(bool full)
    {
        if (full)
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        else
            Screen.fullScreenMode = FullScreenMode.Windowed;
    }
    public void setaudiovolume()
    {
        AudioListener.volume = volume.value;
        PlayerPrefs.SetFloat("volume", volume.value);
    }
    public void Leave()
    {
        Application.Quit();
    }
}
