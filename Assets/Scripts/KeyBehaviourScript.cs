using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyBehaviourScript : MonoBehaviour
{
    // game objects
    public GameObject player;
    public GameObject parent;
    
    // UI text
    public Text keyText;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player.gameObject) // when player touch the key it dissapear and it is written for the player
        {
            PersistentObjectManager.setHasKey(true);
            AudioSource sound = parent.GetComponent<AudioSource>();
            sound.Play();
            gameObject.SetActive(false);
        }
    }
}
