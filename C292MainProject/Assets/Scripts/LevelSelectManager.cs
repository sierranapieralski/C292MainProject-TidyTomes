using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectManager : MonoBehaviour
{
    [SerializeField] private GameObject levelSelectPanel;
    [SerializeField] private Button level1Button;
    [SerializeField] private Button level2Button;
    [SerializeField] private Button level3Button;
    [SerializeField] private Button level4Button;
    [SerializeField] private Button level5Button;
    //[SerializeField] private Button closeButton;

    private void Start()
    {
        // Make sure the panel is hidden initially
        levelSelectPanel.SetActive(false);

        // Assign button listeners for each level
        level1Button.onClick.AddListener(() => LoadLevel("Level1"));
        level2Button.onClick.AddListener(() => LoadLevel("Level2"));
        level3Button.onClick.AddListener(() => LoadLevel("Level3"));

        // Assign a listener to the close button to hide the panel
        //closeButton.onClick.AddListener(HideLevelSelectPanel);
    }

    public void ShowLevelSelectPanel()
    {
        levelSelectPanel.SetActive(true);
    }

    public void HideLevelSelectPanel()
    {
        levelSelectPanel.SetActive(false);
    }

    private void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}
