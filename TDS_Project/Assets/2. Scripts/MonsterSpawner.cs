using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public static MonsterSpawner instance;

    [Header("���� �ɼ�")]
    public int currentMonsters;
    [SerializeField] private int maxSpawnMonsters;
    [SerializeField] private const float minSpawnTimer = 0.6f;
    [SerializeField] private const float maxSpawnTimer = 1.9f;
    
    [Header("���� ��ġ ����")]
    [SerializeField] private Transform[] _spawnPos = new Transform[3];
    
    public float spawnTimer;
    private float timer = 0.0f;
    
    // O(1)
    private Dictionary<int, Transform> layerDictionary;

    // ����
    // 1. ���� ���� �� �ٷ� Spawn
    // 2. �� ���� ���� �ð��� ���� Spawn
    
    void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        
        // Dictionary �ʱ�ȭ
        layerDictionary = new Dictionary<int, Transform>();
        // Dictionary ����
        for (int i = 0; i < _spawnPos.Length; i++)
        {
            int layer = 6 + i;
            layerDictionary.Add(layer, _spawnPos[i]);
        }
    }

    // Start�� �ƴ� Update���� ȣ�� (1�� ������ �ش�)
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
        // Monster�� �����Ǵ� Layer�� (int)6, 7, 8 �̴�.
        // 1. Layer�� Random���� ����
        // 2. Layer�� Ű ���� ���� Transform(���� ��ġ)�� ����.
        int randomLayer = Random.Range(6, 9);
        return layerDictionary[randomLayer];
    }
    
    // �� ���ĺ��� (2�� ������ �ش�)
    void SpawnMonsters()
    {
        if (currentMonsters >= maxSpawnMonsters) return;
        
        Transform spawnPos = LayerAsTransform();
        
        // pool���� ������ �´�.
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