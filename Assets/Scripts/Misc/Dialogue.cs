using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//this script can be found in the Component section under the option NPC/Dialogue
[AddComponentMenu("Skyrim Revengence/NPC/Dialogue")]
public class Dialogue : MonoBehaviour
{
    #region Variables
    [Header("References")]
    public bool showDialogue; //boolean to toggle if we can see a characters dialogue box
    public int dialogueIndex, optionIndex; //index for our current line of dialogu and an index for a set question marker of the dialogue 
    public CharControlWithCam player; //script reference to the player movement, Camera look and player look as this contains all 3
    
    
    [Header("NPC Name and Dialogue")]
    public string npcName; //name of this specific NPC
    public string[] dialogueTexts; //array for text for our dialogue

    [Header("Screen Ratio")]
    Vector2 screenRatio;
    #endregion

    #region Start
    private void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharControlWithCam>(); //find and reference the player object by tag
        //find and reference the maincamera by tag and get the mouse look component 
    }
    #endregion

    #region OnGUI
    private void OnGUI()
    {
        //if our dialogue can be seen on screen
        if (showDialogue)
        {

            if (screenRatio.x != Screen.width / 16 || screenRatio.y != Screen.height / 9)
            {
                screenRatio = new Vector2(Screen.width / 16, Screen.height / 9);
            }

            //the dialogue box takes up the whole bottom 3rd of the screen and displays the NPC's name and current dialogue line
            GUI.Box(new Rect(0f, 6 * screenRatio.y, Screen.width, screenRatio.y * 3), npcName + ": " + dialogueTexts[dialogueIndex]); //Start at left edge and 6 down to take up bottom third


            //if not at the end of the dialogue AND not at the options part. Are we using the last index as a buffer or just flowing over?
            if (!(dialogueIndex >= dialogueTexts.Length - 1 || dialogueIndex == optionIndex)) //using ! >= just to makes sure we dont skip over
            {
                //next button allows us to skip forward to the next line of dialogue
                if(GUI.Button(new Rect(screenRatio.x * 15f, screenRatio.y * 8.5f, screenRatio.x, screenRatio.y * 0.5f), "Next")) //8.5 units down and 15 from the left and 0.5 units stretched down
                {
                    //move forward in the dialogue
                    dialogueIndex++;
                }
            }

            //else if we are at options
            else if (dialogueIndex == optionIndex)
            {
                //Accept button allows us to skip forward to the next line of dialogue
                if(GUI.Button(new Rect(screenRatio.x * 13f, screenRatio.y * 8.5f, screenRatio.x, screenRatio.y * 0.5f), "Accept"))
                {
                    dialogueIndex++;
                }
                //Decline button skips us to the end of the characters dialogue. PUT THESE IN DIFFERENT LOCATIONS SO SPAMMING DOESNT HAPPEN
                if (GUI.Button(new Rect(screenRatio.x * 14f, screenRatio.y * 8.5f, screenRatio.x, screenRatio.y * 0.5f), "Decline"))
                {
                    dialogueIndex = dialogueTexts.Length - 1;
                }
            }
            else {
                //else we are at the end

                //the Bye button allows up to end our dialogue
                if (GUI.Button(new Rect(screenRatio.x * 15f, screenRatio.y * 8.5f, screenRatio.x, screenRatio.y * 0.5f), "Bye"))
                {
                    //close the dialogue box
                    showDialogue = false;
                    //set index back to 0 
                    dialogueIndex = 0;
                    //allow cameras mouselook to be turned back on
                    //get the component mouselook on the character and turn that back on
                    //get the component movement on the character and turn that back on
                    player.enabled = true;
                    //lock the mouse cursor
                    Cursor.lockState = CursorLockMode.Locked;
                    //set the cursor to being invisible
                    Cursor.visible = false;
                }

            }
        }

    }
    #endregion
}
