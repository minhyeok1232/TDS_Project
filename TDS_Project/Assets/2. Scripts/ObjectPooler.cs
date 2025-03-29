using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler instance;

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int poolSize;
    }

    [Header("오브젝트 풀 리스트")]
    public List<Pool> pools; 
    
    [Header("부모")]
    public Transform parent;         
    
    // O(1)
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        InitMonsterPool();
    }

    void InitMonsterPool()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        
        foreach (Pool pool in pools)
        {
            Queue<GameObject> poolingObjectQueue = new Queue<GameObject>();

            for (int i = 0; i < pool.poolSize; i++)
            {
                GameObject obj = CreateNewObject(pool.prefab);
                poolingObjectQueue.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, poolingObjectQueue);
        }
    }

    private GameObject CreateNewObject(GameObject prefab)
    {
        var newObj = Instantiate(prefab, Vector3.zero, Quaternion.identity, parent);
        newObj.SetActive(false);
        return newObj;
    }

    public GameObject GetObject(string tag)
    {
        if (!poolDictionary.ContainsKey(tag)) return null;

        Queue<GameObject> objectPool = poolDictionary[tag];
        
        if (objectPool.Count > 0)
        {
            GameObject obj = objectPool.Dequeue();
            obj.transform.SetParent(null);
            obj.SetActive(true);
            return obj;
        }
        else
        {
            foreach (Pool pool in pools)
            {
                GameObject newObj = instance.CreateNewObject(pool.prefab);
                newObj.transform.SetParent(null);
                newObj.SetActive(true);
                return newObj;
            }
        }
        return null;
    }
    
    public void ReturnObject(string tag, GameObject obj)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Destroy(obj);
            return;
        }
        
        obj.SetActive(false);
        obj.transform.SetParent(parent);
        poolDictionary[tag].Enqueue(obj);
    }
}
