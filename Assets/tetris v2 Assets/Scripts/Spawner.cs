using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


namespace tetrisVersion2
{
    public class Spawner : MonoBehaviour
    {

        private ObjectPooler objectPooler;
        [SerializeField] private List<Tetromino> TetrominoTypes;

        void Start()
        {
            objectPooler = ObjectPooler.ObjPoolerInstance; 
            spawnNext();
        }
        
        public void spawnNext()
        {
            int i = Random.Range(0, TetrominoTypes.Count);
            Instantiate(TetrominoTypes[i], transform.position, Quaternion.identity);

        }
    }
}

