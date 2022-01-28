using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [SerializeField] Pickup[] pickups;
    [SerializeField] int maxActivePickups = 5;
    [SerializeField] float intervals = 5.0f;

    List<Pickup> pickupInstances = new List<Pickup>();
    CircleRenderer circle;

    const float MARGIN = 0.8f;

    void Start()
    {
        circle = Object.FindObjectOfType<CircleRenderer>();
        InvokeRepeating("SpawnPickup", intervals, intervals);
    }

    void SpawnPickup()
    {
        if (pickupInstances.Count >= maxActivePickups)
            return;

        Pickup toSpawn = GetRandomPickup();
        Vector2 pos2d = Random.insideUnitCircle * (circle.Radius * MARGIN);
        Vector3 pos = new Vector3(pos2d.x, pos2d.y, 0);
        Pickup pickup = Instantiate(toSpawn, pos, toSpawn.transform.rotation);
        pickup.transform.SetParent(transform);
        pickup.Cleanup.AddListener(Cleanup);
        pickupInstances.Add(pickup);

        SFXManager.PlaySound(SoundType.PickupSpawn, pos);
    }

    Pickup GetRandomPickup()
    {
        return pickups[Random.Range(0, pickups.Length)];
    }

    void Cleanup(Pickup p)
    {
        p.Cleanup.RemoveAllListeners();
        pickupInstances.Remove(p);
    }

    private void OnDestroy()
    {
        CancelInvoke();
    }
}
