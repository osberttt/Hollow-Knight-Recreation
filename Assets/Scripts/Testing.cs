using UnityEngine;

public class Testing : MonoBehaviour
{
    private float target = 5f;

    private float starting = 0f;

    private float accelerationTime = 0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        starting = Mathf.MoveTowards(starting, target, (float)(Time.deltaTime * target /accelerationTime));
        Debug.Log($"time {Time.time}");
        Debug.Log($"reached {starting}");
    }
}
