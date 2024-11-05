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

    private int booksFound = 0;
    private float startTime;
    private bool levelCompleted = false;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        startTime = Time.time;
        popupPanel.SetActive(false);  // Hide the popup panel initially
    }

    void Update()
    {
        if (!levelCompleted)
        {
            // Update the in-game timer while the level is still in progress
            float elapsedTime = Time.time - startTime;
            timeText.text = $"Time: {elapsedTime:F2} seconds";
        }
    }

    public void IncreaseScore(int amount)
    {
        booksFound += amount;
        booksFoundText.text = "Books Found: " + booksFound + "/3";

        if (booksFound >= 3)  // Assuming 3 is the total number of books needed to complete the level
        {
            ShowLevelCompletionPopup();
        }
    }

    private void ShowLevelCompletionPopup()
    {
        levelCompleted = true;
        popupPanel.SetActive(true);  // Show the popup panel

        // Display final score and time
        scoreText.text = "Level Completed!";
        float finalTime = Time.time - startTime;
        timeText.text = $"Time: {finalTime:F2} seconds";

        // Add button listeners
        nextLevelButton.onClick.AddListener(LoadNextLevel);
        mainMenuButton.onClick.AddListener(LoadMainMenu);
        selectLevelButton.onClick.AddListener(LoadLevelSelect);
    }

    private void LoadNextLevel()
    {
        Debug.Log("Next Level button clicked.");
        // Placeholder code for loading the next level
        // You could use SceneManager.LoadScene("NextLevelSceneName"); if you have multiple scenes set up
    }

    private void LoadMainMenu()
    {
        Debug.Log("Main Menu button clicked.");
        // Placeholder code for loading the main menu
        // Example: SceneManager.LoadScene("MainMenu");
    }

    private void LoadLevelSelect()
    {
        Debug.Log("Select Level button clicked.");
        // Placeholder code for loading the level selection screen
        // Example: SceneManager.LoadScene("LevelSelectScreen");
    }
}











//using System.Collections;
//using System.Collections.Generic;
//using TMPro;
//using UnityEngine;
//using UnityEngine.SocialPlatforms.Impl;

//public class GameManager : MonoBehaviour
//{
//    int booksFound = 0;

//    public static GameManager instance;

//    [SerializeField] TextMeshProUGUI booksFoundText;


//    private void Awake()
//    {
//        instance = this;
//    }


//    // Start is called before the first frame update
//    void Start()
//    {

//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }

//    public void IncreaseScore(int amount)
//    {
//        booksFound += amount;
//        booksFoundText.text = "Books Found: " + booksFound + "/3";
//    }
//}
