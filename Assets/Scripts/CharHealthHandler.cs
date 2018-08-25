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

    [Header("Attributes/Stats")]

    #region BaseStatModifiers
    public int constitution;
    public int strength;
    public int dexterity;
    public int intelligence;
    public int wisdom;
    public int charisma;
    #endregion


    [Header("Health")]
    #region Health
    //Plyaer current health and max health
    public float maxHP;
    public float currentHP;
    public float damageHP;
    public bool isHealing;
    public float tempHealingVal;
    public float healRate;

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
    public int maxExp, currentExp;

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

    #endregion

    #region Start
    //set max health to 100
    //set current health to max
    //make sure player is alive
    //max exp starts at 60
    //connect the Character Controller to the controller variable
    private void Start()
    {
        SetStatValues();
        currentHP = maxHP;
        damageHP = maxHP;
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
    private void LateUpdate()
    {
        CheckHP();
        CheckDamageHP();
        if (isAlive)
        {
            if (isHealing)
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
        GUI.Box(new Rect(scrW, scrH * 0.25f, scrW * 4, scrH * 0.375f), "", manaBar);

        //Stamina bar set up over its background
        GUI.Box(new Rect(scrW, scrH * 0.625f, scrW * 4, scrH * 0.375f), "", staminaBar);

        #endregion




        //GUI Box for current experience that moves in same place as the background bar


        GUI.Box(new Rect(scrW * 6, scrH * 0.75f, currentExp* (scrW * 4) / maxExp, scrH * 0.25f), "", expBar);

        GUI.DrawTexture(new Rect(scrW*13.75f, scrH*0.25f, scrW*2, scrH*2), miniMap);
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
        if(tempHealingVal > maxHP)
        {
            tempHealingVal = maxHP;
        }
        healRate = healVal / time;
        isHealing = true;
    }

    #region StatSetandCheckFunctions

    private void SetStatValues()
    {
        maxHP = 50f + 7 * constitution;
        maxStamina = 50f + 7 * dexterity;
        maxMana = 50f + 7 * wisdom;
    }

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
            //if the damage HP is less than the current health
            if (damageHP > currentHP)
            {
                //reduce the damage bar at a set rate. Coroutine lets me delay when the actual damage bar starts moving
                StartCoroutine(ReduceDamageBar());

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
    }

    private void CheckHealing()
    {
        if (!tempHealingVal.Equals(null))
        {
            if(currentHP < tempHealingVal)
            {
                HealOverTime(healRate);
                if(currentHP >= tempHealingVal)
                {
                    currentHP = tempHealingVal;
                    tempHealingVal = 0;
                    isHealing = false;
                }
            }

        }

    }
    



    #endregion

    #region CoRoutines
    IEnumerator ReduceDamageBar()
    {
        //this will reduce the damageBar at a fast rate but will ensure it does so over time
        yield return new WaitForSeconds(0.5f);
        damageHP -= 10 * Time.deltaTime;
    }

    IEnumerator GiveSetHPAtInterval(float HP)
    {
        currentHP += HP;
        yield return new WaitForSeconds(1);
    }

    #endregion
}









