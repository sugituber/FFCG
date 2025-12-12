using UnityEngine;

public class GetCar : MonoBehaviour
{
    [SerializeField] private Transform startLine; // Assign in Inspector
    [SerializeField] private Transform carFolder; // Folder with all car prefabs

    public string selectedCarName;
    void Start()
    {
        

        if (string.IsNullOrEmpty(selectedCarName))
        {
            Debug.LogError("No car selected!");
            return;
        }

        Transform selectedCar = carFolder.Find(selectedCarName);

        if (selectedCar != null)
        {
            Instantiate(selectedCar.gameObject, startLine.position, startLine.rotation);
            Debug.Log("Spawned car: " + selectedCarName);
        }
        else
        {
            Debug.LogError("Car prefab not found: " + selectedCarName);
        }
    }
}
