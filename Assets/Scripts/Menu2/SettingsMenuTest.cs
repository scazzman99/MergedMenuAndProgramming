using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenuTest : MonoBehaviour {

    public AudioMixer mixer;
    Resolution[] resolutions;
    public Dropdown resolutionDropdown;
    public Light dirLight;

    private void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height; //gets all resolutions from List and imports their dimensions into a string and then into a dropMenu option
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height) //cant compare resolutions, must compare their components
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options); //add the list of options to the dropdown
        resolutionDropdown.value = currentResolutionIndex; //set the current position in dropdown to our current resolution
        resolutionDropdown.RefreshShownValue(); //refresh the dropdown to actually display the above
    }

    public void SetVolume(float volume) //take in volume amount from a slider
    {
        Debug.Log(volume); //between -80 and 0 as mixer use this range
        mixer.SetFloat("volume", volume); //string is the same as our mixer exposed param name. Will set to sliderVal. USE DYNAMIC FLOAT

    }

    public void SetQuality(int index) //gets index of Quality settings in project settings and uses it to set quality level
    {
        QualitySettings.SetQualityLevel(index); //hook up graphic dropdown to this function from inspector. USE DYNAMIC INT
    }

    public void SetFullScreen (bool isFullscreen) //DYNAMIC BOOL
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetBrightness(float brightness)
    {
        dirLight.intensity = brightness;
    }

    public void SetAmbientLighting(float ambient)
    {
        RenderSettings.ambientIntensity = ambient;
    }
}
