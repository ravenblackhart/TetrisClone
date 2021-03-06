using System;
using tetrisVersion2;
using UnityEngine;

// Using Singleton
public class TetrisGrid : SingletonBoilerplate<TetrisGrid>
{
    private static UIManager uiManager;
    private static Spawner spawner;
    private static ObjectPooler objectPooler;


    //Setting up Grid
    public static int w = 10;
    public static int h = 18;
    public static Transform[,] grid = new Transform[w, h];


    public override void Awake()
    {
        base.Awake();
        uiManager = FindObjectOfType<UIManager>();
        spawner = Spawner.Instance;
        objectPooler = ObjectPooler.Instance;
    }

    public static void StartGame()
    {
        spawner.spawnNext();
    }

    //Rounding Script
    public static Vector2 roundVec2(Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
    }

    //insideBorder Helper 
    public static bool insideBorder(Vector2 pos)
    {
        return ((int) pos.x >= 0 && (int) pos.x < w && (int) pos.y >= 0);
    }

    //Delete row
    public static void deleteRow(int y)
    {
        for (int x = 0; x < w; ++x)
        {
            GameObject temp = grid[x, y].gameObject;

            if (temp.transform.childCount > 0)
            {
                temp.transform.DetachChildren();
                Destroy(temp);
            }

            while (grid[x, y] != null)
            {
                GameObject returnable = grid[x, y].gameObject;
                
                objectPooler.ReturnToPool(returnable);
                grid[x, y] = null;
            }
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
                uiManager.UpdateScore();
            }
        }
    }

    //Game Over
    public void GameOver()
    {
        uiManager.GameOver();
        foreach (GameObject tetrominoGroup in GameObject.FindGameObjectsWithTag("Tetromino"))
        {
            tetrominoGroup.transform.DetachChildren();
            Destroy(tetrominoGroup.gameObject);
        }
    }

    public static void RefreshGrid()
    {
        GameObject[] tempList = GameObject.FindGameObjectsWithTag("BaseTile");
        foreach (GameObject returnable in tempList)
        {
            objectPooler.ReturnToPool(returnable);
        }
    }
}