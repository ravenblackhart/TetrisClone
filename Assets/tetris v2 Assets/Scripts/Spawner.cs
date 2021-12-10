using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


namespace tetrisVersion2
{
    public class Spawner : SingletonBoilerplate<Spawner>
    {
        [SerializeField] private List<Tetromino> TetrominoTypes;

        public override void Awake()
        {
            base.Awake();
            spawnNext();
        }

        public void spawnNext()
        {
            int i = Random.Range(0, TetrominoTypes.Count);
            Instantiate(TetrominoTypes[i], transform.position, Quaternion.identity);

        }
    }
}

