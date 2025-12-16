using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class FinishMenu : MonoBehaviour
{
    public static FinishMenu instance;

    [Header("UI")]
    public GameObject finishOverlay;
    public RectTransform finishPanel;

    public TextMeshProUGUI resultText;
    public TextMeshProUGUI yourTimeText;
    public TextMeshProUGUI timeToBeatText;

    [Header("Dropdown Animation")]
    public float dropDuration = 0.4f;
    public float hiddenY = 600f;
    public float shownY = -50f;

    [Header("References")]
    public TimeLogic timer;

    [Header("Scene Names")]
    public string carSelectSceneName;
    public string trackSelectSceneName;

    private void Awake()
    {
        Debug.Log("FinishMenu sees timer: " + timer);
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Debug.Log("FinishMenu sees timer: " + timer);
        instance = this;

        finishOverlay.SetActive(false);
        finishPanel.gameObject.SetActive(false);
    }

    public void ShowFinishScreen(float timeToBeat)
    {
        Time.timeScale = 0f;
        
        // Display times
        yourTimeText.text = "Your Time: " + timer.TimeFormat();
        timeToBeatText.text = "Time To Beat: " + FormatTime(timeToBeat);

        //check Win/Lose 
        if (timer.GetElapsedTime() <= timeToBeat)
            resultText.text = "You did it!";
        else
            resultText.text = "Try again!";

        // Show UI
        finishOverlay.SetActive(true);
        finishPanel.gameObject.SetActive(true);

        StartCoroutine(DropDown());
    }

    IEnumerator DropDown()
    {
        Vector2 start = new Vector2(0f, hiddenY);
        Vector2 end = new Vector2(0f, shownY);

        finishPanel.anchoredPosition = start;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / dropDuration;
            finishPanel.anchoredPosition = Vector2.Lerp(start, end, t);
            yield return null;
        }

        finishPanel.anchoredPosition = end;
    }

    //display time-to-beat
    string FormatTime(float time)
    {
        int min = (int)(time / 60f);
        int sec = (int)(time % 60f);
        int ms = (int)((time * 1000f) % 1000);
        return $"{min:00}:{sec:00}:{ms:000}";
    }

    // ---------- buttons ----------
    public void RetryTrack()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToCarSelect()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(carSelectSceneName);
    }

    public void GoToTrackSelect()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(trackSelectSceneName);
    }
}
