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
            Vector2 v = Grid.roundVec2(child.position);

            // Not inside Border?
            if (!Grid.insideBorder(v))
                return false;

            // Block in grid cell (and not part of same group)?
            if (Grid.grid[(int)v.x, (int)v.y] != null &&
                Grid.grid[(int)v.x, (int)v.y].parent != transform)
                return false;
        }
        return true;
    }

    //Allow pass through intersection in parent group
    void updateGrid()
    {
        // Remove old children from grid
        for (int y = 0; y < Grid.h; ++y)
            for (int x = 0; x < Grid.w; ++x)
                if (Grid.grid[x, y] != null)
                    if (Grid.grid[x, y].parent == transform)
                        Grid.grid[x, y] = null;

        // Add new children to grid
        foreach (Transform child in transform)
        {
            Vector2 v = Grid.roundVec2(child.position);
            Grid.grid[(int)v.x, (int)v.y] = child;
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
            FindObjectOfType<UIManager>().GameOver.enabled = true;

            var prevHighscore = hiScore.prevScore;
            var newHighscore = hiScore.currentScore;

            if (newHighscore > prevHighscore)
            {
                //Save High Score
                PlayerPrefs.SetFloat("High Score", newHighscore);
                Debug.Log("High Score of " + newHighscore.ToString() + " saved");
            }

            //Print High Score
            hiScore.HiScoreText.text = PlayerPrefs.GetFloat("High Score").ToString();

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
                Grid.DeleteFullRows();

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
                Grid.DeleteFullRows();

                // Spawn next Group
                FindObjectOfType<Spawner>().spawnNext();
                
                // Disable script
                enabled = false;
            }

            lastFall = Time.time;
        }

        

    }
}
