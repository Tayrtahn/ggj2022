using UnityEngine;
using UnityEngine.Events;

public abstract class Pickup : MonoBehaviour
{
    public UnityEvent<Pickup> Cleanup;

    private void OnTriggerEnter(Collider other)
    {
        Collect(other);
    }

    private void OnDestroy()
    {
        Cleanup.Invoke(this);
        Goodbye();
    }

    protected abstract void Goodbye();

    protected abstract void Collect(Collider other);
}
