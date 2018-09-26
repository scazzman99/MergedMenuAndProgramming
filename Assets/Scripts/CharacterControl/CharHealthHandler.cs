using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//this script can be found in the Component section under the option Character Set Up 
//Character Handler

//CHALLENGE: HEAL OVER TIME AND DROPPING HEALTH BAR

[AddComponentMenu("Skyrim Revengence/FirstPerson/CharHealthHandler")]
public class CharHealthHandler : MonoBehaviour
{
    #region Variables

    [Header("Character")]
    #region Character 
    public bool isAlive;
    public CharacterController playerController;
    #endregion

    [Header("Attributes/Stats")]

    #region BaseStatModifiers
    //to get stats, call index that matches with stat names index
    /*
    public int strength;
    public int dexterity;
    public int charisma;
    public int constitution;
    public int intelligence;
    public int wisdom;
    */
    public string playerName;
    public string playerClassName;
    public CharacterClass playerClass;
    //index to get the class of the character from enum
    public int classIndex;
    public string[] stats;
    public int[] statVals = new int[6];
    #endregion


    [Header("Health")]
    #region Health
    //Plyaer current health and max health
    public float maxHP;
    public float currentHP;
    public float damageHP;
    public bool isHealing; //bool to check if we are healing
    public bool isDamaging; //bool to check if we have taken damage
    public bool isNaturalHeal; //bool to see if we are regening naturally
    public float tempHealingVal; //heal value to be edited by potions etc
    public float healRate; //rate at which a potion will heal us
    public float naturalRegenRate = 5f; //natural regen rate

    #endregion

    [Header("Mana")]
    #region Mana
    public float maxMana;
    public float currentMana;
    #endregion

    [Header("Stamina")]
    #region Stamina
    public float maxStamina;
    public float currentStamina;
    public float staminaLossRate = 8f;
    #endregion

    [Header("Levels and Exp")]
    #region Level and Exp
    //level, max exp per level and players current exp
    public int playerLvl;
    public float maxExp, currentExp, expToGet, tempNewExp;

    #endregion

    [Header("Camera Connection")]
    #region MiniMap
    //render texture for the mini map that we need to connect to a camera
    public RenderTexture miniMap;

    #endregion

    [Header("Bars")]
    #region Bars
    public GUIStyle healthBar;
    public GUIStyle expBar;
    public GUIStyle damageBar;
    public GUIStyle manaBar;
    public GUIStyle staminaBar;
    #endregion
    public Pause pauseMenu;

    [Header("Texture lists and vars")]
    public Renderer characterRender;
    
    public int hairMax, skinMax, eyesMax, mouthMax, armourMax, clothesMax;

    #endregion

    #region Start
    //set max health to 100
    //set current health to max
    //make sure player is alive
    //max exp starts at 60
    //connect the Character Controller to the controller variable
    private void Start()
    {
        stats = new string[] { "Strength", "Dexterity", "Charisma", "Constitution", "Intelligence", "Wisdom" };

        //get stats from player prefs and get hp, mp and stam values from them
        SetStats();
        SetStatValues();

       

        //set current hp to max hp, same with damage and stamina and mana etc
        currentHP = maxHP;
        damageHP = maxHP;
        currentStamina = maxStamina;
        currentMana = maxMana;
        isAlive = true;
        maxExp = 60;
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
    }
    #endregion

    #region Update
    //if our current experience is greater or equal to the maximum experience
    //then the current experience is equal to our experience minus the maximum amount of experience
    //our level goes up by one
    //the maximum amount of experience is increased by 50

    private void Update()
    {
        /*if(expToGet != 0)
        {
            if(tempNewExp == 0)
            {
                tempNewExp = currentExp + expToGet;
            }
            currentExp += 10 * Time.deltaTime;
            if(currentExp >= tempNewExp)
            {
                currentExp = tempNewExp;
                tempNewExp = 0;
                expToGet = 0;
            }
        }
        */

        if (currentExp >= maxExp)
        {
            currentExp -= maxExp;
            playerLvl++;
            maxExp += 50;
        }
    }

    #endregion

    #region LateUpdate
    private void LateUpdate()
    {

        CheckHP();
        CheckDamageHP();
        if (isAlive)
        {
            //if either is healing is running or naturalHealing
            if (isHealing || isNaturalHeal)
            {
                CheckHealing();
            }

            CheckMana();
            CheckStamina();
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
        if (!pauseMenu.paused)
        {
            for (int x = 0; x < 16; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    GUI.Box(new Rect(scrW * x, scrH * y, scrW, scrH), "");
                }
            }

            #region BackgroundRects

            //background bar will sit in the same place at the main HP bar, and this main HP bar will change with hp MAX. e.g. 10hp and 15hp max fit into same bar
            GUI.Box(new Rect(scrW * 6, scrH * 0.25f, scrW * 4, scrH * 0.5f), "");

            //GUI Box on screen for the experience background
            GUI.Box(new Rect(scrW * 6, scrH * 0.75f, scrW * 4, scrH * 0.25f), "");

            //GUI box for Mana bar background
            GUI.Box(new Rect(scrW, scrH * 0.25f, scrW * 4, scrH * 0.375f), "");

            //GUI box for Stamina bar background
            GUI.Box(new Rect(scrW, scrH * 0.625f, scrW * 4, scrH * 0.375f), "");

            #endregion

            #region BarInitialisation

            //Health bar to sit behind main health and show on damage
            GUI.Box(new Rect(scrW * 6, scrH * 0.25f, damageHP * (scrW * 4) / maxHP, scrH * 0.5f), "", damageBar);

            //GUI Box for current health that moves in same place as the background bar
            //current Health divided by the posistion on screen and timesed by the total max health
            GUI.Box(new Rect(scrW * 6, scrH * 0.25f, currentHP * (scrW * 4) / maxHP, scrH * 0.5f), "", healthBar);

            //Mana bar set up over its background
            GUI.Box(new Rect(scrW, scrH * 0.25f, currentMana * (scrW * 4) / maxMana, scrH * 0.375f), "", manaBar);

            //Stamina bar set up over its background
            GUI.Box(new Rect(scrW, scrH * 0.625f, currentStamina * (scrW * 4) / maxStamina, scrH * 0.375f), "", staminaBar);

            GUI.Box(new Rect(scrW * 6, scrH * 0.75f, currentExp * (scrW * 4) / maxExp, scrH * 0.25f), "", expBar);

            #endregion

            #region MiniMap

            GUI.DrawTexture(new Rect(scrW * 13.75f, scrH * 0.25f, scrW * 2, scrH * 2), miniMap);

            #endregion
        }






    }
    #endregion



