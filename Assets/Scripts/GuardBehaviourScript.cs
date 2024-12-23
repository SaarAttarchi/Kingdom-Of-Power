using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardBehaviourScript : MonoBehaviour
{
    public GameObject player;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float distanse = Vector3.Distance(player.transform.position, transform.position);
        if(distanse < 10) // changes animation if the player is in range
        {
            if(animator.GetInteger("State") != 1)
            {
                animator.SetInteger("State", 1);
            }

            // rotate towards the player
            Vector3 target = player.transform.position - transform.position;
            target.y = 0;
            Vector3 tmp_target = Vector3.RotateTowards(transform.forward, target, Time.deltaTime, 0);
            transform.rotation = Quaternion.LookRotation(tmp_target);
            
        }
        else
        {
            if (animator.GetInteger("State") != 0)
            {
                animator.SetInteger("State", 0);
            }

        }
    }

}
