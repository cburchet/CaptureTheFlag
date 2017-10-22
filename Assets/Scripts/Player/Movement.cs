using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

    Rigidbody body;
    public Camera cam;

    //movement speeds
    float forwardSpeed = 5.0f;
    float strafeSpeed = 5.0f;
    float jumpSpeed = 7.5f;
    float verticalSpeed = 0.0f;

    //sensitivity for mouse
    float sensitivity = 5.0f;

    //current rotation up and down
    float verticalRotation = 0.0f;
    //range of vertical rotation for camera
    float upDownRange = 60.0f;

    //size of collider's y
    float colliderDist;


    // Use this for initialization
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        body = GetComponent<Rigidbody>();
        colliderDist = GetComponent<Collider>().bounds.extents.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        //movement
        Move();
    }


    bool isGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, colliderDist + .1f);
    }


    void Move()
    {

        float sideRotation = Input.GetAxis("Mouse X") * sensitivity;   //get x axis from mouse movement
        transform.Rotate(0, sideRotation, 0);  //rotate the player around the y

        verticalRotation -= Input.GetAxis("Mouse Y") * sensitivity;     //subtract the mouse y axis from vertical rotation
        verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);    //prevent verticalRotation from leaving desired range
        cam.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);     //rotate the camera by verticalRotation

        //check if sprinting and increase/decrease forwardspeed as needed
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            forwardSpeed = 10.0f;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            forwardSpeed = 5.0f;
        }

        //get forward and strafe speeds
        float hMove = Input.GetAxis("Horizontal") * strafeSpeed;
        float fMove = Input.GetAxis("Vertical") * forwardSpeed;

        //check if grounded
        if (isGrounded())
        {
            verticalSpeed = 0.0f;
            //if jump is pressed set vertical speed to jumpSpeed and grouned to false
            if (Input.GetButtonDown("Jump"))
            {
                verticalSpeed = jumpSpeed;
            }
        }
        //if not add gravity acceleration to vertical speed
        else
        {
            verticalSpeed += Physics.gravity.y * Time.deltaTime;
        }

        //set the bodies velocity in relation to current rotation
        body.velocity = transform.rotation * new Vector3(hMove, verticalSpeed, fMove);
    }
}
