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
    //new
    [SerializeField] private GameObject gameCompletionPopup;
    [SerializeField] private TextMeshProUGUI completionTimeText;
    [SerializeField] private TextMeshProUGUI totalBonusText;
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button backToMainMenuButton;


    // new
    private List<float> levelCompletionTimes = new List<float>();
    private int currentLevelIndex;
    private float levelStartTime;

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

        
        // new
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


        //// instruction hint popup at the begining of the different levels
        //if (SceneManager.GetActiveScene().name == "Level1")
        //{
        //    ShowHintBubble("Drag and drop the books into the correct outlines to complete the level");
        //}
        //else if (SceneManager.GetActiveScene().name == "Level2")
        //{
        //    ShowHintBubble("You can also rotate the books with the arrow keys to place them correctly!");
        //}
        //else if (SceneManager.GetActiveScene().name == "Level3")
        //{
        //    ShowHintBubble("Look out for bonus items, they will fit into the empty spots in the shelf!");
        //}
        if (SceneManager.GetActiveScene().name == "Level1")
        {
            Debug.Log("Showing Level 1 Hint");
            ShowHintBubble("Drag and drop the books into the correct outlines to complete the level");
        }
        else if (SceneManager.GetActiveScene().name == "Level2")
        {
            Debug.Log("Showing Level 2 Hint");
            ShowHintBubble("You can also rotate the books with the arrow keys to place them correctly!");
        }
        else if (SceneManager.GetActiveScene().name == "Level3")
        {
            Debug.Log("Showing Level 3 Hint");
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


    public void IncreaseScore(int amount)
    {
        booksFound += amount;
        booksFoundText.text = "Books Found: " + booksFound + "/" + booksInLevel;

        if (booksFound >= booksInLevel) // Check if all books are placed
        {
            levelCompleted = true;
            float completionTime = Time.time - levelStartTime; // Calculate completion time
            levelCompletionTimes.Add(completionTime); // Add to completion times list
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

        timeText.text = $"Time: {gameTime:F2} seconds"; // Display stopped time with 2 decimal places

        if (bonusInLevel > 0)
        {
            totalBonusText.gameObject.SetActive(true); // Ensure the bonus text is visible
            totalBonusText.text = $"Bonus Points Found: {bonusFound}/{bonusInLevel}";
        }

        nextLevelButton.onClick.RemoveAllListeners();   
        nextLevelButton.onClick.AddListener(LoadNextLevel);

        mainMenuButton.onClick.AddListener(LoadMainMenu);
        selectLevelButton.onClick.AddListener(LoadLevelSelect);
    }

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


    private void LoadLevelSelect()
    {
        Debug.Log("Select Level button clicked.");
        popupPanel.SetActive(false);
        levelSelectManager.ShowLevelSelectPanel();
    }

    private void LoadMainMenu()
    {
        Debug.Log("Main Menu button clicked.");
        SceneManager.LoadScene("Titlesceen");
    }

   
    public void AddBonusPoint(int amount)
    {
        bonusFound += amount;
        bonusFoundText.text = "Bonus Points: " + bonusFound + "/" + bonusInLevel;
    }


    private bool IsFinalLevel()
    {
        return currentLevelIndex == 5;
    }


    private void RestartGame()
    {
        levelCompletionTimes.Clear();
        SceneManager.LoadScene("Level1");
    }


}








