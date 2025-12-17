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
        Debug.Log("Quit");
        Application.Quit();
    }

    public void LoadSettings()
    {
        SceneManager.LoadScene("Settings_help");
    }

}