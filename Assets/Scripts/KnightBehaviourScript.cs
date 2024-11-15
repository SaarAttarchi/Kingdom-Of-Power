using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class KnightBehaviourScript : MonoBehaviour
{
    // navigation components
    NavMeshAgent agent;
    Animator animator;
    LineRenderer line;

    // game objects
    public GameObject target;
    public GameObject player;
    public GameObject player_camera;
    public GameObject secretKeyPub;

    // UI elements
    public Text interactText;
    public Text[] dialouge;

    // dialouge settings
    bool isTalking = false;
    int framesCounter;
    int maxCounter = 400;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.isStopped = true;
        animator = GetComponent<Animator>();
        line = GetComponent<LineRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        movementToTarget();

        interactionWithPlayer();
       
        // display the dialouge
        if (isTalking)
        {
            framesCounter++;
            talk();
        }

    }


    // Handles knight movement toward the target
    private void movementToTarget()
    {
        float distance = Vector3.Distance(target.transform.position, transform.position);
        float distanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);

        // when close to the target stop and sit
        if (!agent.isStopped && distance < 0.5)
        {
            sitDown();
        }

        maintainSeatedAnimation(); // keep siiting

        // move to the target if player is nearby and keep until reaching the target
        if (distanceFromPlayer < 15 && distance > 2)
        {

            if (agent.isStopped)
            {
                animator.SetInteger("State", 1); // walking
                agent.SetDestination(target.transform.position);
                agent.isStopped = false;
                line.positionCount = agent.path.corners.Length; // set length of array of corners
                line.SetPositions(agent.path.corners);
            }

        }
        if (!agent.isStopped)
        {
            agent.SetDestination(target.transform.position);
            agent.isStopped = false;
            line.positionCount = agent.path.corners.Length; // set length of array of corners
            line.SetPositions(agent.path.corners);
        }
    }



    // stop the movement and change to sit animation
    private void sitDown()
    {
        agent.isStopped = true;
        transform.rotation = Quaternion.LookRotation(target.transform.forward);
        animator.SetInteger("State", 2); // sitting Animation
    }

    // change the knight's animation to remain sitting animation 
    private void maintainSeatedAnimation()
    {

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Stand To Sit") &&
        animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f)
        {
            animator.SetInteger("State", 3); // remain sitting
        }
    }

    // check if can interact with the player
    private void interactionWithPlayer()
    {
        float distanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);
        // check if the knight is in his target and in range of player
        if (distanceFromPlayer < 5 && agent.isStopped)
        {
            RaycastHit hit;
            if (Physics.Raycast(player_camera.transform.position, player_camera.transform.forward, out hit))
            {
                if (hit.collider.gameObject == gameObject && !isTalking) // if player cast hit the knight
                {
                    interactText.gameObject.SetActive(true); // show interaction text
                    if (Input.GetKey(KeyCode.E)) // starts conversation 
                    {
                        isTalking = true;
                        framesCounter = 0;
                    }
                }
                else
                {
                    interactText.gameObject.SetActive(false); // hide the text if player not lokking at the knight
                }
            }

        }
        else
        {
            interactText.gameObject.SetActive(false); // hide the text when the knight not in range or moving
        }
    }

    // displays the dialouge texts in order
    public void talk()
    {
        if (framesCounter < (maxCounter / 4))
        {
            dialouge[0].gameObject.SetActive(true);
            dialouge[1].gameObject.SetActive(false);
            dialouge[2].gameObject.SetActive(false);
            dialouge[3].gameObject.SetActive(false);
        }
        else if (framesCounter >= (maxCounter / 4) && (framesCounter < ((maxCounter * 2) / 4)))
        {
            dialouge[0].gameObject.SetActive(false);
            dialouge[1].gameObject.SetActive(true);
            dialouge[2].gameObject.SetActive(false);
            dialouge[3].gameObject.SetActive(false);
        }
        else if ((framesCounter >= ((maxCounter * 2) / 4)) && (framesCounter < ((maxCounter * 3) / 4)))
        {
            dialouge[0].gameObject.SetActive(false);
            dialouge[1].gameObject.SetActive(false);
            dialouge[2].gameObject.SetActive(true);
            dialouge[3].gameObject.SetActive(false);
        }
        else if ((framesCounter >= ((maxCounter * 3) / 4)) && (framesCounter < maxCounter))
        {
            dialouge[0].gameObject.SetActive(false);
            dialouge[1].gameObject.SetActive(false);
            dialouge[2].gameObject.SetActive(false);
            dialouge[3].gameObject.SetActive(true);
            secretKeyPub.gameObject.SetActive(true);
        }
        else
        {
            isTalking = false;
            dialouge[0].gameObject.SetActive(false);
            dialouge[1].gameObject.SetActive(false);
            dialouge[2].gameObject.SetActive(false);
            dialouge[3].gameObject.SetActive(false);
        }



    }

}
