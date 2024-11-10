using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private TextMeshProUGUI booksFoundText;
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button selectLevelButton;
    [SerializeField] private int booksInLevel;
    [SerializeField] private Button doneButton; 
    [SerializeField] private GameObject hintBubble;
    [SerializeField] private TextMeshProUGUI hintBubbleText;

    private int booksFound = 0;
    private float startTime;
    private bool levelCompleted = false;
    private float gameTime;
    private bool popupShown = false;
    private float hintBubbleDuration = 5f;
    private float hintTimer = 0f;
    private bool hintActive = false;
    private bool waitingForUserToPressDone = false;
    private float lastBookPlacedTime;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        startTime = Time.time;
        popupPanel.SetActive(false);  // Hide the popup panel initially

        hintBubble.SetActive(false); // Hide the hint bubble initially
        doneButton.onClick.AddListener(OnDoneButtonPressed);


        // setting things up for the start of each level
        booksFound = 0;
        popupShown = false;
        levelCompleted = false;
        waitingForUserToPressDone = false;
        hintTimer = 0f;
        hintActive = false;
        booksFoundText.text = "Books Found: 0/" + booksInLevel;

        
        // instruction hint popup at the begining of the different levels
        if (SceneManager.GetActiveScene().name == "Level1")
        {
            ShowHintBubble("Drag and drop the books into the correct outlines to complete the level");
        }
        else if (SceneManager.GetActiveScene().name == "Level2")
        {
            ShowHintBubble("You can also rotate the books with the arrow keys to place them correctly!");
        }

    }

    void Update()
    {

        if (!levelCompleted)
        {
            gameTime = Time.time - startTime; // Keep track of game time
        }

        if (hintActive)
        {
            hintTimer += Time.deltaTime;
            if (hintTimer >= hintBubbleDuration)
            {
                hintBubble.SetActive(false);
                hintActive = false;
                hintTimer = 0f;
            }
        }

 

        if (waitingForUserToPressDone && !hintActive)
        {
            if (Time.time - lastBookPlacedTime >= hintBubbleDuration)
            {
                ShowHintBubble("Press the Done button to complete the level.");
            }
        }
    }


    public void IncreaseScore(int amount)
    {
        booksFound += amount;
        booksFoundText.text = "Books Found: " + booksFound + "/" + booksInLevel;

        if (booksFound >= booksInLevel) // Check if all books are placed
        {
            levelCompleted = true;
            waitingForUserToPressDone = true;
            lastBookPlacedTime = Time.time;
            timeText.text = $"Time: {gameTime:F2} seconds"; // Display stopped time with 2 decimal places
        }
    }

    private void ShowLevelCompletionPopup()
    {
        popupPanel.SetActive(true);
        doneButton.interactable = false; // Disable the Done button 
        popupShown = true;

        //float finalTime = Time.time - startTime;
        timeText.text = $"Time: {gameTime:F2} seconds"; // Display stopped time with 2 decimal places

        nextLevelButton.onClick.RemoveAllListeners();   // clears any listeners from previous levels
        nextLevelButton.onClick.AddListener(LoadNextLevel);
        mainMenuButton.onClick.AddListener(LoadMainMenu);
        selectLevelButton.onClick.AddListener(LoadLevelSelect);
    }

    public void ShowHintBubble(string message)
    {
        hintBubble.SetActive(true);
        hintBubbleText.text = message;
        hintActive = true;
        hintTimer = 0f;
    }

    private void OnDoneButtonPressed()
    {
        if (!levelCompleted)
        {
            ShowHintBubble("Find all books before completing the level.");
        }
        else if (!popupShown) // Check if the popup has already been shown
        {
            waitingForUserToPressDone = false;
            ShowLevelCompletionPopup();
        }
    }


    private void LoadNextLevel()
    {
        Debug.Log("Next Level button clicked.");
        SceneManager.LoadScene("Level2");
    }


    private void LoadLevelSelect()
    {
        Debug.Log("Select Level button clicked.");
    }

    private void LoadMainMenu()
    {
        Debug.Log("Main Menu button clicked.");
    }
}








