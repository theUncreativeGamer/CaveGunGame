using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    public GameObject prefab;
    public int maxCount;
    public float spawnInterval;

    private HashSet<GameObject> _spawnedInstances = new();
    private float _timer = 0;

    private void FixedUpdate()
    {
        _spawnedInstances.RemoveWhere(obj => obj == null);
        if (_spawnedInstances.Count >= maxCount) return;

        _timer += Time.fixedDeltaTime;
        if (_timer >= spawnInterval)
        {
            _timer -= spawnInterval;

            GameObject newStuff = Instantiate(prefab, transform.position, transform.rotation, transform);
            _spawnedInstances.Add(newStuff);
        }
    }

}
