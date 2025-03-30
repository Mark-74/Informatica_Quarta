using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonClick : MonoBehaviour
{
    public void MainMenuClick()
    {
        SceneManager.LoadScene("Game");
    }

    public void QuitClick()
    {
        Application.Quit();
    }

    public void RestartClick()
    {
        SceneManager.LoadScene("Game");
    }
}
