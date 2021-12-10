using System;
using System.Collections;
using System.Collections.Generic;
using tetrisVersion2;
using UnityEngine;

namespace tetrisVersion2
{
    public class Tetromino : MonoBehaviour
    {
        private static Spawner spawner;
        public Color32 TetriminoColor;
        public List<Vector2Int> TilePositions;

        
        private GameObject spawnedTile;

        private float lastFall = 0;
        private TetrisGrid tetrisGrid; 

        void Awake()
        {
            spawner = Spawner.Instance;
            tetrisGrid = TetrisGrid.Instance;
        }

        bool isValidGridPos()
        {
            foreach (Transform child in transform)
            {
                Vector2 v = TetrisGrid.roundVec2(child.position);

                // Not inside Border?
                if (!TetrisGrid.insideBorder(v))
                    return false;

                // Block in grid cell (and not part of same group)?
                if (TetrisGrid.grid[(int) v.x, (int) v.y] != null &&
                    TetrisGrid.grid[(int) v.x, (int) v.y].parent != transform)
                    return false;
            }

            return true;
        }

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
                TetrisGrid.grid[(int) v.x, (int) v.y] = child;
            }
        }

        void Start()
        {
            foreach (var tile in TilePositions)
            {
                TileFactory.TileSpawn(this.gameObject, tile, TetriminoColor);
            }
            
            if (!isValidGridPos())
            {
                tetrisGrid.GameOver();
            }

        }

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
                        if (isValidGridPos()) spawner.spawnNext();

                        //Disable script
                        for (int i = 0; i <= this.gameObject.transform.childCount; i++)
                        {
                            gameObject.transform.DetachChildren();
                            if (gameObject.transform.childCount == 0)
                            {
                                Destroy(this.gameObject);
                            }
                        }
                        
                    }

                    lastFall = Time.time;
                }


            }


        }

    };
