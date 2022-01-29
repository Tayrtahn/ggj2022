using UnityEngine;
using UnityEngine.Events;

public abstract class Pickup : MonoBehaviour
{
    public UnityEvent<Pickup> Cleanup;

    private void Start()
    {
        iTween.PunchScale(gameObject, Vector3.one * 1.1f, 0.5f);
    }

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
