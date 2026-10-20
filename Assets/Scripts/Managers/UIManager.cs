using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;
    public GameObject loadingPanel;

    [Header("UI SFX")]
    public SimpleEffectsController uiEffects;
    public AudioClip buttonClickClip;

    void Awake()
    {
        if (uiEffects == null)
        {
            uiEffects = GetComponent<SimpleEffectsController>();
        }
        if (uiEffects == null)
        {
            uiEffects = gameObject.AddComponent<SimpleEffectsController>();
            uiEffects.spatialBlend = 0f;
        }
    }

    public void OpenSettings()
    {
        PlayUIButtonSound();
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        PlayUIButtonSound();
        settingsPanel.SetActive(false);
    }

    public void StartGame()
    {
        PlayUIButtonSound();
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
        PlayUIButtonSound();
        Application.Quit();
    }

    void PlayUIButtonSound()
    {
        if (uiEffects != null)
        {
            uiEffects.PlayOneShot(buttonClickClip, 0.5f);
        }
    }
}
