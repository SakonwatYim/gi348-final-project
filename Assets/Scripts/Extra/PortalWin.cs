using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalWin : MonoBehaviour
{
    private bool hasTriggered;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            hasTriggered = true;

            // play portal SFX
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayPortal();
            }

            StartCoroutine(LoadSceneDelayed("MainMenu", 3f));
        }
    }

    private IEnumerator LoadSceneDelayed(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
}
