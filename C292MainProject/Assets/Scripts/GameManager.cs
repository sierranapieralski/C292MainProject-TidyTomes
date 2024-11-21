using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

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
    [SerializeField] private LevelSelectManager levelSelectManager;
    [SerializeField] private TextMeshProUGUI bonusFoundText;
    [SerializeField] private int bonusInLevel;
    [SerializeField] private GameObject gameCompletionPopup;
    [SerializeField] private TextMeshProUGUI completionTimeText;
    [SerializeField] private TextMeshProUGUI totalBonusText;
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button backToMainMenuButton;


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
    private int bonusFound = 0;
    private int currentLevelIndex;
    private float levelStartTime;


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

        
        if (gameCompletionPopup != null)
        {
            gameCompletionPopup.SetActive(false);
        }

        levelStartTime = Time.time; // Record start time of current level
        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;


        // setting things up for the start of each level
        booksFound = 0;
        popupShown = false;
        levelCompleted = false;
        waitingForUserToPressDone = false;
        hintTimer = 0f;
        hintActive = false;
        booksFoundText.text = "Books Found: 0/" + booksInLevel;

        if (bonusInLevel > 0)
        {
            bonusFound = 0;
            bonusFoundText.text = "Bonus Points: 0/" + bonusInLevel;
        }


        // instruction hint popup at the begining of the different levels to provide useful information to the player
        if (SceneManager.GetActiveScene().name == "Level1")
        {
            ShowHintBubble("Drag and drop the books into the correct outlines to complete the level");
        }
        else if (SceneManager.GetActiveScene().name == "Level2")
        {
            ShowHintBubble("You can also rotate the books with the arrow keys to place them correctly!");
        }
        else if (SceneManager.GetActiveScene().name == "Level3")
        {
            ShowHintBubble("Look out for bonus items, they will fit into the empty spots in the shelf!");
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


    // Keeps track of the number of books found and increments the score after each book has been correctly placed
    public void IncreaseScore(int amount)
    {
        booksFound += amount;
        booksFoundText.text = "Books Found: " + booksFound + "/" + booksInLevel;

        if (booksFound >= booksInLevel) // Check if all books are placed
        {
            levelCompleted = true;
            float completionTime = Time.time - levelStartTime; 
            waitingForUserToPressDone = true;
            lastBookPlacedTime = Time.time;
            timeText.text = $"Time: {gameTime:F2} seconds"; // Display stopped time with 2 decimal places
        }
    }

    // After levels 1-4 are completed, this pop up appears showing the time it took the user to complete the level (and bonus points found of applicable the level
    // as well as a way to navigate to the next level, go back to the main level, or select a different level to navigate to
    private void ShowLevelCompletionPopup()
    {
        popupPanel.SetActive(true);
        doneButton.interactable = false; 
        popupShown = true;

        timeText.text = $"Time: {gameTime:F2} seconds"; // Display stopped time with 2 decimal places

        if (bonusInLevel > 0)
        {
            totalBonusText.gameObject.SetActive(true);
            totalBonusText.text = $"Bonus Points Found: {bonusFound}/{bonusInLevel}";
        }

        nextLevelButton.onClick.RemoveAllListeners();   
        nextLevelButton.onClick.AddListener(LoadNextLevel);

        mainMenuButton.onClick.AddListener(LoadMainMenu);
        selectLevelButton.onClick.AddListener(LoadLevelSelect);
    }

    // This pop up appears after the final level letting the user know that they have completed the game along with the their stats from level 5
    private void ShowGameCompletionPopup()
    {
        gameCompletionPopup.SetActive(true);
        doneButton.interactable = false;

        completionTimeText.text = $"Time: {gameTime:F2} seconds";

        totalBonusText.text = $"Total Bonus Points: {bonusFound}/{bonusInLevel}";

        playAgainButton.onClick.RemoveAllListeners();
        playAgainButton.onClick.AddListener(RestartGame);

        backToMainMenuButton.onClick.RemoveAllListeners();
        backToMainMenuButton.onClick.AddListener(LoadMainMenu);
    }

    // Makes the hint bubble visible when called and sets the message within the hint bubble to message
    public void ShowHintBubble(string message)
    {
        hintBubble.SetActive(true);
        hintBubbleText.text = message;
        hintActive = true;
        hintTimer = 0f;
    }

    // When the user completes the level by pressing the done button, this method eithernotifies the user that not all books are found so the level is not yet
    // completed or shows them the completion popup
    private void OnDoneButtonPressed()
    {
        if (!levelCompleted)
        {
            ShowHintBubble("Find all books before completing the level.");
        }
        else if (!popupShown) 
        {
            waitingForUserToPressDone = false;
            popupShown = true;

            if (IsFinalLevel()) 
            {
                ShowGameCompletionPopup();
            }
            else 
            {
                ShowLevelCompletionPopup();
            }
        }
    }

    // When the user presses the next button, this code takes them to the next level depending on what their current level is
    private void LoadNextLevel()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == "Level1")
        {
            Debug.Log("Loading Level 2...");
            SceneManager.LoadScene("Level2");
        }
        else if (currentSceneName == "Level2")
        {
            Debug.Log("Loading Level 3...");
            SceneManager.LoadScene("Level3");
        }
        else if (currentSceneName == "Level3")
        {
            Debug.Log("Loading Level 4...");
            SceneManager.LoadScene("Level4");
        }
        else if (currentSceneName == "Level4")
        {
            Debug.Log("Loading Level 5...");
            SceneManager.LoadScene("Level5");
        }
        else
        {
            Debug.Log("No more levels available.");
        }
    }

    // When the user presses the select button, the select popup appears
    private void LoadLevelSelect()
    {
        Debug.Log("Select Level button clicked.");
        popupPanel.SetActive(false);
        levelSelectManager.ShowLevelSelectPanel();
    }

    // When the user presses the main menu button, the main/title screen appears
    private void LoadMainMenu()
    {
        Debug.Log("Main Menu button clicked.");
        SceneManager.LoadScene("Titlesceen");
    }

   // This code keeps track of bonus points found in the level
    public void AddBonusPoint(int amount)
    {
        bonusFound += amount;
        bonusFoundText.text = "Bonus Points: " + bonusFound + "/" + bonusInLevel;
    }

    private bool IsFinalLevel()
    {
        return currentLevelIndex == 5;
    }

    // This code takes the user back to level 1 if they select the play again? button after level 5
    private void RestartGame()
    {
        //levelCompletionTimes.Clear();
        SceneManager.LoadScene("Level1");
    }
}








