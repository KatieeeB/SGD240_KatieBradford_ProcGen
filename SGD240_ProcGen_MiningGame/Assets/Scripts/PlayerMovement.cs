using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private bool isFacingLeft = false;
    private Vector2 movement;
    private Animator animator;
    private PlayerMining playerMining;


    // Start is called before the first frame update
    void Start()
    {
       rb = GetComponent<Rigidbody2D>(); // Get and store the rigidbody component attached to the player
       animator = GetComponent<Animator>(); //Get and store the Animator component attached to the player
       playerMining = GetComponent<PlayerMining>(); //get and store the playerMining script attatched to the player
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.isPaused)
        {
            if(playerMining != null && playerMining.isMining) //if the player is currently mining
            {
                movement = Vector2.zero; //stop player movement
            }
            else
            {
                movement.x = Input.GetAxisRaw("Horizontal"); //set movement.x to horizontal input
                movement.y = Input.GetAxisRaw("Vertical"); //set movement.y to vertical input

                FlipSprite();

                animator.SetFloat("Walking", Mathf.Abs(movement.sqrMagnitude)); //play walking animation when the player moves in any direction
            }
        }
    }


    void FlipSprite() //flip the sprite when the player changes direction
    {
        if(isFacingLeft && movement.x > 0f || !isFacingLeft && movement.x < 0f) 
        {
            isFacingLeft = !isFacingLeft;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime); //move player
    }
}
