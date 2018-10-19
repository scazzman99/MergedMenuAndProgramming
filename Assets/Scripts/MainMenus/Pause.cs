using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour {
    //make pause var static so only one exists
    //public static bool paused;
    public Canvas pauseCanvas;
    public LevelUpMenu levelUpMenu;
    
	// Use this for initialization
	void Start () {
        Time.timeScale = 1; //sets the time scale to 1, so the game runs at 'regular' speed
        levelUpMenu = GameObject.FindGameObjectWithTag("Player").GetComponent<LevelUpMenu>();
	}
	
	// Update is called once per frame
    /*
	void Update () {
        //can't pause during level up screen

        if (Input.GetKeyDown(KeyCode.Escape) && !levelUpMenu.levelPause)
            {
                if (paused)
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
                else if(paused && Inventory.showInv){
                    paused = false;
                    pauseCanvas.gameObject.SetActive(false);
                }
                else
                {
                    Time.timeScale = 0;
                    paused = true; //with time scale set to 0, we can pull up the pause menu and stop the world.
                    pauseCanvas.gameObject.SetActive(paused);
                    Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = true;
                }
            }
        }
        */
	
}
