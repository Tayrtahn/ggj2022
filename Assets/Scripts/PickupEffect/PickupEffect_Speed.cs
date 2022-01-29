using UnityEngine;

public class PickupEffect_Speed : PickupEffect
{
    public PickupEffect_Speed(GameObject _target, float _magnitude, float _duration) :base(_target, _duration)
    {
        magnitude = _magnitude;
    }

    protected override void Begin()
    {
        SpeedModifier sm = target.GetComponent<SpeedModifier>();
        if (sm)
            sm.AddSpeedModifier(this, magnitude);
    }

    protected override void Process(float delta)
    {
       
    }

    protected override void End()
    {
        SpeedModifier sm = target.GetComponent<SpeedModifier>();
        if (sm)
            sm.RemoveSpeedModifier(this);
    }

    float magnitude;
}
