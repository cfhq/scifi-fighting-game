using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("main-menu");
    }
}
