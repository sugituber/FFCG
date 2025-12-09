using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TrackPreviewHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image previewImg;
    public Sprite previewSprite;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (previewImg != null && previewSprite != null)
            previewImg.sprite = previewSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (previewImg != null)
            previewImg.sprite = null;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
