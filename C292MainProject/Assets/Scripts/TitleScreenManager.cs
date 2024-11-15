using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{

    [SerializeField] private Button startButton;


    void Start()
    {
        startButton.onClick.AddListener(StartGame);
    }

    void StartGame()
    {
        // Load Level 1 when the Start button is clicked
        Debug.Log("StartGame method called");
        SceneManager.LoadScene("Level1");
    }
}