    private void HealOverTime(float healRate)
    {
        //add the heal value to the current HP value over a time
        currentHP += healRate * Time.deltaTime;
    }

    public void GetHealValue(float healVal, int time)
    {
        tempHealingVal = healVal + currentHP;
        if (tempHealingVal > maxHP)
        {
            tempHealingVal = maxHP;
        }
        healRate = healVal / time;
        isHealing = true;
    }

    public void DrainStamina()
    {
        currentStamina -= 8f * Time.deltaTime;
    }

    #region StatSetandCheckFunctions


   
   

    private void CheckMana()
    {
        //if current mana is greater than our max
        if (currentMana > maxMana)
        {
            //set current mana to our max
            currentMana = maxMana;
        }

        //if current mana has dropped beneath 0
        if (currentMana < 0)
        {
            //set current mana to 0
            currentMana = 0;
        }

    }

    private void CheckStamina()
    {
        //exactly the same as CheckMana()
        bool isSprinting = GetComponent<CharControlWithCam>().isSprinting;
        if (isSprinting)
        {
            DrainStamina();
        }

        if (currentStamina > maxStamina)
        {
            currentStamina = maxStamina;
        }

        if (currentStamina < 0)
        {
            currentStamina = 0;
        }
    }

    private void CheckHP()
    {
        //if the current HP is greater than our max
        if (currentHP > maxHP)
        {
            //set our current HP to our max HP
            currentHP = maxHP;
        }

        //if the current HP is less than 0
        if (currentHP < 0)
        {
            //set the current HP to 0
            currentHP = 0;
            Debug.Log("if less than 0 = 0");
        }

        //if the current HP is 0 AND the player is alive
        if (isAlive && currentHP == 0)
        {
            //set player alive to false
            isAlive = false;
            //disable the player controller
            playerController.enabled = false;
            Debug.Log("Disable on death");

        }

    }

    private void CheckDamageHP()
    {
        //move damageBar to the healths current value over time
        //if damage HP is not equal to our current HP
        if (damageHP != currentHP)
        {

            //if the damage HP is more than the current health
            if (damageHP > currentHP)
            {
                //reduce the damage bar at a set rate. Coroutine lets me delay when the actual damage bar starts moving
                StopCoroutine(WaitForRegen());
                StartCoroutine(ReduceDamageBar());

                //stop natural regen
                isDamaging = true;
                isNaturalHeal = false;

                //if we have overshot our HP somehow, make damage bar equal to the currentHP
                if (damageHP < currentHP)
                {
                    damageHP = currentHP;

                }


            }
            else
            {
                //update damage bar immediately if being healed
                damageHP = currentHP;


            }
        }
        else
        {
            StartCoroutine(WaitForRegen());
        }
        

    }

    private void CheckHealing()
    {
        //if there is no damage being taken and natural healing is on
        if(isDamaging == false && isNaturalHeal)
         {
             HealOverTime(naturalRegenRate);
             if (currentHP > maxHP)
             {
                 currentHP = maxHP;
                 isNaturalHeal = false;
             }
         }
         

        if (!tempHealingVal.Equals(0f))
        {
            if (currentHP < tempHealingVal)
            {
                HealOverTime(healRate);
                if (currentHP >= tempHealingVal)
                {
                    currentHP = tempHealingVal;
                    tempHealingVal = 0f;
                    isHealing = false;
                }
            }
            else
            {
                isHealing = false;
            }

        }

    }

    //Inital setting and getting of stats
    #region SetUpStats


    public void SetStats()
    {
        
        for (int i = 0; i < stats.Length; i++)
        {
            statVals[i] = PlayerPrefs.GetInt(stats[i]);
        }
        

        playerName = PlayerPrefs.GetString("CharacterName");
        classIndex = PlayerPrefs.GetInt("CharacterClass");
        playerClass = (CharacterClass)classIndex;
        playerClassName = playerClass.ToString();
        gameObject.name = playerName;
    }

    private void SetStatValues()
    {
        maxHP = 50f + 7 * statVals[3]; //gets constitution
        maxStamina = 50f + 7 * statVals[1]; //gets dexterity
        maxMana = 50f + 7 * statVals[5]; //gets wisdom
    }

    #endregion



    #endregion



    #region CoRoutines
    IEnumerator ReduceDamageBar()
    {
        //this will reduce the damageBar at a fast rate but will ensure it does so over time
        yield return new WaitForSeconds(0.5f);
        damageHP -= 20 * Time.deltaTime;
    }


     IEnumerator WaitForRegen()
     {
         yield return new WaitForSeconds(2);
         isDamaging = false;
         isNaturalHeal = true;
     }
     

    #endregion
}









