using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TrackInfo : MonoBehaviour
{
    // 1. Keep the Asset for the Editor so you can drag-and-drop
#if UNITY_EDITOR
    public SceneAsset sceneFile;
#endif

    // 2. The string must exist on BOTH platforms. 
    // Do not wrap this in #if statements!
    // We hide it in inspector because the script handles it automatically.
    [HideInInspector] 
    public string sceneName;

    public Sprite previewImage;

    // 3. The Property just returns the string variable now
    public string SceneName
    {
        get { return sceneName; }
    }

    // 4. OnValidate runs automatically in the Editor whenever you change a value.
    // This syncs the SceneAsset name to the string variable.
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (sceneFile != null)
        {
            sceneName = sceneFile.name;
        }
    }
#endif
}