using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Using Object Pool Pattern

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string TetrominoName;
        public GameObject TetriminoPrefab;
        public int PoolSize; 
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>(); 
            for (int i = 0; i < pool.PoolSize; i++)
            {
                GameObject tetromino = Instantiate(pool.TetriminoPrefab);
                tetromino.SetActive(false);
                objectPool.Enqueue(tetromino);
            }

            poolDictionary.Add(pool.TetrominoName, objectPool);
        }
    }

    public GameObject SpawnFromPool (string TetriminoType, Vector2 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(TetriminoType)) return null;

        GameObject objectToSpawn = poolDictionary[TetriminoType].Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        poolDictionary[TetriminoType].Enqueue(objectToSpawn);

        return objectToSpawn;
    } 
}
