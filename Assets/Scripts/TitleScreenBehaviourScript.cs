using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenBehaviourScript : MonoBehaviour
{
    // game objects
    public GameObject StartGameButton;
    public GameObject ContinueGameButton;
    public GameObject GamePanel;
    public GameObject StartPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // opening the menu
        if(Input.GetKeyDown(KeyCode.Escape)) 
        {
            openTitleMenu();
        }
    }

    // change so it want be staty but continue because the game has started
    private void openTitleMenu()
    {
        StartPanel.SetActive(true);
        ContinueGameButton.SetActive(true);
        StartGameButton.SetActive(false);
        GamePanel.SetActive(false);
    }

}
