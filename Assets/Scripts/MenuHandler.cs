using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; //for scene change interactivity
using UnityEngine.UI; // interact with UI elements
using UnityEngine.EventSystems; //for use of a controller (control events)
using System.Collections.Generic;

public class MenuHandler : MonoBehaviour
{
    //these function will be public because we need to be able to access them from the inspector later
    
    #region Variables
    [Header("Options")]
    public bool showOptions; //no defined value means this will default to FALSE
    public Resolution[] resolutions; //holds all resolutions
    public int resIndex; //index of the resolution array
    public bool isFullscreen; //is the game in fullScreen
    [Header("References")]
    public AudioSource mainAudio; //for our audio
    public GameObject mainMenu, optionsMenu; //declare 2 gameObjects for later definition (likely from the inspector)
    public Slider volSlider, brightSlider, ambientSlider;
    public Light dirLight; //for the brightness
    public Dropdown resDropdown; //dropdown for resolutions
    [Header("Keys")]
    public KeyCode holdingKey;
    public KeyCode forward, backward, left, right, jump, crouch, sprint, interact; //will remember the default keys
    [Header("KeyBind References")]
    public Text forwardText; //currently attached thru inspector
    public Text backwardText, leftText, rightText, jumpText, crouchText, sprintText, interactText; //make this an array or something later
    public Text[] inputTexts;

    
    #endregion

    private void Start()
    {
        mainAudio = GameObject.Find("MainMusic").GetComponent<AudioSource>(); //do the same but for audio source
        dirLight = GameObject.FindGameObjectWithTag("DirLight").GetComponent<Light>(); //etc but found a tag instead
       

        #region SetUpKeys
        forward = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Forward", "W")); //forward is where we will save the string. the second string must match KEYCODE!
        backward = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Backward", "S"));//first part of these converts a string to an Enum
        left = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Left", "A"));
        right = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Right", "D"));
        jump = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Jump", "Space"));
        crouch = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Crouch", "LeftControl"));
        sprint = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Sprint", "LeftShift"));
        interact = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Interact", "E"));
        
        
        #endregion

    }

    private void Update()
    {
        
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(1); //scene 1 is going to be our actual game scene
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif //this will quit the unity editor with the exit button. the # removes this line from the code's execution in a normal environment. THIS IS A DEBUG LINE

        Application.Quit(); //this should let you quit the game, but this will not quit the unity preview!
    }

    //for this next function we could just use a void but we want to practice long hand to adress boolean functions

    public void ToggleOptions()
    {
        OptionToggle();
    }
    bool OptionToggle() // doesnt need to be public or private
    {
        if (showOptions) // This is equivilant to showOptions == true
        {
            //if options is true we toggle it off and THEN set the mainMenu as the active menu and deactivate the optionsMenu!
            showOptions = false;
            mainMenu.SetActive(true);
            optionsMenu.SetActive(false);
            return true; //we will use this return value to let the method that called this function that we have completed this task.
        }
        else
        {
            //vice versa to the above
            showOptions = true;
            mainMenu.SetActive(false);
            optionsMenu.SetActive(true);
            volSlider = GameObject.Find("VolumeSlider").GetComponent<Slider>(); //volSlider is of slider type, so the gameobject return value must the Slider component of the retrieved GameObject!
            brightSlider = GameObject.Find("BrightnessSlider").GetComponent<Slider>();
            ambientSlider = GameObject.Find("AmbientSlider").GetComponent<Slider>();
            resDropdown = GameObject.Find("ResDropdown").GetComponent<Dropdown>();
            ResDropDownSetup();


            volSlider.value = mainAudio.volume; //value of the slider is equal to the audio level of mainAudio.
            brightSlider.value = dirLight.intensity;
            ambientSlider.value = RenderSettings.ambientIntensity;

            
            return false;
        }
        //additionally, we would maybe not return the value of showOptions (possibly on the off chance we are calling and returning a null value for a bool return type!)
    }

    public void Volume() //slider to control volume
    {
        mainAudio.volume = volSlider.value; //set the audio level to whatever the value of the slider is
    }

    public void Brightness()
    {
        dirLight.intensity = brightSlider.value;
    } //slider to control brightness

    public void AmbientBrightness()
    {
        RenderSettings.ambientIntensity = ambientSlider.value;
    }

    public void Resolutions()
    {
       
        Resolution currentRes = resolutions[resIndex];
        Screen.SetResolution(currentRes.width, currentRes.height, isFullscreen);
    }

    public void Save()
    {
        PlayerPrefs.SetString("Forward", forward.ToString()); //file name then the value it will hold!
        PlayerPrefs.SetString("Backward", backward.ToString());
        PlayerPrefs.SetString("Left", left.ToString());
        PlayerPrefs.SetString("Right", right.ToString());
        PlayerPrefs.SetString("Jump", jump.ToString());
        PlayerPrefs.SetString("Crouch", crouch.ToString());
        PlayerPrefs.SetString("Sprint", sprint.ToString());
        PlayerPrefs.SetString("Interact", interact.ToString());

    }

    public void ResDropDownSetup()
    {
        List<string> resOptions = new List<string>();
        resolutions = Screen.resolutions;
        resDropdown.ClearOptions();
        resIndex = 0;
        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            resOptions.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                resIndex = i;
            }
        }
        resDropdown.AddOptions(resOptions);
        resDropdown.value = resIndex;
        resDropdown.RefreshShownValue();

    }

    private void OnGUI() //make this better later
    {
        Event e = Event.current;
        if(forward == KeyCode.None)
        {
            Debug.Log("Keycode: " + e.keyCode);
            if(!(e.keyCode == backward || e.keyCode == left || e.keyCode == right || e.keyCode == jump || e.keyCode == crouch || e.keyCode == sprint || e.keyCode == interact))
            {
                forward = e.keyCode;
                holdingKey = KeyCode.None;
                forwardText.text = forward.ToString();
                
            }
        }
    }

    public void Forward()
    {
        if(!(backward == KeyCode.None || left == KeyCode.None) || right == KeyCode.None || jump == KeyCode.None || crouch == KeyCode.None ||
            sprint == KeyCode.None || interact == KeyCode.None)
        {
            holdingKey = forward;
           forward = KeyCode.None;
           forwardText.text = forward.ToString();
        }
    }

    public void FullscreenRedun()
    {
        isFullscreen = !isFullscreen;
        Screen.fullScreen = isFullscreen;
        Debug.Log(isFullscreen);
    }

    public void Fullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
