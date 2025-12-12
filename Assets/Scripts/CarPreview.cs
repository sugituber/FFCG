using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CarPreview : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string carName;
    public GameObject textObject;
    public TMPro.TextMeshProUGUI textbox;
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("hover here here");
        textbox.text = carName;
        textObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
       textObject.SetActive(false);
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
