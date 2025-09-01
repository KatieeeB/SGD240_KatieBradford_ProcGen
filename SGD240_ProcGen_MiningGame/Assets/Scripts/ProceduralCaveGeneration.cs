using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralCaveGeneration : MonoBehaviour
{
   [SerializeField] private int width;
   [SerializeField] private int height;
   [SerializeField] private int cellularAutomataSteps;
   [Range(0,100)] [SerializeField] private int fillPercent;
   [SerializeField] private GameObject filledTilePrefab;
    
    private enum squareType{
        EMPTY,
        FILL
    }
    private squareType[,] map;

    

    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        map = new squareType[width,height];
        RandomNoiseGrid(); //generate random noise grid
        for (int i = 0; i < cellularAutomataSteps; i ++) { //repeat cellular automata for desired number of steps.
			CellularAutomata();
        }
        DrawMap();
    }

    void RandomNoiseGrid() //Generates random noise grid
    {
        for (int x = 0; x < width; x ++) {
            for (int y = 0; y < height; y ++) {
                //generate a random number between 0 and 100, if it's less than the fill percentage fill it in.
                if (Random.Range(0,100) < fillPercent) 
                { 
                    map[x,y] = squareType.FILL;
                }
                //if it's more than the fill percentage it is empty
                else {
                    map[x,y] = squareType.EMPTY;
                }                 
            }
        }
    }

    
    void CellularAutomata() { //Put the map through Cellular Automata process
		for (int x = 0; x < width; x ++) {
			for (int y = 0; y < height; y ++) {
				int neighbourWallTiles = GetNeighbouringWallCount(x,y); //get the number of neighbouring wall tiles

				if (neighbourWallTiles > 4) //if there are more than 4 neighbouring wall tiles, set the tile to a wall
					map[x,y] = squareType.FILL;
				else if (neighbourWallTiles < 4) //otherwise the tile is empty
					map[x,y] = squareType.EMPTY;

			}
		}
	}


    bool IsInsideMap(int x, int y) { //check if the neighbouring tile is inside the map
        return x >= 0 && x < width && y >= 0 && y < height; //true if x/y is greater than or equal to 0, and is less than the width/height
    }


    int GetNeighbouringWallCount (int gridX, int gridY) //get the number of neighbouring wall tiles
    {
        int wallCount = 0;

        for (int neighbourX = gridX - 1; neighbourX <= gridX +1; neighbourX ++) { 
            for (int neighbourY = gridY - 1; neighbourY <= gridY +1; neighbourY ++) {
                if (IsInsideMap(neighbourX, neighbourY)) { //if the tile is inside the map
                    if (map[neighbourX, neighbourY] == squareType.FILL) { //if it is a wall
                        wallCount ++; //add to the wall count
                    }
                }
                else { //if the tile is not inside the map
                    wallCount ++; //add to the wall count
                }
            }
        }

        return wallCount;
    } 

    void DrawMap()
    {
        for (int x = 0; x < width; x ++) {
            for (int y = 0; y < height; y ++) {
                Vector2 position = new Vector2(x, y);

                if (map[x,y] == squareType.FILL) //place a wall where every filled tile is.
                {
                    Instantiate(filledTilePrefab, position, Quaternion.identity, transform);
                }
            }
        }
    }
}
