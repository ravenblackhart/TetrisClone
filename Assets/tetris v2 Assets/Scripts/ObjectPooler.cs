using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

//Using Object Pool Pattern + Singleton

namespace tetrisVersion2
{
    public class ObjectPooler : MonoBehaviour
    {
        [System.Serializable]
        public class Pool
        {
            public string ObjectType;
            public GameObject ObjectPrefab;
            public int PoolSize; 
        }
        
        #region Singleton

        private static ObjectPooler instance;
        public static ObjectPooler ObjPoolerInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<ObjectPooler>();
                    if (ObjPoolerInstance == null)
                    {
                        GameObject objectPooler = new GameObject();
                        objectPooler.name = "ObjectPooler";
                        instance = objectPooler.AddComponent<ObjectPooler>(); 
                        DontDestroyOnLoad(objectPooler);
                    }
                }

                return instance;
            }
        }

        private void Awake()
        { 
            if (instance != null) Destroy(gameObject);
            else 
            { 
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            } 
            
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
    
        #endregion
    
        public List<Pool> pools;
        public Dictionary<string, Queue<GameObject>> poolDictionary;
        [HideInInspector]public GameObject ObjectToSpawn; 
        
        public GameObject SpawnFromPool (string objectType)
        {
            if (!poolDictionary.ContainsKey(objectType))
            {
                Debug.LogWarning($"{objectType} does not exist");
                return null;
            }
            
            ObjectToSpawn = poolDictionary[objectType].Dequeue();
            ObjectToSpawn.SetActive(true);

            poolDictionary[objectType].Enqueue(ObjectToSpawn);
    
            return ObjectToSpawn;

        } 
    }

}
