﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
[AddComponentMenu("Skyrim Revengence/Menu/PauseMenu")]

public class PauseMenu : MonoBehaviour
{

    public bool isOptions = false;
    public GameObject pauseMenu, settingsMenu;
    public Scrollbar volumeSlider, brightSlider, ambientSlider;
    public Light dirLight;
    public Resolution[] myResolutions;
    public int resIndex;
    public Dropdown resolutions;
    public bool isFullScreen;
    public Dictionary<string, KeyCode> myKeys = new Dictionary<string, KeyCode>();
    public KeyCode holdingKey;
    public Text forwardTxt, backwardTxt, leftTxt, rightTxt, jumpTxt, crouchTxt, sprintTxt, interactTxt;
    private GameObject currentKey;
    public static bool paused;
    public Canvas pauseCanvas;
    public LevelUpMenu levelUpMenu;

    // Use this for initialization
    void Start()
    {
        myKeys.Add("Forward", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Forward", "W")));
        myKeys.Add("Backward", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Backward", "S")));
        myKeys.Add("Left", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Left", "A")));
        myKeys.Add("Right", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Right", "D")));
        myKeys.Add("Jump", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Jump", "Space")));
        myKeys.Add("Crouch", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Crouch", "LeftControl")));
        myKeys.Add("Sprint", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Sprint", "LeftShift")));
        myKeys.Add("Interact", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Interact", "E")));

        forwardTxt.text = myKeys["Forward"].ToString();
        backwardTxt.text = myKeys["Backward"].ToString();
        leftTxt.text = myKeys["Left"].ToString();
        rightTxt.text = myKeys["Right"].ToString();
        jumpTxt.text = myKeys["Jump"].ToString();
        crouchTxt.text = myKeys["Crouch"].ToString();
        sprintTxt.text = myKeys["Sprint"].ToString();
        interactTxt.text = myKeys["Interact"].ToString();

        levelUpMenu = GameObject.FindGameObjectWithTag("Player").GetComponent<LevelUpMenu>();
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void ToggleOptions()
    {

        //dont have this equal anything or the bool will get ruined
        OptionToggle();
    }

    public void TogglePause()
    {
        if (!levelUpMenu.levelPause)
        {
            if (paused)
            {
                //if we are in the setings menu
                if (isOptions)
                {
                    //toggle menus
                    ToggleOptions();
                }
                if (paused && !Inventory.showInv)
                {
                    Time.timeScale = 1;
                    paused = false; //with time scale back at 1 and the menu gone, the gmae will continue to play
                    pauseCanvas.gameObject.SetActive(paused);
                    //reset panels
                    pauseMenu.SetActive(true);
                    settingsMenu.SetActive(false);
                    isOptions = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    
                }
                else if (paused && Inventory.showInv)
                {
                    paused = false;
                    pauseCanvas.gameObject.SetActive(false);
                }
            }
            else
            {
                Time.timeScale = 0;
                paused = true; //with time scale set to 0, we can pull up the pause menu and stop the world.
                pauseCanvas.gameObject.SetActive(paused);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

        }
    }

    bool OptionToggle()
    {
        if (isOptions)
        {
            isOptions = false;
            settingsMenu.SetActive(false);
            pauseMenu.SetActive(true);
            return true;
        }
        else
        {
            isOptions = true;
            settingsMenu.SetActive(true);
            pauseMenu.SetActive(false);
            volumeSlider = GameObject.Find("VolumeBar").GetComponent<Scrollbar>();
            brightSlider = GameObject.Find("BrightnessBar").GetComponent<Scrollbar>();
            ambientSlider = GameObject.Find("AmbientBar").GetComponent<Scrollbar>();
            resolutions = GameObject.Find("ResDropdown").GetComponent<Dropdown>();
            brightSlider.value = dirLight.intensity;
            ambientSlider.value = RenderSettings.ambientIntensity;
            GetResoltuions();

            return false;
        }
    }

    public void Volume()
    {

    }

    public void Brightness()
    {
        dirLight.intensity = brightSlider.value;
    }

    public void Ambient()
    {
        RenderSettings.ambientIntensity = ambientSlider.value;
    }

    public void GetResoltuions()
    {
        List<string> resOps = new List<string>();
        myResolutions = Screen.resolutions;
        resolutions.ClearOptions();
        for (int i = 0; i < myResolutions.Length; i++)
        {
            string option = myResolutions[i].width + "x" + myResolutions[i].height;
            resOps.Add(option);
            if (myResolutions[i].Equals(Screen.currentResolution))
            {
                resIndex = i;
            }
        }
        resolutions.AddOptions(resOps);
        resolutions.value = resIndex;
        resolutions.RefreshShownValue();
    }

    public void SetResoultion()
    {
        Resolution myRes = myResolutions[resIndex];
        Screen.SetResolution(myRes.width, myRes.height, isFullScreen);
    }

    public void FullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        paused = false; //with time scale back at 1 and the menu gone, the gmae will continue to play
        pauseCanvas.gameObject.SetActive(paused);
        GameObject.Find("PauseHandler").GetComponent<PauseMenu>().pauseMenu.SetActive(true);
        GameObject.Find("PauseHandler").GetComponent<PauseMenu>().settingsMenu.SetActive(false);
        GameObject.Find("PauseHandler").GetComponent<PauseMenu>().isOptions = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    public void ReturnToMain()
    {
        SceneManager.LoadScene(0);
    }

    public void GetNewKeybind(GameObject pressedButton)
    {
        currentKey = pressedButton;
    }

    private void OnGUI()
    {
        if (currentKey != null)
        {
            Event e = Event.current;
            if (e.isKey)
            {

                myKeys[currentKey.name] = e.keyCode;
                //get the text componenet of the the child of the pressed button
                currentKey.transform.GetChild(0).GetComponent<Text>().text = e.keyCode.ToString();
                currentKey = null;
            }
        }
    }
}
