using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMining : MonoBehaviour
{
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>(); //Get and store the Animator component attached to the player
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Mine"))
        {
            animator.SetBool("Mining", true);
        }
        else
        {
            animator.SetBool("Mining", false);
        }
    }

}
