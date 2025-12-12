using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TrackInfo : MonoBehaviour
{

#if UNITY_EDITOR
    public SceneAsset sceneFile;
#endif
    public Sprite previewImage;

    public string SceneName
    {
        get
        {
#if UNITY_EDITOR
            return sceneFile != null ? sceneFile.name : "";
#else      
            return fallbackSceneName;
#endif
        }
    }

#if !UNITY_EDITOR
    [HideInInspector]
    public string fallbackSceneName;
#endif


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
