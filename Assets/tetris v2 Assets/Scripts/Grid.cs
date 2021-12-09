using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

// Using Singleton
public class Grid : MonoBehaviour
{
    private static Grid instance; 

    //Setting up Grid
    public static int w = 10;
    public static int h =18;
    public static Transform[,] grid = new Transform[w, h];

    //Update Scores
    public static UIManager updateScore;

    void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else 
        { 
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        } 

    }
    
    void Start()
    {
        updateScore = FindObjectOfType<UIManager>();
    }

    //Rounding Script
    public static Vector2 roundVec2(Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
    }

    //insideBorder Helper 
    public static bool insideBorder(Vector2 pos)
    {
        return ((int)pos.x >= 0 && (int)pos.x < w && (int)pos.y >= 0);
    }

    //Delete row
    public static void deleteRow(int y)
    {
        for (int x = 0; x < w; ++x)
        {
            Transform temp = grid[x, y].gameObject.transform.parent;
            grid[x, y].gameObject.transform.parent = null; 
            grid[x, y].gameObject.SetActive(false);
            grid[x, y] = null;
            
            if (temp.childCount == 0) Destroy(temp.gameObject);
            
        }
    }

    //Decrease Row
    public static void decreaseRow(int y)
    {
        for (int x = 0; x < w; ++x)
        {
            if (grid[x, y] != null)
            {
                // Move one towards bottom
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;

                // Update Block position
                grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    //Decrease RowsAbove
    public static void decreaseRowsAbove(int y)
    {
        for (int i = y; i < h; ++i)
        {
            decreaseRow(i);
        }

    }

    //Is Row Full?
    public static bool isRowFull(int y)
    {
        for (int x = 0; x < w; ++x)
            if (grid[x, y] == null)
                return false;
        return true;

    }

    //Delete Full Rows 
    public static void DeleteFullRows()
    {
        for (int y = 0; y < h; ++y)
        {
            if (isRowFull(y))
            {
                deleteRow(y);
                decreaseRowsAbove(y + 1);
                --y;
                Debug.Log("Line Cleared");
                updateScore.currentScore += 100;
            }
        }

    }






}
