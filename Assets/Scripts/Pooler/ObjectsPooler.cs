
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ObjectsPooler : MonoBehaviour
{
    private Dictionary<string, Queue<GameObject>> _poolDictionary;

    private void Start()
    {
        _poolDictionary = new Dictionary<string, Queue<GameObject>>();
    }

    public void AddObjectToPool(string tag, GameObject objectToAdd)
    {
        if (!_poolDictionary.ContainsKey(tag))
        {
            _poolDictionary.Add(tag, new Queue<GameObject>());
        }
        objectToAdd.SetActive(false);
        _poolDictionary[tag].Enqueue(objectToAdd);
    }

    public GameObject SpawnFromPool(string tag, Vector3 position)
    {
        GameObject objToSpawn = _poolDictionary[tag].Dequeue();
        objToSpawn.SetActive(true);
        objToSpawn.transform.position = position;
        IPooledObject pooledObject = objToSpawn.GetComponent<IPooledObject>();

        if (pooledObject != null)
        {
            pooledObject.OnObjectSpawn();
        }

        return objToSpawn;
    }
    
    public void ReturnObjectToPool(string tag, GameObject obj)
    {
        obj.SetActive(false);
        _poolDictionary[tag].Enqueue(obj);
    }
}