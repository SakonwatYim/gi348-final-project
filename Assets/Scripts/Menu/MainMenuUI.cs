using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
 
    [Header("Scenes")]
    [SerializeField] private string playSceneName = "Main";

    [Header("UI")]
    [SerializeField] private GameObject settingsPanel;

    // Called by UI Button: Play
    public void OnPlayPressed()
    {
        if (!string.IsNullOrEmpty(playSceneName))
        {
            

            SceneManager.LoadScene(playSceneName);
        }
        
    }

    // Called by UI Button: Settings (toggle panel)
    public void OnSettingsPressed()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(!settingsPanel.activeSelf);
        }
    }

    public void CloseSettingPanel()
    {         
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }

    // Called by UI Button: Exit
    public void OnExitPressed()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}