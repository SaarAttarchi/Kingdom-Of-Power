using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class PersistentObjectManager : MonoBehaviour
{
    // singleton Instance
    public static PersistentObjectManager Instance = null;

    // player stats
    public static int numGoldCoins = 0;
    public static float numHealth = 100;
    public static float swordXP = 0;
    public static int swordLvl = 1;

    // game objects states
    public static bool gameStart = false;
    public static bool hasSwordInHand = false;
    public static bool hasSwordOnWall = true;
    public static bool keyAcquired = false;
    public static bool exitCave = false;

    // game objects
    public GameObject sword_in_hand;
    public GameObject sword_on_wall;
    public GameObject Player;
    public GameObject StartPanel;
    public GameObject ExitCaveLocation;

    // UI elements
    public Text numCoinsText;
    public Text HealthText;
    public Text SwordXPText;
    public Text SwordLvlText;
    public Text keyText;
    private void Awake()
    {
        if (Instance == null)// this for the first time
        {
            Instance = this;
            // keep the original instance
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // initialize the UI
        updateUI();
        sword_in_hand.SetActive(hasSwordInHand);
        sword_on_wall.SetActive(hasSwordOnWall);
        StartPanel.SetActive(!gameStart);

        if (exitCave && ExitCaveLocation != null)
        {
            Player.transform.position = ExitCaveLocation.transform.position;
        }


    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frames
    void Update()
    {

    }

    private void updateUI()
    {
        numCoinsText.text = "Gold: " + numGoldCoins;
        HealthText.text = "Health: " + numHealth;
        SwordXPText.text = "Sword XP: " + swordXP;
        SwordLvlText.text = "Sword Lvl: " + swordLvl;
        keyText.gameObject.SetActive(keyAcquired);
    }


    // start game 
    public static void setGameStart(bool isStart)
    {
        gameStart = isStart;
    }

    // player data menegment
    public static void setHealth(float health)
    {
        numHealth = health;
    }
    public static void setGoldCoins(int coins)
    {
        numGoldCoins = coins;
    }
    public static void setSwordXP(int XP)
    {
        swordXP = XP;
    }
    public static void setSwordLvl(int lvl)
    {
        swordLvl = lvl;
    }
    public static void setHasSword(bool hasSword)
    {
        hasSwordInHand = hasSword;
    }
    public static void setHasSwordOnWall(bool hasSword)
    {
        hasSwordOnWall = hasSword;
    }
    public static void setHasKey(bool hasKey)
    {
        keyAcquired = hasKey;
    }

    // exit cave 
    public static void setExitCave(bool exit)
    {
        exitCave = exit;
    }
}
