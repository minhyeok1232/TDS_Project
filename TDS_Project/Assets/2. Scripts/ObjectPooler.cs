using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler instance;
    
    [SerializeField] private GameObject monsterPrefab;
    [SerializeField] private int        monsterPoolSize;
    
    private Queue<GameObject> pool = new Queue<GameObject>();


    
    void Start()
    {
        InitMonsterPool();
    }

    void InitMonsterPool()
    {
        for (int i = 0; i < monsterPoolSize; i++)
        {
            GameObject obj = Instantiate(monsterPrefab);
            obj.SetActive(false);
            obj.tag = Helpers.Monster.Monster_Tag;
            pool.Enqueue(obj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
