using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMining : MonoBehaviour
{
    private Animator animator;
    private Vector2 facingDirection = Vector2.right;
    [SerializeField] private float miningDuration = 0.8f;
    [SerializeField] private float miningRange = 1.5f;
    public bool isMining = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>(); //Get and store the Animator component attached to the player
    }

    // Update is called once per frame
    void Update()
    {
        UpdateFacingDirection();

        if (!PauseMenu.isPaused)
        {
        //if the player presses the "mine" button and is not currently mining
        if(Input.GetButtonDown("Mine") && !isMining) 
        {
            animator.SetBool("Mining", true); //start mining animation
            StartCoroutine(MineCoroutine()); //start coroutine
        }
        else
        {
            animator.SetBool("Mining", false); //set Mining bool to false in animator
        }
        }

    }

    IEnumerator MineCoroutine()
    {
        isMining = true; //set isMkining to true

        yield return new WaitForSeconds(miningDuration); //wait for miningDuration
        
        MineBlock(); //mine the block (if mineable)

        isMining = false; //set isMining to false
        
    }

    void MineBlock()
    {
        Vector2 origin = transform.position; //get player position
        RaycastHit2D hit = Physics2D.Raycast(origin, facingDirection, miningRange); //check if there is a block infront of the player
        if (hit.collider != null) //if the raycast hits an object
        {
            GameObject block = hit.collider.gameObject;          

            if (block.CompareTag("Mineable")) //check if the block has the mineable tag
            {
                Destroy(block); //destroy the object
            }
        }
    }

    void UpdateFacingDirection() 
    {
       Vector2 inputDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")); //check the players direction based on input

       if (inputDir != Vector2.zero) //if there is an input
        {
            facingDirection = inputDir.normalized; //set facingDirection to the direction player is walking in
        }

        //otherwise if the player isnt moving, the facingDirection stays the same as the last input
    }
}
