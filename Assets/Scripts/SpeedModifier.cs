using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedModifier : MonoBehaviour
{
    private Dictionary<object, float> speedMultiplierLookup = new Dictionary<object, float>();

    public void AddSpeedModifier(object source, float multiplier)
    {
        if (speedMultiplierLookup.ContainsKey(source))
            speedMultiplierLookup[source] = multiplier;
        else
            speedMultiplierLookup.Add(source, multiplier);
    }

    public void RemoveSpeedModifier(object source)
    {
        speedMultiplierLookup.Remove(source);
    }

    public float GetSpeedMultiplier()
    {
        float result = 1.0f;
        foreach (KeyValuePair<object, float> kvp in speedMultiplierLookup)
            result *= kvp.Value;
        return result;
    }
}
