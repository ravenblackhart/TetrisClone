using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

//Using Object Pool Pattern

namespace tetrisVersion2
{
    public class ObjectPooler : SingletonBoilerplate<ObjectPooler>
    {
        [System.Serializable]
        public class Pool
        {
            public string ObjectType;
            public GameObject ObjectPrefab;
            public int PoolSize; 
        }
        
        public override void Awake()
        {
            base.Awake();
            InstantiatePool();
        }
        
    
        public List<Pool> pools;
        public Dictionary<string, Queue<GameObject>> poolDictionary;
        [HideInInspector] public GameObject ObjectToSpawn;
        [HideInInspector] public GameObject ObjectToReturn; 

        private void InstantiatePool()
        {
            poolDictionary = new Dictionary<string, Queue<GameObject>>();
            
            foreach (Pool pool in pools)
            {
                Queue<GameObject> objectPool = new Queue<GameObject>(); 
                for (int i = 0; i < pool.PoolSize; i++)
                {
                    GameObject poolObject = Instantiate(pool.ObjectPrefab);
                    DontDestroyOnLoad(poolObject);
                    poolObject.SetActive(false);
                    objectPool.Enqueue(poolObject);
                }
    
                poolDictionary.Add(pool.ObjectType, objectPool);
            }
        }
        
        public GameObject LoadFromPool (string objectType)
        {
            if (!poolDictionary.ContainsKey(objectType))
            {
                Debug.LogWarning($"{objectType} does not exist");
                return null;
            }

            ObjectToSpawn = poolDictionary[objectType].Dequeue();
            ObjectToSpawn.SetActive(true);

            return ObjectToSpawn;

        }

        public GameObject ReturnToPool(GameObject returnedObject)
        {
            ObjectToReturn = returnedObject; 
            
            ObjectToReturn.SetActive(false);
            string objectType = ObjectToReturn.tag;
            poolDictionary[objectType].Enqueue(returnedObject);

            return ObjectToReturn; 
        }
    }

}
