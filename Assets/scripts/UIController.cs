using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    [SerializeField] private Slider playerHealthSlider;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text timerText;

    [SerializeField] private Slider playerExpSlider;
    [SerializeField] private TMP_Text ExpText;

    public LevelUpbutton[] levelUpButtons;

    public GameObject gameOverPanel;
    public GameObject levelUpPanel;
    public GameObject pausePanel;
    public GameObject pauseButton;
    public GameObject resumeButton;



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void UpdateHealthSlider()
    {
        playerHealthSlider.maxValue = PlayerController.Instance.playerMaxHealth;
        playerHealthSlider.value = PlayerController.Instance.playerCurrentHealth;
        // healthText.text = playerHealthSlider.value + "/" + playerHealthSlider.maxValue;

    }

    public void UpdateExpSlider()
    {  
        playerExpSlider.value = PlayerController.Instance.exp;
        playerExpSlider.maxValue = PlayerController.Instance.playerLevels[PlayerController.Instance.currentLevel - 1];
        // healthText.text = playerHealthSlider.value + "/" + playerHealthSlider.maxValue;

    }

    public void UpdateTimer(float timer)
    {
        float min = Mathf.FloorToInt(timer / 60f);
        float sec = Mathf.FloorToInt(timer % 60f);

        timerText.text = min + ":" + sec.ToString("00");
    }

    public void levelUpPanelOpen()
    {
        levelUpPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void levelUpPanelClose()
    {
        levelUpPanel.SetActive(false);
        Time.timeScale = 1f;
    }



}
