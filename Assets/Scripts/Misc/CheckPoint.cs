using UnityEngine;
using System.Collections;
//this script can be found in the Component section under the option Character Set Up 
//CheckPoint


public class CheckPoint : MonoBehaviour 
{
    #region Variables
    [Header("CheckPoints")]
    GameObject checkpointCurrent;
    [Header("HealthHandler")]
    CharHealthHandler healthHandler;
    
    #endregion

    #region Start
    
    private void Start()
    {
        //the character handler is the component attached to our player
        healthHandler = GetComponent<CharHealthHandler>();

        //if we have a save key called SpawnPoint
        if (PlayerPrefs.HasKey("SpawnPoint"))
        {
            //then our checkpoint is equal to the game object that is named after our save file
            //remember to get saves we use PlayerPrefs.GetString
            checkpointCurrent = GameObject.Find(PlayerPrefs.GetString("SpawnPoint"));

            //our transform.position is equal to that of the checkpoint
            transform.position = checkpointCurrent.transform.position;
            
        }

    }
    #endregion

    #region Update
    //if our characters health is less than or equal to 0
    
    //our characters health is equal to full health
    //character is alive
    //characters controller is active		

    private void Update()
    {
        //already checked for less than in other class
        if(CharHealthHandler.currentHP == 0)
        {
            //our transform.position is equal to that of the checkpoint
            transform.position = checkpointCurrent.transform.position;

            //set player hp to max
            CharHealthHandler.currentHP = CharHealthHandler.maxHP;

            //set player to alive and reactivate them
            healthHandler.isAlive = true;
            healthHandler.playerController.enabled = true;
        }
    }
    #endregion

    #region OnTriggerEnter
    //Collider other
    //if our other objects tag when compared is CheckPoint
    //our checkpoint is equal to the other object
    //save our SpawnPoint as the name of that object
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CheckPoint"))
        {
            //checkpoint is equal to the object with collided with
            checkpointCurrent = other.gameObject;
            
            //save the checkpoint position to spawnpoint using SetString. Want other's gameobject name
            PlayerPrefs.SetString("SpawnPoint", checkpointCurrent.name);
        }
    }
    #endregion


}





