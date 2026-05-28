using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIOverlayController : MonoBehaviour
{
    public static UIOverlayController Instance;

    [Header("Panels")]
    [SerializeField] public GameObject gameOverPanel;
    [SerializeField] public GameObject levelUpPanel;
    [SerializeField] public GameObject pausePanel;

    [Header("Buttons")]
    [SerializeField] public GameObject pauseButton;
    [SerializeField] public GameObject resumeButton;

    [Header("Level Up")]
    public LevelUpbutton[] levelUpButtons;

    private bool isPaused;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void OpenLevelUpPanel()
    {
        levelUpPanel.SetActive(true);
        PauseGame();
    }

    public void CloseLevelUpPanel()
    {
        levelUpPanel.SetActive(false);
        ResumeGame();
    }

    public void OpenPausePanel()
    {
        pausePanel.SetActive(true);

        pauseButton.SetActive(false);
        resumeButton.SetActive(true);

        PauseGame();
    }

    public void ClosePausePanel()
    {
        pausePanel.SetActive(false);

        pauseButton.SetActive(true);
        resumeButton.SetActive(false);

        ResumeGame();
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);

        PauseGame();
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
    }
}