using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_InputField seedInputField;

    private GameManager gameManager;
    
    private void Start()
    {
       gameManager = GameManager.Instance; //create a reference to the game manager
    }

    public void StartGame() //when the player clicks 'start game'
    {
        string input = seedInputField.text; //get input from seedInputField

        if (string.IsNullOrWhiteSpace(input)) //if player didn't input anything
        {
            gameManager.useCustomSeed = false; //set useCustomSeed bool to false
        }
        else 
        {
            gameManager.customSeed = input; //set the custom seed to the players input
            gameManager.useCustomSeed = true; //set useCustomSeed bool to true
        }

        SceneManager.LoadScene("Game"); //load game scene
    }

    public void QuitGame() //when the player clicks 'quit game'
    {
        Debug.Log("QUIT");
        Application.Quit(); //quit the application
    }
}
