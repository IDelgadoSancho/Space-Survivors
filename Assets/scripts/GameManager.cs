using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public float gameTime;
    public bool gameActive;
    public int enemiesKilled;
    public AudioSource music;

    private void Awake()
    {
        Screen.orientation = ScreenOrientation.LandscapeRight;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToLandscapeRight = true;

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        gameActive = true;
    }

    void Update()
    {
        if (gameActive)
        {
            gameTime += Time.deltaTime;
            UIController.Instance.UpdateTimer(gameTime);

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Pause();
            }
        }
    }

    public void GameOver()
    {
        gameActive = false;
        StartCoroutine(ShowGameOverScreen());

        AudioController.Instance.SetMusicVolume(0.0f);

        float time = gameTime;
        int kills = enemiesKilled;

        UIOverlayController.Instance.UpdatePoints(time, kills);

    }

    IEnumerator ShowGameOverScreen()
    {
        yield return new WaitForSeconds(3f);
        UIOverlayController.Instance.gameOverPanel.SetActive(true);
        UIOverlayController.Instance.pauseButton.SetActive(false);
        AudioController.Instance.PlaySound(AudioController.Instance.gameOver);

        UIController.Instance.playerHealthSlider.gameObject.SetActive(false);
        UIController.Instance.healthText.gameObject.SetActive(false);

        UIController.Instance.playerExpSlider.gameObject.SetActive(false);
        UIController.Instance.expText.gameObject.SetActive(false);

        // UIController.Instance.pauseButton.SetActive(false);
    }

    public void Restart()
    {
        SceneManager.LoadScene("MainScene");
        AudioController.Instance.SetMusicVolume(1f);
        Time.timeScale = 1f;
    }

    public void Pause()
    {
        if (UIOverlayController.Instance.pausePanel.activeSelf == false && UIOverlayController.Instance.gameOverPanel.activeSelf == false)
        {
            UIOverlayController.Instance.pausePanel.SetActive(true);
            UIOverlayController.Instance.pauseButton.SetActive(false);
            // UIController.Instance.resumeButton.SetActive(true);
            music.Pause();
            Time.timeScale = 0f;
            AudioController.Instance.PlaySound(AudioController.Instance.pause);
        }
        else
        {
            UIOverlayController.Instance.pausePanel.SetActive(false);
            UIOverlayController.Instance.pauseButton.SetActive(true);
            // UIController.Instance.resumeButton.SetActive(false);
            music.UnPause();
            Time.timeScale = 1f;
            AudioController.Instance.PlaySound(AudioController.Instance.unpause);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        AudioController.Instance.SetMusicVolume(1f);
        Time.timeScale = 1f;
    }

}
