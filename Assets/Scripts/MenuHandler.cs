using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; //for scene change interactivity
using UnityEngine.UI; // interact with UI elements
using UnityEngine.EventSystems; //for use of a controller (control events)

public class MenuHandler : MonoBehaviour
{
    //these function will be public because we need to be able to access them from the inspector later
    
    #region Variables
    public GameObject mainMenu, optionsMenu; //declare 2 gameObjects for later definition (likely from the inspector)
    public bool showOptions; //no defined value means this will default to FALSE
    public Slider volSlider, brightSlider;
    public AudioSource mainAudio; //for our audio
    public Light dirLight; //for the brightness
    #endregion

    private void Start()
    {
        
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
            mainAudio = GameObject.Find("MainMusic").GetComponent<AudioSource>(); //do the same but for audio source
            dirLight = GameObject.FindGameObjectWithTag("DirLight").GetComponent<Light>(); //etc but found a tag instead

            volSlider.value = mainAudio.volume; //value of the slider is equal to the audio level of mainAudio.
            
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

    } //slider to control brightness

}
