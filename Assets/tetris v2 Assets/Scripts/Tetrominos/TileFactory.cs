using System.Collections;
using System.Collections.Generic;
using tetrisVersion2;
using UnityEngine;

//Using Factory Method
public static class TileFactory
{
    private static ObjectPooler objectPooler = ObjectPooler.ObjPoolerInstance;

    public static GameObject TileSpawn(this GameObject tetrominoGroup, Vector2 tilePos, Color32 tetrominoColor)
    {
        objectPooler.SpawnFromPool("BaseTile");
        GameObject go = objectPooler.ObjectToSpawn;
        go.transform.parent = tetrominoGroup.transform;
        go.GetComponent<SpriteRenderer>().color = tetrominoColor;
        go.transform.localPosition = tilePos;
        return go; 
    }
}
