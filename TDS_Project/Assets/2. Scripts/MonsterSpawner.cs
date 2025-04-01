using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public static MonsterSpawner instance;

    [Header("스폰 옵션")]
    public int currentMonsters;
    [SerializeField] private int maxSpawnMonsters;
    [SerializeField] private const float minSpawnTimer = 0.6f;
    [SerializeField] private const float maxSpawnTimer = 1.9f;
    
    [Header("스폰 위치 지정")]
    [SerializeField] private Transform[] _spawnPos = new Transform[3];
    
    public float spawnTimer;
    private float timer = 0.0f;
    
    // O(1)
    private Dictionary<int, Transform> layerDictionary;

    // 구조
    // 1. 게임 시작 시 바로 Spawn
    // 2. 그 이후 랜덤 시간을 통한 Spawn
    
    void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        
        // Dictionary 초기화
        layerDictionary = new Dictionary<int, Transform>();
        // Dictionary 설정
        for (int i = 0; i < _spawnPos.Length; i++)
        {
            int layer = 6 + i;
            layerDictionary.Add(layer, _spawnPos[i]);
        }
    }

    // Start가 아닌 Update에서 호출 (1번 구조에 해당)
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnTimer)
        {
            timer = 0.0f;
            SpawnMonsters();
        }
    }


    Transform LayerAsTransform()
    {
        // Monster이 생성되는 Layer는 (int)6, 7, 8 이다.
        // 1. Layer을 Random으로 지정
        // 2. Layer의 키 값에 따라 Transform(스폰 위치)를 지정.
        int randomLayer = Random.Range(6, 9);
        return layerDictionary[randomLayer];
    }
    
    // 그 이후부터 (2번 구조에 해당)
    void SpawnMonsters()
    {
        if (currentMonsters >= maxSpawnMonsters) return;
        
        Transform spawnPos = LayerAsTransform();
        
        // pool에서 가지고 온다.
        GameObject monster = ObjectPooler.instance.GetObject("Monster");
        monster.transform.position = spawnPos.position;
        monster.transform.rotation = spawnPos.rotation;
        monster.layer = spawnPos.gameObject.layer;
        
        currentMonsters++;
        RandomSpawnTimer();
    }

    void RandomSpawnTimer()
    {
        spawnTimer = Random.Range(minSpawnTimer, maxSpawnTimer);
    }
}