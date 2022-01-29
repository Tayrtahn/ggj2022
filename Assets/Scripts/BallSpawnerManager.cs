using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawnerManager : MonoBehaviour
{
    [SerializeField]
    private List<BallSpawner> _ballSpawners;

    [SerializeField]
    private GameObject _ballPrefab;

    public Ball SpawnFromRandomSpawner()
    {
        int index = Random.Range(0, _ballSpawners.Count-1);
        return _ballSpawners[index].Spawn(_ballPrefab);
    }
}
