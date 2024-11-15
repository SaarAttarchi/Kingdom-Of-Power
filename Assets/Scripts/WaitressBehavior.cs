using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaitressBehavior : MonoBehaviour
{
    // navigation components
    NavMeshAgent agent;
    Animator animator;
    LineRenderer line;
    
    // targets game objects
    public GameObject target;
    public GameObject Point1;
    public GameObject Point2;
    public GameObject Point3;
    public GameObject standindSpot;
    private int state = 0; // the waitress current state

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
        // the distance from current target
        float distance = Vector3.Distance(target.transform.position, transform.position);

        switch(state)
        {
            case 1: // going upstairs
                if (!agent.isStopped && distance < 1)
                {
                    state = 2; // Transition to moving to standing spot
                    target.transform.position = standindSpot.transform.position;
                    agent.SetDestination(target.transform.position);
                }
                break;

            case 2: // going back to the standing spot
                if (!agent.isStopped && distance < 1)
                {
                    state = 3; // Transition to moving to Point1
                    target.transform.position = Point1.transform.position;
                    agent.SetDestination(target.transform.position);
                }
                break;

            case 3: // Transition to moving to Point1
                if (!agent.isStopped && distance < 1)
                {
                    StartCoroutine(StopAtPoint()); // Call coroutine to pause
                    state = 4; // Transition to moving to Point2
                    animator.SetInteger("State", 0); // idle
                    target.transform.position = Point2.transform.position;
                    agent.SetDestination(target.transform.position);
                }
                break;
            
            case 4: // Transition to moving to Point2
                if (!agent.isStopped && distance < 1)
                {
                    StartCoroutine(StopAtPoint()); // Call coroutine to pause
                    state = 5; // Transition to moving to standing spot
                    animator.SetInteger("State", 0); // idle
                    target.transform.position = standindSpot.transform.position;
                    agent.SetDestination(target.transform.position);
                }
                break;

            case 5: // Transition to moving to standing spot
                if (!agent.isStopped && distance < 1)
                {
                    agent.isStopped = true;
                    // Make the waitress face back toward the player
                    Vector3 directionToFace = (transform.position - target.transform.position).normalized;
                    Quaternion lookRotation = Quaternion.LookRotation(directionToFace);
                    transform.rotation = lookRotation;
                    animator.SetInteger("State", 0); // idle
                    target.transform.position = Point3.transform.position;
                    state = 0; // reset the states
                }
                break;
        }


        // Coroutine to pause at points
        IEnumerator StopAtPoint()
        {
            agent.isStopped = true;
            yield return new WaitForSeconds(1); // Wait for 1 second
            animator.SetInteger("State", 1); // Resume walking animation
            agent.isStopped = false;
            agent.SetDestination(target.transform.position); // Resume to the next destination
        }




        // starts moving 
        if (Input.GetKeyDown(KeyCode.T))
        {

            if (agent.isStopped)
            {
                animator.SetInteger("State", 1); // walking
                agent.SetDestination(target.transform.position);
                agent.isStopped = false;
                line.positionCount = agent.path.corners.Length; // set length of array of corners
                line.SetPositions(agent.path.corners);
                state = 1;
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
}
