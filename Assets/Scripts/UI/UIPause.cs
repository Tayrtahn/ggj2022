using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPause : MonoBehaviour
{
    void OnEnable()
    {
        TimeManager.AddPauseSource(this);        
    }

    void OnDisable()
    {
        TimeManager.RemovePauseSource(this);
    }
}
