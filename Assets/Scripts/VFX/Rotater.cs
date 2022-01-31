using UnityEngine;

public class Rotater : MonoBehaviour
{
    [SerializeField] bool shouldPingPong = true;
    [SerializeField] Vector3 rotateVector;

    void Update()
    {
        if (shouldPingPong)
            transform.Rotate(rotateVector * Time.deltaTime * (1.0f - Mathf.PingPong(Time.time, 2.0f) ));
        else
            transform.Rotate(rotateVector * Time.deltaTime);
    }
}
