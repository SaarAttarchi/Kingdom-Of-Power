using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestBehaviourScript : MonoBehaviour
{
    // chest atributes
    public bool chestlocked;
    Animator animator;
    AudioSource sound;
    public Text chestText;
    
    // game objects
    public GameObject player;
    public GameObject player_camera;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        sound = GetComponent<AudioSource>();
        chestlocked = true;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        canOpenChest(distance);

    }

    // if the conditiens are true to open the chest
    private void canOpenChest(float distance)
    {
        if (distance < 10 && chestlocked) // if in range and the chest is locked
        {
            RaycastHit hit;
            if (Physics.Raycast(player_camera.transform.position, player_camera.transform.forward, out hit))
            {
                if (hit.collider.gameObject == gameObject) // hit the chest
                {
                    chestText.gameObject.SetActive(true); // show pick up text
                    if (Input.GetKey(KeyCode.E))
                    {
                        openChest();
                    }
                }
                else
                {
                    chestText.gameObject.SetActive(false);
                }
            }

        }
        else
        {
            chestText.gameObject.SetActive(false); // hide the text when can't open 
            if (!chestlocked)
            {
                PersistentObjectManager.setHasKey(false);
            }
        }
    }
    
    // open the chest
    public void openChest()
    {
        // if player have the key 
        if (PersistentObjectManager.keyAcquired)
        {
            animator.SetBool("Locked", false);
            sound.Play();
            chestlocked = false;
            PersistentObjectManager.setHasKey(false); // destroy the key object

        }
    }


}
