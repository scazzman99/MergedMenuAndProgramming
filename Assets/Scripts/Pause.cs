using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour {

    public bool paused;
	// Use this for initialization
	void Start () {
        Time.timeScale = 1; //sets the time scale to 1, so the game runs at 'regular' speed
	}
	
	// Update is called once per frame
	void Update () {
		
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                Time.timeScale = 1;
                paused = false; //with time scale back at 1 and the menu gone, the gmae will continue to play
                
            } else
            {
                Time.timeScale = 0;
                paused = true; //with time scale set to 0, we can pull up the pause menu and stop the world.
                
            }
        }
	}
}
