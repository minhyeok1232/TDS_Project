using System.Collections;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [Header("스폰 위치")] public Transform spawnPoint;
    [Header("스폰 간격")] public float spawnInterval = 1.0f;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            var monster = ObjectPooler.GetObject();
            monster.transform.position = spawnPoint.position;
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}