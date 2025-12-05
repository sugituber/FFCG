using System.Collections.Generic;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
    public Transform TracksFolder;
    public List<Transform> trackList = new List<Transform>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (Transform child in TracksFolder)
        {
            trackList.Add(child);
        }

        Debug.Log("Tracks found: " + trackList.Count);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
