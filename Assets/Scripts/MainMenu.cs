using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("TrackSelection");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
