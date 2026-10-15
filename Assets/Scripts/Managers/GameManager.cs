using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    [Header("UI SFX")]
    public SimpleEffectsController uiEffects;
    public AudioClip backButtonClip;

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

    public void BackToMainMenu()
    {
        if (uiEffects != null)
        {
            uiEffects.PlayOneShot(backButtonClip, 0.6f);
        }
        Time.timeScale = 1f;
        SceneManager.LoadScene("main-menu");
    }
}
