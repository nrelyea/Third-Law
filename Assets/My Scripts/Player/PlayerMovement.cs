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
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("IsJumping", true);
        }

        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }
    }

    public void OnLand()
    {
        //Debug.Log("LANDED (Y Velocity = " + m_Rigidbody2D.velocity.y + ")");
        if((m_Rigidbody2D.velocity.y > 13.4 && m_Rigidbody2D.velocity.y < 13.42) || m_Rigidbody2D.velocity.y == 0) animator.SetBool("IsJumping", true);
        else animator.SetBool("IsJumping", false);
    }

    public void ForceLand()
    {
        animator.SetBool("IsJumping", false);
    }

    // called once every fixed amount of time (better for physics and movement)
    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }
}
