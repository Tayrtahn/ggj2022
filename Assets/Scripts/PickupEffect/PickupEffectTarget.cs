using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupEffectTarget : MonoBehaviour
{
    List<PickupEffect> effects = new List<PickupEffect>();

    void Update()
    {
        for (int i = effects.Count - 1; i >= 0; i--)
        {
            if (effects[i].Update(Time.deltaTime))
                effects.RemoveAt(i);
        }
    }

    public void AddEffect(PickupEffect effect)
    {
        effects.Add(effect);
    }
}
