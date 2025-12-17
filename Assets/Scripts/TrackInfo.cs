using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TrackInfo : MonoBehaviour
{
    // Editor-only reference to SceneAsset
#if UNITY_EDITOR
    public SceneAsset sceneFile;
#endif

    // Shared field
    public Sprite previewImage;

    // Fallback name is serialized everywhere to avoid player/editor mismatch
    [HideInInspector] 
    public string fallbackSceneName;

    // Runtime property to get the scene name
    public string SceneName
    {
        get
        {
#if UNITY_EDITOR
            return sceneFile != null ? sceneFile.name : fallbackSceneName;
#else
            return fallbackSceneName;
#endif
        }
    }

    // Optional: automatically update fallbackSceneName in editor
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (sceneFile != null)
        {
            fallbackSceneName = sceneFile.name;
        }
    }
#endif
}
