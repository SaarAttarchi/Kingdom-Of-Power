using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinBehaviourScript : MonoBehaviour
{
    public static int numCoins = 0; // one variable per CoinBehavoir 

    // game objects
    public GameObject player;
    public GameObject parent;
    public Text coinText;
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
        if (other.gameObject == player.gameObject) // when player touch coin it dissapear and add to the number of coins
        {
            numCoins++;
            coinText.text = "Gold: " + numCoins.ToString();
            gameObject.SetActive(false);
            AudioSource sound = parent.GetComponent<AudioSource>();
            sound.Play();
        }
    }
}
