using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Timer : MonoBehaviour
{

    #region Vars
    public float timer; //Time in float to be converted to clock time
    public string clocktime; //Could be attached publically to canvas or via function 
    public GUIStyle text; //How the clock actually looks
    public DateTime time;

    #endregion


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        time = DateTime.Now;
        //this is a countdown, for count up just use +=
        if (timer != 0)
        {
            timer -= Time.deltaTime;
        }

        if (timer < 0)
        {
            timer = 0;
        }


    }

    private void OnGUI()
    {
        //should start timer 1 second above what you want as this floors the value, so on frame 1 it will drop to desire number. e.g. 61 is 60
        int mins = Mathf.FloorToInt(timer / 60);
        //this will figure out where we are at in terms of seconds by seeing how many seconds have passed in terms of minutes and our timer
        int seconds = Mathf.FloorToInt(timer - mins * 60);
        clocktime = string.Format("{0:0}:{1:00}", mins, seconds);
        float scrW = Screen.width / 16;
        float scrH = Screen.height / 9;
        GUI.Label(new Rect(0, scrH * 8, scrW * 2, scrH ), clocktime, text);
        GUI.Label(new Rect(0, 0, scrW * 2, scrH), time.Hour + ":" + time.Minute + ":" + time.Second, text);
    }
}
