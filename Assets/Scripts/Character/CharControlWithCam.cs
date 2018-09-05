using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharControlWithCam : MonoBehaviour {

    #region Vars
    [Header("MainMovementVariables")]
    [Space(10)]
    public float speed = 5f;
    public float crouchSpeed = 2f;
    public float sprintSpeed = 8f;
    public float grav = 20f;
    public float jumpSpeed = 8f;
    public bool isSprinting;
    public CharacterController playerController;
    private Vector3 moveVector = Vector3.zero;
    [Header("CameraVariables")]
    [Space(10)]
    public Camera mainCam;
    public RotationAxis rotAxis;
    //Sensitivity vars will control the speed at which cam/player rotation will happen
    public float sensX = 10f;
    public float sensY = 10f;
    //Min and max Y will be used to clamp the angle at which the camera can tilt vertically
    public float minY = -60f;
    public float maxY = 60f;
    //Empty variable used to store input for Y movement of camera
    public float rotY;
    #endregion

    #region Start&Update
    // Use this for initialization
    void Start () {
        playerController = GetComponent<CharacterController>();
        mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        PlayerMovement();
        CameraMovement();
	}

    #endregion

    #region Player&CameraMovement

    void PlayerMovement()
    {
        if (playerController.isGrounded)
        {
            //get the new direction of movement from input
           moveVector = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

            //actually use this vector to transform coordinates of player
            moveVector = transform.TransformDirection(moveVector);

            if (Input.GetKey(KeyCode.C))
            {
                moveVector *= crouchSpeed;
            }

            else if (Input.GetKey(KeyCode.LeftShift))
            {
                //if the player still has stamina
                if (GetComponent<CharHealthHandler>().currentStamina != 0)
                {
                    moveVector *= sprintSpeed;
                    isSprinting = true;
                } else
                {
                    isSprinting = false;
                    //move at default speed
                    moveVector *= speed;
                }
            }
            else
            {
                //increase the rate at which the player travels
                moveVector *= speed;
            }


            if (Input.GetButton("Jump"))
            {
                //if jump button is pressed, add y component to moveVector to make the player jump. APPLY CONST GRAVITY
                moveVector.y = jumpSpeed;
            }
        }

        moveVector.y -= grav * Time.deltaTime;

        //ensures that if game pauses the player will stop
        playerController.Move(moveVector * Time.deltaTime);

        //if player stops sprinting set to false so HealthHandler stops draining stamina
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isSprinting = false;
        }
    }

    void CameraMovement()
    {
        switch (rotAxis)
        {
            case RotationAxis.MouseXandY:
                //TIME.DELTATIME IS A FLOAT AND WILL SEVERLY HURT ROTATION, THERFORE USE TIMESCALE WHICH IS EITHER 1 OR 0
                //Y rotation is just y axis of mouse multiply sens in Y direction
                rotY += Input.GetAxis("Mouse Y") * sensY * Time.timeScale;

                //takes the Y rotation and ensures that it cannot exceed our max and min y rotations, simulating a neck.
                rotY = Mathf.Clamp(rotY, minY, maxY);

                //rotate the camera around the X axis using clamped values to give vertical rotation
                mainCam.transform.localEulerAngles = new Vector3(-rotY, 0f, 0f);

                //rotate the player around the Y axis according to their input from X axis and sensX
                playerController.transform.Rotate(0f, Input.GetAxis("Mouse X") * sensX * Time.timeScale, 0f);
               
                

                
                break;

            case RotationAxis.MouseX:
                //rotate the player around the Y axis when turning left or right with Cam
                playerController.transform.Rotate(0f, Input.GetAxis("Mouse X") * sensX, 0f);
                break;
            case RotationAxis.MouseY:
                //get rotation input and * sens to give base Y rotation
                rotY += Input.GetAxis("Mouse Y") * sensY;
                //same as in MouseXandY
                rotY = Mathf.Clamp(rotY, minY, maxY);

                //adjust angle of camera based on -rotY value, which will turn camera around the x axis.
                mainCam.transform.localEulerAngles = new Vector3(-rotY, 0f, 0f);
                break;
        }
    }

    #endregion

}

#region RotationAxisEnum
public enum RotationAxis
{
    MouseXandY,
    MouseX,
    MouseY
}
#endregion