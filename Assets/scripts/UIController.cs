using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [Header("Health")]
    [SerializeField] public Slider playerHealthSlider;
    [SerializeField] public TMP_Text healthText;

    [Header("EXP")]
    [SerializeField] public Slider playerExpSlider;
    [SerializeField] public TMP_Text expText;

    [Header("Timer")]
    [SerializeField] private TMP_Text timerText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void UpdateHealthSlider()
    {
        playerHealthSlider.maxValue = PlayerController.Instance.playerMaxHealth;
        playerHealthSlider.value = PlayerController.Instance.playerCurrentHealth;

        healthText.text = "LIFE: " + PlayerController.Instance.playerCurrentHealth;
    }

    public void UpdateExpSlider()
    {
        playerExpSlider.value = PlayerController.Instance.exp;

        playerExpSlider.maxValue =
            PlayerController.Instance.playerLevels[
                PlayerController.Instance.currentLevel - 1
            ];
        
        expText.text = "LVL" + PlayerController.Instance.currentLevel;
    }

    public void UpdateTimer(float timer)
    {
        float min = Mathf.FloorToInt(timer / 60f);
        float sec = Mathf.FloorToInt(timer % 60f);

        timerText.text = min + ":" + sec.ToString("00");
    }
}