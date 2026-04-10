using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    public void GameOver()
    {
        StartCoroutine(ShowGameOverScreen());
    }

    IEnumerator ShowGameOverScreen()
    {
        yield return new WaitForSeconds(1.5f);
        UIController.Instance.gameOverPanel.SetActive(true);
        UIController.Instance.pauseButton.SetActive(false);

        // UIController.Instance.pauseButton.SetActive(false);
    }

    public void Restart()
    {
        SceneManager.LoadScene("MainScene");
        Time.timeScale = 1f;
    }

    public void Pause()
    {
        if (UIController.Instance.pausePanel.activeSelf == false && UIController.Instance.gameOverPanel.activeSelf == false)
        {
            UIController.Instance.pausePanel.SetActive(true);

            UIController.Instance.pauseButton.SetActive(false);
            // UIController.Instance.resumeButton.SetActive(true);

            Time.timeScale = 0f;
        }
        else
        {
            UIController.Instance.pausePanel.SetActive(false);

            UIController.Instance.pauseButton.SetActive(true);
            // UIController.Instance.resumeButton.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }

}
