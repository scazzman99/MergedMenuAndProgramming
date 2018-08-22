using UnityEngine;
using System.Collections;
//this script can be found in the Component section under the option Character Set Up 
//Character Handler

    //CHALLENGE: HEAL OVER TIME AND DROPPING HEALTH BAR

    [AddComponentMenu("RPG controls and stuff")]
public class CharHealthHandler : MonoBehaviour 
{
    #region Variables

    [Header("Character")]
    #region Character 
    public bool isAlive;
    public CharacterController playerController;
    #endregion

    [Header("Health")]
    #region Health
    //Plyaer current health and max health
    public float maxHP;
    public float currentHP;

    #endregion

    [Header("Levels and Exp")]
    #region Level and Exp
    //level, max exp per level and players current exp
    public int playerLvl;
    public int maxExp, currentExp;

    #endregion

    [Header("Camera Connection")]
    #region MiniMap
    //render texture for the mini map that we need to connect to a camera
    public RenderTexture miniMap;
    public GUIStyle healthBar;
    public GUIStyle expBar;
    #endregion

    #endregion

    #region Start
    //set max health to 100
    //set current health to max
    //make sure player is alive
    //max exp starts at 60
    //connect the Character Controller to the controller variable
    private void Start()
    {
        maxHP = 100f;
        currentHP = maxHP;
        isAlive = true;
        maxExp = 60;
        playerController = GameObject.Find("Player").GetComponent<CharacterController>();
    }
    #endregion

    #region Update
    //if our current experience is greater or equal to the maximum experience
    //then the current experience is equal to our experience minus the maximum amount of experience
    //our level goes up by one
    //the maximum amount of experience is increased by 50

    private void Update()
    {
        if(currentExp >= maxExp)
        {
            currentExp -= maxExp;
            playerLvl++;
            maxExp += 50;
        }
    }

    #endregion

    #region LateUpdate
    //if our current health is greater than our maximum amount of health
    //then our current health is equal to the max health
    //if our current health is less than 0 or we are not alive
    //current health equals 0
    //if the player is alive
    //and our health is less than or equal to 0
    //alive is false
    //controller is turned off

    private void LateUpdate()
    {
        if(currentHP > maxHP)
        {
            currentHP = maxHP;
        }

        if(currentHP < 0)
        {
            currentHP = 0;
            Debug.Log("if less than 0 = 0");
        }

        if(isAlive && currentHP == 0)
        {
            
            isAlive = false;
            playerController.enabled = false; //disables the player controller
            Debug.Log("Disable on death");
            
        }
    }
    #endregion

    #region OnGUI
    //set up our aspect ratio for the GUI elements
    //scrW - 16
    //scrH - 9
    //GUI Box on screen for the healthbar background
 
    
    //current experience divided by the posistion on screen and timesed by the total max experience
    //GUI Draw Texture on the screen that has the mini map render texture attached

    private void OnGUI()
    {
        float scrW = Screen.width / 16;
        float scrH = Screen.height / 9;

        for (int x = 0; x < 16; x++)
        {
            for (int y = 0; y < 9; y++)
            {
                GUI.Box(new Rect(scrW * x, scrH * y, scrW, scrH), "");
            }
        }

        GUI.Box(new Rect(scrW * 6, scrH * 0.25f, scrW * 4, scrH * 0.5f), "");
        //background bar will sit in the same place at the main HP bar, and this main HP bar will change with hp MAX. e.g. 10hp and 15hp max fit into same bar
        
        //GUI Box for current health that moves in same place as the background bar
        //current Health divided by the posistion on screen and timesed by the total max health
        GUI.Box(new Rect(scrW * 6, scrH * 0.25f, currentHP * (scrW * 4) / maxHP, scrH * 0.5f), "", healthBar);

        //GUI Box on screen for the experience background
        //GUI Box for current experience that moves in same place as the background bar
        GUI.Box(new Rect(scrW * 6, scrH * 0.75f, scrW * 4, scrH * 0.25f), "");
        GUI.Box(new Rect(scrW * 6, scrH * 0.75f, currentExp* (scrW * 4) / maxExp, scrH * 0.25f), "", expBar);

        GUI.DrawTexture(new Rect(scrW*13.75f, scrH*0.25f, scrW*2, scrH*2), miniMap);
    }
    #endregion
}









