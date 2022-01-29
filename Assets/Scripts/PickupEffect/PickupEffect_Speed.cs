using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupEffect_Speed : PickupEffect
{
    public PickupEffect_Speed(GameObject _target, float _magnitude, float _duration) :base(_target, _duration)
    {
        magnitude = _magnitude;
    }

    protected override void Begin()
    {
        // increase speed
        target.GetComponent<SpeedModifier>().AddSpeedModifier(this, magnitude);
    }

    protected override void Process(float delta)
    {
       
    }

    protected override void End()
    {
        // increase speed
        target.GetComponent<SpeedModifier>().RemoveSpeedModifier(this);
    }

    float magnitude;
}
