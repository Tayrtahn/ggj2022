using UnityEngine;

public class Pickup_Timescale : Pickup
{
    [SerializeField] float timescale = 1.0f;
    [SerializeField] float duration = 10.0f;

    protected override void Collect(Collider other)
    {
        if (timescale > 1)
        {
            SFXManager.PlaySound(SoundType.Pickup_SpeedUp);
            ParticleManager.Emit(ParticleType.SpeedUp, transform.position);
        }
        else
        {
            SFXManager.PlaySound(SoundType.Pickup_SlowDown);
            ParticleManager.Emit(ParticleType.SpeedDown, transform.position);
        }
        ParticleManager.Emit(ParticleType.Starburst, transform.position);

        //TimeManager.AddTimescaleSource(this, timescale);

        other.gameObject.GetComponent<PickupEffectTarget>().AddEffect(GetEffect(other.gameObject));
        Invoke("Done", duration);
        gameObject.SetActive(false);
    }

    void Done()
    {
        Destroy(gameObject);
    }

    protected override void Goodbye()
    {
        CancelInvoke();
        //TimeManager.RemoveTimescaleSource(this);
    }

    protected override PickupEffect GetEffect(GameObject target)
    {
        return new PickupEffect_Speed(target, timescale, duration);
    }
}
