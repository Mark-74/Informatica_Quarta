using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonClick : MonoBehaviour
{
    public void Click()
    {
        SceneManager.LoadScene("Game");
    }
}
