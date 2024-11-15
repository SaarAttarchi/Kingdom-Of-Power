using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class SwordBehaviourScript : MonoBehaviour
{
    Animator animator;
    AudioSource swingSound;

    // sword atributes
    public static bool isBlocking = false;
    public static int SwordDamage = 20;
    public static int swordXP = 0;
    public static int swordLvl = 1;
    private int swordDamageIncrease = 10;

    // game objects
    public GameObject explosion;
    public GameObject eye;
    public GameObject Target;
    public GameObject HitBox;
    public GameObject FireSwordStats;
    public GameObject levelUpMessage;

    // UI elements
    public Text XPText;
    public Text LvlText;
    public Text LvlUpText;
    public Text damagelUpText;

    // special attack variables
    float speed = 10;
    bool isFlameOn;
    int framesCounter;
    int maxCounter = 300;

    // Start is called before the first frame update
    void Start()
    {
     animator = GetComponent<Animator>();  
     swingSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PersistentObjectManager.hasSwordInHand)
        {
            FireSwordStats.gameObject.SetActive(true);
            
            // actions 
            swordActions();
            handleFlameMovement();
            
            // show the lvl up messeage if needed
            displayLvlUpMessage();

        }
        else
        {
            FireSwordStats.gameObject.SetActive(false);
        }

    }


    // set the sword attack 
    private void swordActions()
    {
        // attack
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            animator.SetInteger("State", 1); // attack animation

        }
        // block
        else if (Input.GetMouseButton(1))
        {

            animator.SetInteger("State", 2); // block animation
            isBlocking = true;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            animator.SetInteger("State", 0); // reset to idle
            isBlocking = false;
            swingSound.Play();
        }
        // special attack
        else if (Input.GetKey(KeyCode.R))
        {
            flameAttack();
        }
        else
        {
            animator.SetInteger("State", 0); // reset to idle
        }
        
        // make sound and active the hit box only when attacking
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("SwordSwingAnimation"))
        {
            swingSound.Play();
            HitBox.SetActive(true);

        }
        else
        {
            HitBox.SetActive(false);
        }
    }

    // set a point for the explosion
    private void flameAttack()
    {
        RaycastHit hit;
        if (Physics.Raycast(eye.transform.position, eye.transform.forward, out hit))
        {
            Target.transform.position = hit.point;
            float distance = Vector3.Distance(transform.position, Target.transform.position);
            if (distance < 80) // if not far enough
            {
                animator.SetInteger("State", 3); // flame attack animation
                isFlameOn = true;
                explosion.SetActive(true);
                explosion.transform.LookAt(Target.transform);
            }

        }
    }
    // move a flame explosion toward a spesific point
    private void handleFlameMovement()
    {
        if (isFlameOn)
        {
            Vector3 direction = (Target.transform.position - explosion.transform.position).normalized;
            explosion.transform.position += direction * speed * Time.deltaTime;
            if (Vector3.Distance(explosion.transform.position, Target.transform.position) < 0.1f) // stops the explosion when reaching the target
            {
                explosion.transform.position = transform.position;
                explosion.SetActive(false);
                isFlameOn = false;
            }
        }
    }

    // gain xp from an enemy 
    public void gainXp(int XP)
    {
        swordXP += XP;
        XPText.text = "Soword XP: " + swordXP.ToString();
        if(swordXP / (100 * swordLvl) >= 1) // if the xp points equal to a certain level then level up
        {
            levelUp();
        }
    }

    // updating the sword stats when leveling up
    public void levelUp() 
    {
        framesCounter = 1;
        swordLvl++;
        SwordDamage += swordDamageIncrease;

        LvlText.text = "Soword Lvl: " + swordLvl.ToString();
        levelUpMessage.SetActive(true);

        // lvl up UI texts
        LvlUpText.text = "Soword Lvl: "  + (swordLvl - 1).ToString() +  " -> " + swordLvl.ToString();
        damagelUpText.text = "Sowrd damage: " + (SwordDamage - 10).ToString() + " -> " + SwordDamage.ToString();

    }
    
    // show the messeage for a certain time 
    private void displayLvlUpMessage()
    {
        if (framesCounter >= 1)
        {
            framesCounter++;
            if (framesCounter >= maxCounter)
            {
                levelUpMessage.SetActive(false);
                framesCounter = 0;
            }
        }
    }





}
