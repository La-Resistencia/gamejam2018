using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject {

    private float maxSpeed = 7;
    public float maxAutomaticSpeed = 3;
    public float jumpTakeOffSpeed = 7;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private float automaticSpeed;

    // Use this for initialization
    void Awake () 
    {
        spriteRenderer = GetComponent<SpriteRenderer> (); 
        animator = GetComponent<Animator> ();
        automaticSpeed = 0f;
    }

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        automaticSpeed = automaticSpeed > maxAutomaticSpeed?
            automaticSpeed:
            automaticSpeed + 0.01f ;
        move.x = automaticSpeed;
        Debug.Log("compute velocity "+move);

        if (Input.GetButtonDown ("Jump") && grounded) {
            velocity.y = jumpTakeOffSpeed;
        } else if (Input.GetButtonUp ("Jump")) 
        {
            if (velocity.y > 0) {
                velocity.y = velocity.y * 0.5f;
            }
        }

        if(move.x > 0.01f)
        {
            if(spriteRenderer.flipX == true)
            {
                spriteRenderer.flipX = false;
            }
        } 
        else if (move.x < -0.01f)
        {
            if(spriteRenderer.flipX == false)
            {
                spriteRenderer.flipX = true;
            }
        }

        animator.SetBool ("grounded", grounded);
        animator.SetFloat ("velocityX", Mathf.Abs (velocity.x) / maxSpeed);

        targetVelocity = move * maxSpeed;
    }
}