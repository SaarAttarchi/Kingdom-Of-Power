using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour
{
    
    CharacterController controller;
    AudioSource footSteps;
    public static float PlayerHP;
    int healthRegainAmount = 5;

    // player movement setings
    float walk = 10;
    float sprint = 20;
    float speed;
    float angular_speed = 50;

    // game objects
    public GameObject player_camera;
    public GameObject sword_on_wall;
    public GameObject sword_in_hand;
    public GameObject chest;

    public GameObject bowAndArrow;
    public GameObject bowAndArrowInHand;

    // UI elements
    public Text pickText;
    public Text chestText;
    public Text keyText;
    public Text HealthText;

    // counters
    int framesCounter;
    int maxCounter = 150;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        footSteps = GetComponent<AudioSource>();
        PlayerHP = 100;
    }

    // Update is called once per frame
    void Update()
    {
        playerMovement();
        weaponPickUp();
        updateKey();

        // counter for health regain
        framesCounter++;
        if(framesCounter % maxCounter == 0)
        {
            regainHealth(healthRegainAmount); // Regenerates player health at intervals
        }


    }



    // handle the player's movement
    private void playerMovement()
    {
        float dx, dz;
        float rotation_about_y, rotation_about_x;


        // rotate camera about local x axis
        rotation_about_x = -Input.GetAxis("Mouse Y") * angular_speed + Time.deltaTime;
        player_camera.transform.Rotate(rotation_about_x, 0, 0);

        // rotate player abput local y axis
        rotation_about_y = Input.GetAxis("Mouse X") * angular_speed + Time.deltaTime;
        transform.Rotate(0, rotation_about_y, 0);

        // change speed when pressing shift
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = sprint;
        }
        else
        {
            speed = walk;
        }

        // Calculate movement vector
        dx = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        dz = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        Vector3 motion = new Vector3(dx, -1, dz);
        motion = transform.TransformDirection(motion);
        controller.Move(motion); // apply movement

        if (!(Mathf.Abs(dx) < 0.01f && Mathf.Abs(dz) < 0.01f))
        {
            if (!footSteps.isPlaying)
            {
                footSteps.Play(); // play footstep sound if moving
            }

        }
    }

    // handle the pick up of weapons
    private void weaponPickUp()
    {
        
        float swordDistance = Vector3.Distance(transform.position, sword_on_wall.transform.position); // sword distance
        float bowDistance = Vector3.Distance(transform.position, bowAndArrow.transform.position); // bow distance

        // check if in range of the weapons
        if (swordDistance < 15 || bowDistance < 15)
        {
            RaycastHit hit;
            if (Physics.Raycast(player_camera.transform.position, player_camera.transform.forward, out hit))
            {
                if (hit.collider.gameObject == sword_on_wall.gameObject || hit.collider.gameObject == bowAndArrow.gameObject)
                {
                    pickText.gameObject.SetActive(true); // show pick up text
                    if (Input.GetKey(KeyCode.E))
                    {
                        // pick up the weapon that the player is watching
                        if (hit.collider.gameObject == sword_on_wall.gameObject)
                        {
                            sword_on_wall.SetActive(false);
                            sword_in_hand.SetActive(true);
                            PersistentObjectManager.setHasSword(true);// set sword in hand
                            PersistentObjectManager.setHasSwordOnWall(false);
                            bowAndArrowInHand.SetActive(false);
                        }
                        else if (hit.collider.gameObject == bowAndArrow.gameObject)
                        {
                            bowAndArrow.SetActive(false);
                            bowAndArrowInHand.SetActive(true);
                            sword_in_hand.SetActive(false); // only one weapon can be in hand
                        }

                    }
                }
                else
                {
                    pickText.gameObject.SetActive(false); // hide the text when is not one of the objects
                }
            }
        }
        else
        {
            pickText.gameObject.SetActive(false); // hide the text when there is no object in range
        }
    }

    // switching between the weapons by pressing 1 or 2 
    public void switchWeapons()
    {
        if (PersistentObjectManager.hasSwordInHand)
        {
            if (Input.GetKey(KeyCode.Alpha1))
            {
                sword_on_wall.SetActive(false);
                sword_in_hand.SetActive(true);
                PersistentObjectManager.setHasSword(true);
                PersistentObjectManager.setHasSwordOnWall(false);
                bowAndArrowInHand.SetActive(false);


            }
            else if (Input.GetKey(KeyCode.Alpha2))
            {
                bowAndArrowInHand.SetActive(true);
                sword_in_hand.SetActive(false);
            }
        }
    }

    private void updateKey()
    {
        if (PersistentObjectManager.keyAcquired == true)
        {
            keyText.gameObject.SetActive(true);
        }
        else
        {
            keyText.gameObject.SetActive(false);
        }
    }

    // Reduces player health when taking damage
    public void takeDamage(int  damage)
    {
        PlayerHP -= damage;
        PlayerHP = Mathf.Max(PlayerHP, 0); // make sure the HP won't go below 0
        HealthText.text = "Health: " + PlayerHP.ToString();
        if (PlayerHP <= 0)
        {
            die();
        }
    }
    
    // place holder for player's death
    private void die()
    {
        sword_in_hand.SetActive(false);
    }
            
    // restore the player's health by a certain amount
    public void regainHealth(int regain)
    {
        PlayerHP += regain;
        PlayerHP = Mathf.Min(PlayerHP, 100); // stops at 100
        HealthText.text = "Health: " + PlayerHP.ToString();
    }


}
