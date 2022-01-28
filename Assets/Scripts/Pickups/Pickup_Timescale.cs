using UnityEngine;

public class Pickup_Timescale : Pickup
{
    [SerializeField] float timescale = 1.0f;
    [SerializeField] float duration = 10.0f;

    protected override void Collect(Collider other)
    {
        TimeManager.AddTimescaleSource(this, timescale);
        Invoke("Done", duration);
        gameObject.SetActive(false);
    }

    void Done()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        CancelInvoke();
        TimeManager.RemoveTimescaleSource(this);
    }
}
