using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("Pause UI")]
    [SerializeField] private GameObject pausePanel;

    private bool isPaused;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        ShowPausePanel(!isPaused);
    }

    public void ShowPausePanel(bool show)
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(show);
        }

        if (show)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    private void PauseGame()
    {
        if (isPaused) return;
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        if (!isPaused) return;
        Time.timeScale = 1f;
        isPaused = false;
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
