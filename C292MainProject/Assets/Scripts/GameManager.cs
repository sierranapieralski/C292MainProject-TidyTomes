using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    //new
    private float gameTime;
    private bool popupShown = false;
    private float hintBubbleDuration = 10f;
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

        //new
        hintBubble.SetActive(false); // Hide the hint bubble initially
        doneButton.onClick.AddListener(OnDoneButtonPressed);
    }

    void Update()
    {

        if (!levelCompleted)
        {
            gameTime = Time.time - startTime; // Keep track of game time
        }

        // Hint bubble logic
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

        //if (waitingForUserToPressDone && !hintActive)
        //{
        //    if (Time.time - startTime >= hintBubbleDuration)
        //    {
        //        ShowHintBubble("Press the Done button to complete the level.");
        //    }
        //}

        if (waitingForUserToPressDone && !hintActive)
        {
            if (Time.time - lastBookPlacedTime >= hintBubbleDuration)
            {
                ShowHintBubble("Press the Done button to complete the level.");
            }
        }

        // new
        //if (hintActive)
        //{
        //    hintTimer += Time.deltaTime;
        //    if (hintTimer >= hintBubbleDuration)
        //    {
        //        hintBubble.SetActive(false);
        //        hintActive = false;
        //        hintTimer = 0f;
        //    }
        //}

        //if (waitingForUserToPressDone && !hintActive)
        //{
        //    if (Time.time - startTime >= hintBubbleDuration)
        //    {
        //        ShowHintBubble("Press the Done button to complete the level.");
        //    }
        //}
    }

    //public void IncreaseScore(int amount)
    //{
    //    booksFound += amount;
    //    booksFoundText.text = "Books Found: " + booksFound + "/3";

    //    if (booksFound >= booksInLevel)  // Assuming 3 is the total number of books needed to complete the level **************************************
    //    {
    //        levelCompleted = true;
    //        ShowLevelCompletionPopup();
    //        //new
    //        startTime = Time.time;
    //    }
    //}

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


    //private void ShowLevelCompletionPopup()
    //{
    //    //levelCompleted = true;
    //    if (levelCompleted)
    //    {
    //        popupPanel.SetActive(true);  // Show the popup panel

    //        // Display final score and time
    //        scoreText.text = "Level Completed!";
    //        float finalTime = Time.time - startTime;
    //        timeText.text = $"Time: " + finalTime + " seconds";

    //        // Add button listeners
    //        nextLevelButton.onClick.AddListener(LoadNextLevel);
    //        mainMenuButton.onClick.AddListener(LoadMainMenu);
    //        selectLevelButton.onClick.AddListener(LoadLevelSelect);
    //    }
    //}

    ////new
    //private void ShowHintBubble(string message)
    //{
    //    hintBubble.SetActive(true);
    //    hintBubbleText.text = message; // Set the hint text dynamically
    //    hintActive = true;
    //    hintTimer = 0f; // Reset hint timer
    //}

    ////new
    //private void OnDoneButtonPressed()
    //{
    //    if (!levelCompleted)
    //    {
    //        ShowHintBubble("Find all books before completing the level.");
    //    }
    //    else
    //    {
    //        waitingForUserToPressDone = false;
    //        ShowLevelCompletionPopup();
    //    }
    //}

    private void ShowLevelCompletionPopup()
    {
        popupPanel.SetActive(true);
        doneButton.interactable = false; // Disable the Done button to prevent further presses
        popupShown = true;

        float finalTime = Time.time - startTime;
        //timeText.text = $"Time: " + finalTime + " seconds";
        timeText.text = $"Time: {gameTime:F2} seconds";

        nextLevelButton.onClick.AddListener(LoadNextLevel);
        mainMenuButton.onClick.AddListener(LoadMainMenu);
        selectLevelButton.onClick.AddListener(LoadLevelSelect);
    }

    private void ShowHintBubble(string message)
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








