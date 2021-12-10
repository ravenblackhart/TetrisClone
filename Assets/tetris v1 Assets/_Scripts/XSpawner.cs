using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XSpawner : MonoBehaviour {

    //Groups 
    public GameObject[] groups;

    // Use this for initialization
    void Start()
    {

        //Spawn Initial Group
        spawnNext();
    }

  

    public void spawnNext()
    {

        //Random Index 
        int i = Random.Range(0, groups.Length);

        //Spawn Group at Current Position 
        Instantiate(groups[i], transform.position, Quaternion.identity);
    }

}
