using System.Collections;
using UnityEngine;

public class Interact : MonoBehaviour
{
    // Use this for initialization
    [Header("Reference")]
    public GameObject player; //split these up to not duplicate header
    public GameObject mainCam; //will use this for mouse look later on

    void Start()
    {
        player = GameObject.Find("Player"); //this is reference by name. Can be used for specifics but not generic things e.g. not Door but player is individual here
        mainCam = GameObject.FindGameObjectWithTag("MainCamera"); //this finds AN object with tag given. Can do FindGameObjectsWithTag for getting several objects with the same tag

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray interact; //this Ray object only exists inside this function, which is just a line 
            interact = Camera.main.ScreenPointToRay(new Vector2((Screen.width/2) , (Screen.height/2))); //Screen is a vector 2. set cam position to centre of screen hence screen width/height is divided by 2
            RaycastHit hitInfo; //raycast that can detect hits on it (physics)
            if (Physics.Raycast(interact, out hitInfo, 10f)) //checks for proximity. takes the line (ray) the output from that line (out raycasthit) and the distance of that line to hit
            {
                #region NPC DIALOGUE
                //regions let you and your group easily navigate where they are in the code. Note, regions are also able to be nested!
                if (hitInfo.collider.CompareTag("NPC")){ //checks the tag of the object which the RaycastHit's collider has hit and checks if it has the tag "NPC"
                    Debug.Log("Talk to NPC");//print this message to the debug log
                }
                #endregion

                #region CHEST
                if (hitInfo.collider.CompareTag("CHEST"))
                {
                    Debug.Log("OPEN CHEST");
                }
                #endregion

                #region ITEM
                if (hitInfo.collider.CompareTag("ITEM"))
                {
                    Debug.Log("GET ITEM");
                }
                #endregion

            }

        }

    }
}
