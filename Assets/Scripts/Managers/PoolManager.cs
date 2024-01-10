using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoolManager : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] Transform poolParent;
    [SerializeField] int replicas;

    [SerializeField] List<GameObject> pool = new List<GameObject>();


    protected void CreatePool()
    {
        for (int i = 0; i < replicas; i++)
        {
            var poolObject = Instantiate(prefab, poolParent);
            poolObject.SetActive(false);
            pool.Add(poolObject);
        }
    }
    public GameObject GetObject(Vector3 position)
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                pool[i].SetActive(true);
                pool[i].transform.position = position;
                return pool[i];
            }
        }
        var poolObject = Instantiate(prefab, poolParent);
        pool.Add(poolObject);
        poolObject.transform.position = position;
        return poolObject;
    }
    
}
