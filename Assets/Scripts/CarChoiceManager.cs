using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CarChoiceManager : MonoBehaviour
{
    public Transform CarFolder;
    public List<Transform> carlist = new List<Transform>();

    public GameObject carbuttonprefab;       // <-- Your UI button prefab
    public Transform carbuttonParent;        // <-- A UI Panel or Vertical Layout Group

    public Button nextbutton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (Transform child in CarFolder)
        {
            carlist.Add(child);
        }
        
        Debug.Log("Cars presets found: " + carlist.Count);

        Carbuttons();
        nextbutton.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (nextbutton.interactable == true)
        {
            OnNext();
        }
    }

    void Carbuttons()
    {
        foreach (Transform car in carlist)
        {
            Transform carRef = car;  // <-- local copy
            GameObject button = Instantiate(carbuttonprefab, carbuttonParent);
            button.GetComponentInChildren<TextMeshProUGUI>().text = carRef.name;
            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                Debug.Log("Clicked on car: " + carRef.name);
                GameFlow.Instance.selectedCar = carRef.name;
                nextbutton.interactable = true;
            });

            CarPreview carhover = button.AddComponent<CarPreview>();
        }

    }

    void OnNext()
    {
        nextbutton.GetComponent<Button>().onClick.AddListener(() =>
        {
            Debug.Log("Next thingy:D");
            Debug.Log("Loading track: " + GameFlow.Instance.selectedTrack);
            // SceneManager.LoadScene("");
            SceneManager.LoadScene(GameFlow.Instance.selectedTrack);
        });
    }
}
