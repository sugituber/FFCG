using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrackManager : MonoBehaviour
{
    public Transform TracksFolder;
    public TMPro.TextMeshProUGUI track;
    public List<Transform> trackList = new List<Transform>();

    // Assign in Inspector
    public GameObject buttonPrefab;       // <-- Your UI button prefab
    public Transform buttonParent;        // <-- A UI Panel or Vertical Layout Group

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

    void DisplayTracks()
    {
        string textOutput = "";
        foreach (Transform t in trackList)
        {
            textOutput += t.name + "\n";
            Debug.Log("In here");
        }
        track.text = textOutput;
        Debug.Log("Here");
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
                Debug.Log("Clicked track: " + trackRef.name);
            });
        }
    }
}
