using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Collect(other);
    }

    protected abstract void Collect(Collider other);
}
