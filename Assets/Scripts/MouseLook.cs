using UnityEngine;
using System.Collections;
//this script can be found in the Component section under the option Character Set Up 
//Mouse Look
[AddComponentMenu("FirstPerson/Camera")]
public class MouseLook : MonoBehaviour 
{
    //Before you write this section please scroll to the bottom of the page
    #region Variables
    [Header("Rotational Axis")]
    //create a public link to the rotational axis called axis and set a defualt axis
    public RotationAxis axis = RotationAxis.MouseX;
    [Header("sensitivity")]
    //public floats for our x and y sensitivity
    public float sensX = 10f;
    public float sensY = 10f;
    [Header("Y Rotation Clamp")]
    //If we let our rotation on Y go to war we will just start spinning.
    //max and min Y rotation
    public float minY = -60f;
    public float maxY = 60f;
    //we will have to invert our mouse position later to calculate our mouse look correctly
    //UI and MouseLook apparently do not naturally line up, and that is why they need adjusting
    //float for rotation Y
    //if this is PUBLIC/PRIVATE then it will default its value to zero, but with no access modifier, it will default it to private but its value will be NULL
    public float rotY;
    #endregion


    #region Start
    //if our game object has a rigidbody attached to it
    //set the rigidbodys freezRotaion to true
    private void Start()
    {
        //if game object has a rigid body
        if (this.GetComponent<Rigidbody>())
        {
            //set rigid body freeze rotation to true
            this.GetComponent<Rigidbody>().freezeRotation = true;
        }
    }

    #endregion


    #region Update
    private void Update()
    {
        
        #region Mouse X and Y 
        /*
        //if our axis is set to Mouse X and Y
        if(axis == RotationAxis.MouseXandY)
        {
            //float rotation x is equal to our y axis plus the mouse input on the Mouse X times our x sensitivity
            //euler angles in degrees relative to the parents transform rotation
            float rotX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensX; //sensX is the speed that we will move along mouseX axis
            //our rotation Y is plus equals  our mouse input for Mouse Y times Y sensitivity
            rotY += Input.GetAxis("Mouse Y") * sensY;
            //the rotation Y is clamped using Mathf and we are clamping the y rotation to the Y min and Y max
            rotY = Mathf.Clamp(rotY, minY, maxY);
            //transform our local position to the nex vector3 rotaion - y rotaion on the x axis and x rotation on the y axis. Need to invert Y to have Up actually be Up
            transform.localEulerAngles = new Vector3(-rotY, rotX, 0);
        }
        */
        #endregion
        #region Mouse X
        /*
        //else if we are rotating on the X
        else if (axis == RotationAxis.MouseX)
        {
            //transform the rotation on our game objects Y by our Mouse input Mouse X times X sensitivity
            transform.Rotate(0, Input.GetAxis("Mouse X") * sensX, 0); //only rotate the object around Y axis to allow horizontal turning with sens speed
        }
        */
        #endregion
        #region Mouse Y
        /*
        //else we are only rotation on the Y
        else
        {
            //our rotation Y is pluse equals  our mouse input for Mouse Y times Y sensitivity
            rotY += Input.GetAxis("Mouse Y") * sensY;
            //the rotation Y is clamped using Mathf and we are clamping the y rotation to the Y min and Y max
            rotY = Mathf.Clamp(rotY, minY, maxY);
            //transform our local position to the nex vector3 rotaion - y rotaion on the x axis and local euler angle Y on the y axis
            transform.localEulerAngles = new Vector3(-rotY, 0, 0);
        }
        */


        #endregion

        #region SwitchVersion
        switch (axis)
        {
            case RotationAxis.MouseXandY:
                //float rotation x is equal to our y axis plus the mouse input on the Mouse X times our x sensitivity
                //euler angles in degrees relative to the parents transform rotation
                float rotX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensX; //sensX is the speed that we will move along mouseX axis
                                                                                              //our rotation Y is plus equals  our mouse input for Mouse Y times Y sensitivity
                rotY += Input.GetAxis("Mouse Y") * sensY;
                //the rotation Y is clamped using Mathf and we are clamping the y rotation to the Y min and Y max
                rotY = Mathf.Clamp(rotY, minY, maxY);
                //transform our local position to the nex vector3 rotaion - y rotaion on the x axis and x rotation on the y axis. Need to invert Y to have Up actually be Up
                transform.localEulerAngles = new Vector3(-rotY, rotX, 0);
                break;

            case RotationAxis.MouseX:
                //transform the rotation on our game objects Y by our Mouse input Mouse X times X sensitivity
                transform.Rotate(0, Input.GetAxis("Mouse X") * sensX, 0); //rotate around Y axis lets us turn horizontally
                break;

            case RotationAxis.MouseY:
                rotY += Input.GetAxis("Mouse Y") * sensY;
                rotY = Mathf.Clamp(rotY, minY, maxY);
                transform.localEulerAngles = new Vector3(-rotY, 0, 0);
                
                break;
        }
        #endregion
    }
    #endregion

}
#region RotationalAxis
/*
enums are what we call state value variables 
they are comma separated lists of identifiers
we can use them to create Type or Category variables
meaning each heading of the list is a type or category element that this can be
eg weapons in an inventory are a type of inventory item
if the item is a weapon we can equipt it
if it is a consumable we can drink it
it runs different code depending on what that objects enum is set to
you can also have subtypes within those types
eg weapons are an inventory category or type
we can then have ranged, melee weapons
or daggers, short swords, long swords, mace, axe, great axe, war axe and so on
each Type or Category holds different infomation the game needs like 
what animation plays, where the hands sit on the weapon, how many hands sit on the weapon and so on
*/
//Create a public enum called RotationalAxis
    //MouseXandY
    //MouseX
    //MouseY
//Creating an enum out side the script and making it public means it can be asessed anywhere in any script with out reference
public enum RotationAxis //numbers associated with contents go from 0 upwards in order of variable introduced
{
    MouseXandY,
    MouseX,
    MouseY
}
#endregion











