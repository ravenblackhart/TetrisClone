using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class Group : MonoBehaviour
{

    public UIManager hiScore;
    
    // Time since last gravity tick
    float lastFall = 0;



    //Verify Group position is Valid
    bool isValidGridPos()
    {
        foreach (Transform child in transform)
        {
            Vector2 v = TetrisGrid.roundVec2(child.position);

            // Not inside Border?
            if (!TetrisGrid.insideBorder(v))
                return false;

            // Block in grid cell (and not part of same group)?
            if (TetrisGrid.grid[(int)v.x, (int)v.y] != null &&
                TetrisGrid.grid[(int)v.x, (int)v.y].parent != transform)
                return false;
        }
        return true;
    }

    //Allow pass through intersection in parent group
    void updateGrid()
    {
        // Remove old children from grid
        for (int y = 0; y < TetrisGrid.h; ++y)
            for (int x = 0; x < TetrisGrid.w; ++x)
                if (TetrisGrid.grid[x, y] != null)
                    if (TetrisGrid.grid[x, y].parent == transform)
                        TetrisGrid.grid[x, y] = null;

        // Add new children to grid
        foreach (Transform child in transform)
        {
            Vector2 v = TetrisGrid.roundVec2(child.position);
            TetrisGrid.grid[(int)v.x, (int)v.y] = child;
        }
    }

    // Use this for initialization
    void Start()
    {
        hiScore = FindObjectOfType<UIManager>();
    
        // Default position not valid? Then it's game over
        if (!isValidGridPos())
        {
            Debug.Log("GAME OVER");
            
        }


    }

    // Update is called once per frame
    void Update()
    {

        // Move Left
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // Modify position
            transform.position += new Vector3(-1, 0, 0);

            // See if valid
            if (isValidGridPos())
                // Its valid. Update grid.
                updateGrid();
            else
                // Its not valid. revert.
                transform.position += new Vector3(1, 0, 0);
        }

        // Move Right
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // Modify position
            transform.position += new Vector3(1, 0, 0);

            // See if valid
            if (isValidGridPos())
                // It's valid. Update grid.
                updateGrid();
            else
                // It's not valid. revert.
                transform.position += new Vector3(-1, 0, 0);
        }

        // Rotate
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.Rotate(0, 0, -90);

            // See if valid
            if (isValidGridPos())
                // It's valid. Update grid.
                updateGrid();
            else
                // It's not valid. revert.
                transform.Rotate(0, 0, 90);
        }

        // Move Down
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            // Modify position
            transform.position += new Vector3(0, -1, 0);

            // See if valid
            if (isValidGridPos())
                // It's valid. Update grid.
                updateGrid();
            else
            {
                // It's not valid. revert.
                transform.position += new Vector3(0, 1, 0);

                // Clear filled horizontal lines
                TetrisGrid.DeleteFullRows();

            }
        }


        // Auto Fall
        else if (Time.time - lastFall >= 1)
        {
            // Modify position
            transform.position += new Vector3(0, -1, 0);

            // See if valid
            if (isValidGridPos())
            {
                // It's valid. Update grid.
                updateGrid();
            }
            else
            {
                // It's not valid. revert.
                transform.position += new Vector3(0, 1, 0);

                // Clear filled horizontal lines
                TetrisGrid.DeleteFullRows();

                // Spawn next Group
                FindObjectOfType<XSpawner>().spawnNext();
                
                // Disable script
                enabled = false;
            }

            lastFall = Time.time;
        }

        

    }
}
