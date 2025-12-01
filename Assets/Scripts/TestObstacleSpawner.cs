using UnityEngine;

public class TestObstacleSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject spawnable;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < 100; i++)
        {
            Instantiate<GameObject>(spawnable, new(Random.Range(-100,100), Random.Range(-100,100)), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
