using System;
using System.Collections;
using System.Collections.Generic;
using tetrisVersion2;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    public Color32 TetriminoColor;
    public List<Vector2Int> TilePositions; 
    
    private ObjectPooler objectPooler;
    private GameObject spawnedTile; 
    
    void Awake()
    {
        objectPooler = ObjectPooler.ObjPoolerInstance;
    }

    void Start()
    {
        foreach (var tile in TilePositions)
        {
            TileFactory.TileSpawn(this.gameObject, tile, TetriminoColor); 
        }
    }
    
    
    
}
