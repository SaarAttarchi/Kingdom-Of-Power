using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalBehaviourScript : MonoBehaviour
{
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
        if (SceneManager.GetActiveScene().buildIndex == 0) // enter cave with all current player's data
        {
            PersistentObjectManager.setGoldCoins(CoinBehaviourScript.numCoins);
            PersistentObjectManager.setHealth(PlayerBehaviour.PlayerHP);
            PersistentObjectManager.setSwordXP(SwordBehaviourScript.swordXP);
            PersistentObjectManager.setSwordLvl(SwordBehaviourScript.swordLvl);
            SceneManager.LoadScene(1);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1) // exit cave with all current player's data
        {
            PersistentObjectManager.setExitCave(true);
            PersistentObjectManager.setGoldCoins(CoinBehaviourScript.numCoins);
            PersistentObjectManager.setHealth(PlayerBehaviour.PlayerHP);
            PersistentObjectManager.setSwordXP(SwordBehaviourScript.swordXP);
            PersistentObjectManager.setSwordLvl(SwordBehaviourScript.swordLvl);
            SceneManager.LoadScene(0);
        }
    }
}
