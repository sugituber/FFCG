using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CarPreview : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string carName;
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("hover here here");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
       
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
