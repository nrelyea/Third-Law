using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController2D controller;
    public Animator animator;

    public float runSpeed = 40f;

    private float horizontalMove = 0f;
    private bool jump = false;
    private bool crouch = false;

    private Rigidbody2D m_Rigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // ignore all input handling if game is paused
        if (GlobalVars.GameIsPaused) return;
        
        //  The original left / right movement data was encapsulated into this one line, which also has benefit of future controller support
        //  I have to remove for now to substitute in manual checks for right and left in order to maintain controls changing support
        //horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (Input.GetKey(GlobalVars.MoveRightKey) && !Input.GetKey(GlobalVars.MoveLeftKey)) // if input is only right, move right
        {
            horizontalMove = runSpeed;
        }
        else if (Input.GetKey(GlobalVars.MoveLeftKey) && !Input.GetKey(GlobalVars.MoveRightKey)) // if input is only left, move left
        {
            horizontalMove = runSpeed * -1f;
        }
        else // otherwise, if input is neither or both, don't move
        {
            horizontalMove = 0;
        }

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetKeyDown(GlobalVars.JumpKey))
        {
            jump = true;
            //animator.SetBool("IsJumping", true);
        }

        if (Input.GetKey(GlobalVars.WalkKey))
        {
            crouch = true;
        }
        else if (Input.GetKeyUp(GlobalVars.WalkKey))
        {
            crouch = false;
        }
    }

    public void OnLand()
    {
        //Debug.Log("LANDED (Y Velocity = " + m_Rigidbody2D.velocity.y + ")");
        //if((m_Rigidbody2D.velocity.y > 13.4 && m_Rigidbody2D.velocity.y < 13.42) || m_Rigidbody2D.velocity.y == 0) animator.SetBool("IsJumping", true);
        //else animator.SetBool("IsJumping", false);
    }

    // called once every fixed amount of time (better for physics and movement)
    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }

    
}
