using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;
    public GameObject loadingPanel;

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void StartGame()
    {
        StartCoroutine(StartGameRoutine());
    }

    IEnumerator StartGameRoutine()
    {
        loadingPanel.SetActive(true);
        mainMenuPanel.SetActive(false);

        yield return null;
        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene("gameplay");
    }

public void QuitGame()
    {
        Application.Quit();
    }
}
