using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpMenu : MonoBehaviour
{

    public int level;
    public int points;
    public int[] statVals;
    public string[] stats;
    public int[] minVals;
    public CharHealthHandler charStats;
    public bool levelPause;
   
    // Use this for initialization
    void Start()
    {
        //must be attached to the player
        charStats = GetComponent<CharHealthHandler>();
        
    }

    private void Update()
    {
        //if player opens the menu reset the points and arrays by rerunning prepare
        if(charStats.canLevel && Input.GetKeyDown(KeyCode.I))
        {
            PrepareLevelUp();
        }

      
    }


    private void OnGUI()
    {
        float scrW = Screen.width / 16f;
        float scrH = Screen.height / 9f;
        //if the levelpause is true (hapens when level up occurs)
        if (levelPause)
        {
            GUI.Box(new Rect(scrW * 6f, scrH * 2f, scrW * 4f, scrH * 0.5f), "Points: " + points);

            for (int i = 0; i < statVals.Length; i++)
            {
                if (statVals[i] > minVals[i])
                {
                    if (GUI.Button(new Rect(scrW * 5f, scrH * 2.5f + i * (scrH * 0.5f), scrW, scrH * 0.5f), "-"))
                    {
                        points++;
                        statVals[i]--;
                    }
                }

                GUI.Box(new Rect(scrW * 6f, scrH * 2.5f + i * (scrH * 0.5f), scrW * 4f, scrH * 0.5f), stats[i] + ": " + statVals[i]);

                if (points > 0)
                {
                    if (GUI.Button(new Rect(scrW * 10f, scrH * 2.5f + i * (scrH * 0.5f), scrW, scrH * 0.5f), "+"))
                    {
                        points--;
                        statVals[i]++;
                    }
                }


            }

            if (points == 0)
            {
                if (GUI.Button(new Rect(scrW * 6f, scrH * 7f, scrW * 4f, scrH * 1f), "FINISH"))
                {
                    charStats.playerLvl = level;
                    charStats.statVals = statVals;
                    
                    //Adjust player health, stamina and mana
                    charStats.SetStatValues();
                    Time.timeScale = 1;
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    //remove ability to open levelup menu untill next xp cap
                    charStats.canLevel = false;
                    charStats.Save();
                    levelPause = false;
                }
            }
        }
    }


    public void PrepareLevelUp()
    {

        if (!levelPause)
        {
            //freeze time for the level up process
            levelPause = true;
            Time.timeScale = 0;
            //make stat vals and minvals empty arrys and fill them in using getstats
            statVals = new int[charStats.statVals.Length];
            minVals = charStats.statVals;
            //fill in the arrays from charStats
            GetStats();
            level = charStats.playerLvl;
            stats = charStats.stats;
            //give us points to use for the level up process
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            levelPause = false;
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void GetStats()
    {
        for (int i = 0; i < charStats.stats.Length; i++)
        {
            statVals[i] = charStats.statVals[i];
            minVals[i] = statVals[i];
        }
    }
}
