using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))] //auto grabs a CharacterController for the object this script is attached to, this will only work upon initial attachment to the object!
public class CharacterMovement : MonoBehaviour
{
    [Header("MOVEMENT VARIABLES")]
    [Space(10)]
    [Header("Mini Header")]
    [Range(0f, 10f)]
    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f; //this exageration, and others like it are usually needed for actions to feel satisfying
    private Vector3 moveDirection = Vector3.zero; //sets initial movement to 0 so nothing happens upon starting!
    public CharacterController controller;


    //For initialisation
    private void Start() //try to connect required elements in this method
    {
        controller = GetComponent<CharacterController>(); //same as controller = this.GetComponent<CharacterController>();
        //references 'controller' or 'this' instance
        /* multi
         * line
         * fam
         * */
    }
    // Update is called once per frame
    void Update ()
    {
		if(controller.isGrounded) //isGrounded is a function of CharacterController. This means we cannot adjust our trajectory mid-air
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical"));
            //getAxis can count as up,down,left,right on anything e.g keyboard, mouse, joystick etc. It has default response for W,A,S,D,SPACEand is based on LOCAL POSITION
            //getKey would look for hold, getKeyUp might be for button release and getKeyDown would be for button press

            moveDirection = transform.TransformDirection(moveDirection); // References attached objects transform property and runs function TransformDirection
            moveDirection *= speed; //takes base value for moveDirection vector and multiplies it by our 'speed' variable

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
                //now we need gravity to be affecting us. We need to do this always actually!
                //NOTE: We havent killed our running speed when jumping, because why would be ever do that

            }
        }
        moveDirection.y -= gravity * Time.deltaTime; //NOTE: GRAVITY IS CONSTATLY AFFECTING THE PLAYER AS IT IS OUTSIDE OF THE IF STATEMENT


        /*Time.deltaTime is a measure of time based on the frames in a game. this would let me pause mid jump and stop the jump
        it is essentially measuring real time to frames to keep stuff consistant, so varied frame rates wouldnt alter how something like gravity would work
        creates a reference between real time and the computers time
        we will inspect this later on */

        controller.Move(moveDirection * Time.deltaTime);
    }
}
