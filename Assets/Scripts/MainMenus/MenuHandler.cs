using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; //for scene change interactivity
using UnityEngine.UI; // interact with UI elements
using UnityEngine.EventSystems; //for use of a controller (control events)
using System.Collections.Generic; //needed to use lists well
using System.Xml.Serialization;
using System.Xml;
using System.IO;

public class OptionsData
{
    public float volSlider, brightSlider, ambientSlider;
    public KeyCode forward, backward, left, right, jump, crouch, sprint, interact;
    public bool fullScreen;
}
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
    public Dictionary<string, KeyCode> myKeyCodes;
    public List<KeyCode> keyCodeVals;
    public Text[] buttonTexts;
    [Header("SavingStuff")]
    public OptionsData data = new OptionsData();
    private string filePath, fileName = "Game Data";

    #endregion

    private void Awake()
    {
        mainAudio = GameObject.Find("MainMusic").GetComponent<AudioSource>(); //do the same but for audio source
        dirLight = GameObject.FindGameObjectWithTag("DirLight").GetComponent<Light>(); //etc but found a tag instead
        filePath = Application.dataPath + "/Data/" + fileName + ".xml";
        if (File.Exists(filePath))
        {
            Load();
        }
        else
        {
            forward = KeyCode.W;
            backward = KeyCode.S;
            left = KeyCode.A;
            right = KeyCode.D;
            jump = KeyCode.Space;
            crouch = KeyCode.LeftControl;
            sprint = KeyCode.LeftShift;
            interact = KeyCode.E;
            
        }
    }
    private void Start()
    {
        
        
        myKeyCodes = new Dictionary<string, KeyCode>();
        

        #region SetUpKeys

        

        keyCodeVals.Add(forward);
        keyCodeVals.Add(backward);
        keyCodeVals.Add(left);
        keyCodeVals.Add(right);
        keyCodeVals.Add(jump);
        keyCodeVals.Add(crouch);
        keyCodeVals.Add(sprint);
        keyCodeVals.Add(interact);
        Debug.Log(keyCodeVals.Capacity);
        Debug.Log(keyCodeVals[0]);

        for(int i = 0; i < keyCodeVals.Capacity; i++)
        {
            buttonTexts[i].text = keyCodeVals[i].ToString(); //intialises the buttons text as the loaded controls as they are the same length
        }

        //These dictionary keys will match the gameobject name of the button for reference in GetButtonName()
        
        
        myKeyCodes["ForwardButton"] = keyCodeVals[0];
        myKeyCodes["BackwardButton"] = keyCodeVals[1];
        myKeyCodes["LeftButton"] = keyCodeVals[2];
        myKeyCodes["RightButton"] = keyCodeVals[3];
        myKeyCodes["JumpButton"] = keyCodeVals[4];
        myKeyCodes["CrouchButton"] = keyCodeVals[5];
        myKeyCodes["SprintButton"] = keyCodeVals[6];
        myKeyCodes["InteractButton"] = keyCodeVals[7];
        


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

    #region OptionsFunctions

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


    #region ResolutionFunctions

    //gets all resolutions monitor supports and adds them to a list. Sets index of list to current res
    public void ResDropDownSetup()
    {
        List<string> resOptions = new List<string>();
        resolutions = Screen.resolutions;
        resDropdown.ClearOptions();
        resIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            resOptions.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                resIndex = i;
            }
        }
        resDropdown.AddOptions(resOptions);
        resDropdown.value = resIndex;
        resDropdown.RefreshShownValue();

    }

    //gets index from dropdown and sets our current resolution to the dropdowns
    public void Resolutions()
    {

        Resolution currentRes = resolutions[resIndex];
        Screen.SetResolution(currentRes.width, currentRes.height, isFullscreen);
    }

    #endregion

    #region KeyBind Functions

    //gets the name of the button pushed and sends the keycode of that button to SetKey
    public void GetButtonName()
    {
        string buttonName = EventSystem.current.currentSelectedGameObject.name; //grabs the name of the button that called this function. Button will selected game object in EventSys. This is how to get the name of the thing that called a button
        Debug.Log(buttonName);
        if (buttonName != null && myKeyCodes.ContainsKey(buttonName)) //if the button name is in the dictionary as a key
        {
            KeyCode keyBind = myKeyCodes[buttonName]; //set a KeyCode to the dictionary value of the button name
            Debug.Log(keyBind);
            SetKey(keyBind);

        }
    }

    //empties the keycode of button from GetButtonName() into the holdingKey and sets keycode to none for OnGUI to handle
    void SetKey(KeyCode keyBind)
    {
        int index = -1; //use this to see if we picked anything up
        for (int i = 0; i < keyCodeVals.Capacity; i++)
        {
            if (keyBind == keyCodeVals[i]) //if keybind is the same as keycode at index i
            {
                index = i; //set index to i
                break; //leave the for loop
            }
        }

        if (index != -1) //if we found anything earlier
        {
            if (!keyCodeVals.Contains(KeyCode.None)) //if the list of keycodes DOES NOT contain keycode None
            {
                holdingKey = keyCodeVals[index]; //set holding key to our keycode
                keyCodeVals[index] = KeyCode.None; //set our key code in the list to none
                buttonTexts[index].text = keyCodeVals[index].ToString(); //set the button text to the string of our new keycode
                
            }
        }
    }

    //reinitialises all keybinds and associated structures to the default state
    public void DefaultKeyBinds()
    {
        

        keyCodeVals[0] = KeyCode.W;
        keyCodeVals[1] = KeyCode.S;
        keyCodeVals[2] = KeyCode.A;
        keyCodeVals[3] = KeyCode.S;
        keyCodeVals[4] = KeyCode.Space;
        keyCodeVals[5] = KeyCode.LeftControl;
        keyCodeVals[6] = KeyCode.LeftShift;
        keyCodeVals[7] = KeyCode.E;

        RefreshDictionary();

        for (int i = 0; i < keyCodeVals.Capacity; i++)
        {
            buttonTexts[i].text = keyCodeVals[i].ToString(); //intialises the buttons text as the loaded controls as they are the same length
        }

        //save data
        Save();

    }

    //saves custom keybinds & OPTIONS

    public void Save()
    {
        

        data.volSlider = volSlider.value;
        data.brightSlider = brightSlider.value;
        data.ambientSlider = ambientSlider.value;
        data.forward = myKeyCodes["ForwardButton"];
        data.backward = myKeyCodes["BackwardButton"];
        data.left = myKeyCodes["LeftButton"];
        data.right = myKeyCodes["RightButton"];
        data.jump = myKeyCodes["JumpButton"];
        data.crouch = myKeyCodes["CrouchButton"];
        data.sprint = myKeyCodes["SprintButton"];
        data.interact = myKeyCodes["InteractButton"];

        var serializer = new XmlSerializer(typeof(OptionsData));

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            serializer.Serialize(stream, data);
        }
    }

    public void Load()
    {
        var serializer = new XmlSerializer(typeof(OptionsData));

        using (var stream = new FileStream(filePath, FileMode.Open))
        {

            data = serializer.Deserialize(stream) as OptionsData;
        }

        forward = data.forward;
        backward = data.backward;
        left = data.left;
        right = data.right;
        jump = data.jump;
        crouch = data.crouch;
        sprint = data.sprint;
        interact = data.interact;

        mainAudio.volume = data.volSlider;
        dirLight.intensity = data.brightSlider;
        RenderSettings.ambientIntensity = data.ambientSlider;
        Screen.fullScreen = data.fullScreen;

    }

    private void OnGUI() //make this better later
    {
        Event e = Event.current;


        if (e.isKey) //if the event is a key
        {
            for (int i = 0; i < keyCodeVals.Capacity; i++) //cycle thru list of keyBinds
            {
                if (keyCodeVals[i] == KeyCode.None) //if the key bind is equal to nothing
                {
                    Debug.Log("Keycode: " + e.keyCode);
                    if (!keyCodeVals.Contains(e.keyCode)) //if there are NO other keybinds with our event keycode
                    {
                        keyCodeVals[i] = e.keyCode; //set the keycode to the event keycode
                        holdingKey = KeyCode.None; //set the holding key to nothing
                        buttonTexts[i].text = keyCodeVals[i].ToString(); //make the buttons text the string value of our current keycode
                        RefreshDictionary();
                    }
                    else
                    {
                        keyCodeVals[i] = holdingKey;
                        holdingKey = KeyCode.None;
                        buttonTexts[i].text = keyCodeVals[i].ToString();
                    }
                    Debug.Log(myKeyCodes["ForwardButton"] + " from loop");
                    break;
                }
            }
        }

    }

    #endregion

    #region FullScreenFunctions

    //alt version of fullscreen()
    public void FullscreenRedun()
    {
        isFullscreen = !isFullscreen;
        Screen.fullScreen = isFullscreen;
        Debug.Log(isFullscreen);
    }

    //changes fullscreen status using dynamic bool
    public void Fullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        
    }

    #endregion

    #endregion

    //refreshes the dictionary values if the list has changed
    void RefreshDictionary()
    {
        myKeyCodes["ForwardButton"] = keyCodeVals[0];
        myKeyCodes["BackwardButton"] = keyCodeVals[1];
        myKeyCodes["LeftButton"] = keyCodeVals[2];
        myKeyCodes["RightButton"] = keyCodeVals[3];
        myKeyCodes["JumpButton"] = keyCodeVals[4];
        myKeyCodes["CrouchButton"] = keyCodeVals[5];
        myKeyCodes["SprintButton"] = keyCodeVals[6];
        myKeyCodes["InteractButton"] = keyCodeVals[7];
    }
}


