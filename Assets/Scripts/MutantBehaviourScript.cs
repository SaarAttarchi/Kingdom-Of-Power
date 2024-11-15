using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MutantBehaviourScript : MonoBehaviour
{
    NavMeshAgent agent;
    Animator animator;
    LineRenderer line;
    AudioSource breathingSound;
    public AudioSource sweepSound;

    // game objects
    public GameObject parent;
    public GameObject target;
    public GameObject player;
    public GameObject sword;
    public GameObject punch;
    public GameObject sweep;
    public GameObject HealthBar;
    public Slider HealthSlider;

    // health and damage atributes
    bool alive = true;
    int maxHP = 200;
    int currentHP = 200;
    public int punchDamage = 10;
    public int sweepDamage = 20;

    // counters
    int framesCounter;
    int maxCounter = 200;
    int numPunchHit;
    int numSweepHit;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.isStopped = true;
        animator = GetComponent<Animator>();
        line = GetComponent<LineRenderer>();
        breathingSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PersistentObjectManager.exitCave == true)
        {
            float distance = Vector3.Distance(target.transform.position, transform.position);
            gameObject.SetActive(true);

            if (distance < 40) // start combat if in range
            {
                HealthBar.SetActive(true);
                framesCounter++;
                if (alive)
                {
                    mutantCombat(distance);
                }
                else if (framesCounter > maxCounter)
                {
                    // destroy the health and mutant objects after certain time
                    gameObject.SetActive(false);
                    HealthBar.SetActive(false);

                }
            }
            else
            {
                HealthBar.SetActive(false);
            }
        }
        else
        {
            gameObject.SetActive(false);
            HealthBar.SetActive(false);
        }



        // check for death state
        if (currentHP <= 0 && alive)
        {
            death();

        }


    }



    // set the mutant movement and attacks acording to the players's range
    private void mutantCombat(float distance)
    {
        if (!breathingSound.isPlaying)
        {
            breathingSound.Play();
        }
        // in attack range
        if (distance < 5)
        {
            agent.isStopped = true;
            if (framesCounter > 75)
            {
                sweepSound.Stop();
                int attackType = Random.Range(2, 4); // Randomly choose between punch or sweep
                attack(attackType);
                framesCounter = 0;
            }

        }


        else if (distance < 20 && distance > 5)
        {
            moveToPlayer();

        }
        else if (distance > 20)
        {
            // stop the movement
            agent.isStopped = true;
            animator.SetInteger("State", 0); // go back to idle

        }

    }

    // move the mutant to the player
    private void moveToPlayer()
    {
        if (agent.isStopped)
        {
            animator.SetInteger("State", 1); // walking animation 
            agent.SetDestination(target.transform.position);
            agent.isStopped = false;
            line.positionCount = agent.path.corners.Length; // set length of array of corners
            line.SetPositions(agent.path.corners);
            // sets them off when moving
            sweep.SetActive(false);
            punch.SetActive(false);

        }
    }

    private void attack(int attackType)
    {
        // activates only the needed hitbox for the attack
        if (attackType == 2)
        {
            punch.SetActive(true);
            sweep.SetActive(false);
            animator.SetInteger("State", 2); // punch Animation
            numPunchHit = 0;
        }
        else
        {
            sweep.SetActive(true);
            punch.SetActive(false);
            animator.SetInteger("State", 3); // sweep Animation
            sweepSound.PlayDelayed(0.3f); // play soundeffect
            numSweepHit = 0;
        }

    }

    private void death()
    {
        AudioSource deathSound = parent.GetComponent<AudioSource>();
        deathSound.Play();

        SwordBehaviourScript Sword = sword.GetComponentInParent<SwordBehaviourScript>();
        Sword.gainXp(100); // player gets xp from the mutant

        alive = false;
        animator.SetInteger("State", 10); // death animation
        framesCounter = 0;
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == sword.gameObject)
        {
            mutantTakeDamage(SwordBehaviourScript.SwordDamage); // taking damage from sword
        }

        if (other.CompareTag("Player") && other == player.GetComponent<Collider>())
        {
            // punch attack damage
            if (punch.GetComponent<Collider>().bounds.Intersects(other.bounds))
            {
                PlayerBehaviour Player = player.GetComponent<PlayerBehaviour>();
                if (punch.activeSelf)
                {
                    if (!SwordBehaviourScript.isBlocking && numPunchHit < 3) // if the colision happens more then 3 times by accident
                    {
                        Player.takeDamage(punchDamage);
                        numPunchHit++;
                    }

                }
            }
            // sweep attack damage
            else if (sweep.GetComponent<Collider>().bounds.Intersects(other.bounds))
            {
                PlayerBehaviour Player = player.GetComponent<PlayerBehaviour>();
                if (sweep.activeSelf)
                {
                    if (!SwordBehaviourScript.isBlocking && numSweepHit < 1) // if the colision happens more then 1 time by accident
                    {
                        Player.takeDamage(sweepDamage);
                        numSweepHit++;
                    }

                }
            }
        }

    }

    public void mutantTakeDamage(int damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Max(currentHP, 0); // make sure the HP won't go below 0
        HealthSlider.value = (currentHP / (float)maxHP);
    }




}
