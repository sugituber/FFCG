using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TrackManager : MonoBehaviour
{
    public Transform TracksFolder;
    public TMPro.TextMeshProUGUI track;
    public List<Transform> trackList = new List<Transform>();

    // Assign in Inspector
    public GameObject buttonPrefab;       // <-- Your UI button prefab
    public Transform buttonParent;        // <-- A UI Panel or Vertical Layout Group

    public Image previewUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (Transform child in TracksFolder)
        {
            trackList.Add(child);
        }
        
        Debug.Log("Tracks found: " + trackList.Count);
        // DisplayTracks();
        TrackButton();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TrackButton()
    {
        foreach (Transform t in trackList)
        {
            Transform trackRef = t; // <-- local copy
            Debug.Log("button here");
            GameObject button = Instantiate(buttonPrefab, buttonParent);
            button.GetComponentInChildren<TextMeshProUGUI>().text = trackRef.name;
            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                // SceneManager.LoadScene(trackRef.name);
                TrackInfo info = trackRef.GetComponent<TrackInfo>();
                if (info != null)
                {
                    GameFlow.Instance.selectedTrack = info.SceneName;
                    Debug.Log("Clicked track: " + trackRef.name);
                    Debug.Log("SceneName: " + info.SceneName);
                }
                SceneManager.LoadScene("CarChoice");
            });

            TrackPreviewHover hover = button.AddComponent<TrackPreviewHover>();
            hover.previewImg = previewUI;
            TrackInfo info = trackRef.GetComponent<TrackInfo>();
            if (info != null)
                hover.previewSprite = info.previewImage;
        }
    }
}
