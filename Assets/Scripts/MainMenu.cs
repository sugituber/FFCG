using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void TrackSelect()
    {
        Debug.Log("Start button clicked!");
        SceneManager.LoadScene("TrackSelection");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    public void LoadSettings()
    {
        SceneManager.LoadScene("Settings_help");
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("TimeTest");
    }
}