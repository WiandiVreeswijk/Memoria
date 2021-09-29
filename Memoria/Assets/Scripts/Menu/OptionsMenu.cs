using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour {

    private string masterBusString = "Bus:/";
    private FMOD.Studio.Bus masterBus;

    Resolution[] resolutions;
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown graphicsDropdown;

    private void Start()
    {
        masterBus = FMODUnity.RuntimeManager.GetBus(masterBusString);

        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        graphicsDropdown.value = 5;


    }

    public void SetVolume(float volume)
    {
        masterBus.setVolume(volume);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(int screenTypeIndex)
    {
        if(screenTypeIndex == 0)
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }else if(screenTypeIndex == 1)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }else if(screenTypeIndex == 2)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
    }
}
