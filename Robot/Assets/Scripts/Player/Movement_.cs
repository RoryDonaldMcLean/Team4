using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using UnityEngine.UI;

public class Movement_ : MonoBehaviour
{
    //Used for the Xinput Plug in. Tracks the state of the controllers for player 1 and 2
    GamePadState state;
    GamePadState prevState;
    GamePadState player2State;
    GamePadState player2PrevState;

    Vector3 velocity = new Vector3(0.0f, 0.0f, 0.0f);

    //is there 2 players in the game. if so use different controls for player 1 and 2 
    //but allows it all to be in 1 script
    public bool player2 = false;

    public float playerSpeed = 4.0f;

    public Rigidbody rb1;
    public float jumpSpeed;
    public float dropdownSpeed;

    public bool grounded = true;
    private bool doubleJump = false;

    // Use this for initialization
    void Start()
    {
        rb1 = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    void ProcessInput()
    {
        //update the game controller
        prevState = state;
        state = GamePad.GetState(PlayerIndex.One);

        player2PrevState = player2State;
        player2State = GamePad.GetState(PlayerIndex.Two);

        //player 1
        //move forward
        if (player2)
        {
            if (Input.GetKey(GameManager.Instance.playerButtons.currentButton[0]) || (prevState.ThumbSticks.Left.Y > 0.1))
            {
                velocity.z += 1.0f;

            }
            //move backward
            if (Input.GetKey(GameManager.Instance.playerButtons.currentButton[2]) || (prevState.ThumbSticks.Left.Y < -0.1))
            {
                velocity.z -= 1.0f;
            }


            //move left
            if (Input.GetKey(GameManager.Instance.playerButtons.currentButton[1]) || (prevState.ThumbSticks.Left.X < -0.1))
            {
                velocity.x -= 1.0f;
            }

            //move right
            if (Input.GetKey(GameManager.Instance.playerButtons.currentButton[3]) || (prevState.ThumbSticks.Left.X > 0.1))
            {
                velocity.x += 1.0f;
            }

            if ((grounded || doubleJump) && Input.GetKeyDown(KeyCode.M) && this.GetLegQuantity() >= 1)
            {
                if (grounded && this.GetLegQuantity() >= 2)
                    doubleJump = true;
                else
                    doubleJump = false;
                grounded = false;
                velocity.y = jumpSpeed;
            }


        }
        else
        {
            //////////////////////////////////////////////////////////
            /// //player 2
            /// //move forward
            if (Input.GetKey(KeyCode.UpArrow) || (player2PrevState.ThumbSticks.Left.Y > 0.1))
            {
                velocity.z += 1.0f;
            }
            //move backward
            if (Input.GetKey(KeyCode.DownArrow) || (player2PrevState.ThumbSticks.Left.Y < -0.1))
            {
                velocity.z -= 1.0f;
            }

            //if (gameController.GetComponent<SCR_CameraFollow> ().leftPuzzle == false)
            //{
            //move left
            if (Input.GetKey(KeyCode.LeftArrow) || (player2PrevState.ThumbSticks.Left.X < -0.1))
            {
                velocity.x -= 1.0f;
            }

            //move right
            if (Input.GetKey(KeyCode.RightArrow) || (player2PrevState.ThumbSticks.Left.X > 0.1))
            {
                velocity.x += 1.0f;
            }
            //}

            //jumping
            if ((grounded || doubleJump) && Input.GetKeyDown(KeyCode.G) && this.GetLegQuantity() >= 1)
            {
                if (grounded && this.GetLegQuantity() >= 2)
                    doubleJump = true;
                else
                    doubleJump = false;
                grounded = false;
                velocity.y = jumpSpeed;
            }

        }
        updateMovement(velocity);
        velocity.z = 0.0f;
        velocity.x = 0.0f;
        if (!grounded)
        {
            if (velocity.y >= -1)
                velocity.y += Physics.gravity.y * Time.deltaTime * dropdownSpeed;
            else
                velocity.y = -1.01f;
        }
        else
            velocity.y = 0;
    }

    //updates movement using the passed velocity vector
    void updateMovement(Vector3 vel)
    {
        //Rigidbody rb1 = GetComponent<Rigidbody> ();
        rb1.velocity = vel * playerSpeed;

        //will rotate the player to face the direction they are moving
        transform.LookAt(transform.position + new Vector3(rb1.velocity.x, 0, rb1.velocity.z));
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Ground")
        {
            grounded = true;
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Ground")
        {
            grounded = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Ground")
        {
            grounded = false;
        }
    }

    private int GetLegQuantity()
    {
        int quantity = 0;
        //loop through all the child objects attached to player
        for (int i = 0; i < this.transform.childCount; i++)
        {
            //find the object that has the "area" in it's name
            if (this.transform.GetChild(i).name.Contains("area"))
            {
                //loop through all of that objects children, they should all be the hinges OR Limbs
                for (int u = 0; u < this.transform.GetChild(i).childCount; u++)
                {
                    //find the object that has the "Leg" in it's name
                    if (this.transform.GetChild(i).transform.GetChild(u).name.Contains("Leg"))
                    {
                        quantity++;
                    }
                }
            }

        }
        return quantity;
    }

}
