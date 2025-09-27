using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralCaveGeneration : MonoBehaviour
{
    [Header("Ore Settings")]
    [SerializeField] private List<OreType> oreTypes;
    
    [Header("Cave Settings")]
    [SerializeField] private int cellularAutomataSteps;
    [Range(0,100)] [SerializeField] private int wallFillPercent;
    [SerializeField] private GameObject caveWallPrefab;

    [Header("Map Settings")]
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private int outskirtsWidth;
    [SerializeField] private GameObject borderPrefab;

#region enum

    //empty or fill (random noise/cellular automata)
    private enum squareType
    {
        EMPTY,
        FILL
    }
    private squareType[,] grid;

    //map tiles
    private enum tileType
    {
        AIR,
        WALL,
        ORE,
    }
    private tileType[,] map;
#endregion


    void Start()
    {
        grid = new squareType[width,height];
        map = new tileType[width,height];

        GenerateCaves();
        GenerateOres();
        DrawMap();
        DrawOutskirts();
        DrawBorder();
    }
    
    //generate the cave systems
    void GenerateCaves()
    {
        RandomNoiseGrid(wallFillPercent); //generate random noise grid
        for (int i = 0; i < cellularAutomataSteps; i ++) //repeat cellular automata for desired number of steps.
        { 
		    CellularAutomata(outOfBoundsRule: true);
        }

        //every filled tile on the grid becomes a wall
        for (int x = 0; x < width; x ++) 
        {
			for (int y = 0; y < height; y ++) 
            {
                if (grid[x,y] == squareType.FILL)
                {
                    map[x,y] = tileType.WALL;
                }
                else
                {
                    map[x,y] = tileType.AIR;
                }
            }
        }
    }
    
    //populate the walls with veins of ores
    void GenerateOres()
    {
        foreach (OreType ore in oreTypes)
        {
            RandomNoiseGrid(ore.fillPercent); //generate random noise grid with ore fill percentage
            for (int i = 0; i < ore.caSteps; i ++) //repeat cellular automata for desired number of steps
            {
                CellularAutomata(outOfBoundsRule: false);
            }

            
            for (int x = 0; x < width; x ++) 
            {
                for (int y = 0; y < height; y ++) 
                {
                    if (grid[x,y] == squareType.FILL && map[x,y] == tileType.WALL) //if the tile is filled on the grid AND it is a wall on the map
                    {
                        map[x,y] = tileType.ORE; //set the tile to ORE

                        InstantiateTiles(x, y, ore.prefab); //instantiate ore
                    }
                }
            }
        }
    } 

#region Random Noise Grid
    void RandomNoiseGrid(int fillPercent) //Generates random noise grid
    {
        for (int x = 0; x < width; x ++) 
        {
            for (int y = 0; y < height; y ++) 
            {    
                //generate a random number between 0 and 100, if it's less than the fill percentage fill it in.
                if (Random.Range(0,100) < fillPercent) 
                { 
                    grid[x,y] = squareType.FILL;
                }
                //if it's more than the fill percentage it is empty
                else 
                {
                    grid[x,y] = squareType.EMPTY;                
                }
            }
        }
    }
#endregion

#region Cellular Automata

    void CellularAutomata(bool outOfBoundsRule)
    {
        for (int x = 0; x < width; x ++) 
        {
			for (int y = 0; y < height; y ++) 
            {
                if (IsMapCentre(x, y)) //leave the spawn radius empty when generating the random noise grid
                {
                    grid[x,y] = squareType.EMPTY;
                }
                else
                {
				    int neighbourWallTiles = GetNeighbouringWallCount(x,y, outOfBoundsRule); //get the number of neighbouring wall tiles

				    if (neighbourWallTiles > 4) //if there are more than 4 neighbouring wall tiles, set the tile to a wall
				    {
                        grid[x,y] = squareType.FILL;
                    }
				    else if (neighbourWallTiles < 4) //otherwise the tile is empty
				    {	
                        grid[x,y] = squareType.EMPTY;
                    }
			    }
            }
		}
    }

    int GetNeighbouringWallCount (int gridX, int gridY, bool outOfBoundsRule) //get the number of neighbouring wall tiles
    {
        int wallCount = 0;

        for (int neighbourX = gridX - 1; neighbourX <= gridX +1; neighbourX ++) 
        { 
            for (int neighbourY = gridY - 1; neighbourY <= gridY +1; neighbourY ++) 
            {
                if (IsInsideMap(neighbourX, neighbourY)) { //if the tile is inside the map
                    if (grid[neighbourX, neighbourY] == squareType.FILL) //if it is a wall
                    { 
                        wallCount ++; //add to the wall count
                    }
                }
                else if (outOfBoundsRule)//if the tile is not inside the map (and the out of bounds rule is true)
                { 
                    wallCount ++; //add to the wall count
                }
                else //if the tile is not inside the map (and the out of bounds rule is false)
                {
                    wallCount --; //remove from the wall count
                }
            }
        }

        return wallCount;
    } 

    bool IsMapCentre(int x, int y) 
    {
        return x >= (width/2) - 1 && x <= (width/2) + 1 && y >= (height/2) - 2 && y <= (height/2); //middle 3x3 tiles of the map
    }

    bool IsInsideMap(int x, int y)  //check if the neighbouring tile is inside the map
    { 
        return x >= 0 && x < width && y >= 0 && y < height; //true if x/y is greater than or equal to 0, and is less than the width/height
    }

#endregion

#region instantiating tiles/map

    void InstantiateTiles(int x, int y, GameObject prefab)
    {
        Vector2 position = new Vector2(x - (width/2), y - (height/2)); //centre the map
        Instantiate(prefab, position, Quaternion.identity, transform); //instantiate the prefab
    }

    void DrawMap()
    { 
        for (int x = 0; x < width; x ++) 
        {
            for (int y = 0; y < height; y ++) 
            {
                if (map[x,y] == tileType.WALL)
                {
                    InstantiateTiles(x, y, caveWallPrefab); //instantiate walls
                }
            }
        }

    }




    void DrawOutskirts() //create a buffer between the cave systems and edge of the map
    {
        for (int i = 0; i < outskirtsWidth; i++)
        {
            for (int x = 0-(i+1); x < width+(i+1); x++) //x axis
            {
                InstantiateTiles(x, (-1-i), caveWallPrefab);
                InstantiateTiles(x, (height+i), caveWallPrefab);
            }
            for (int y = (0-i); y < (height+i); y ++) //y axis
            {
            InstantiateTiles((-1-i), y, caveWallPrefab);
            InstantiateTiles((width+i), y, caveWallPrefab);
            } 
        }
    }

    void DrawBorder() //create a border around the map
    {
        for (int x = (0 - outskirtsWidth); x < (width + outskirtsWidth); x++) //x axis
        {
            InstantiateTiles(x, (-1 - outskirtsWidth), borderPrefab);
            InstantiateTiles(x, (height + outskirtsWidth), borderPrefab);
        }

        for (int y = (0 - outskirtsWidth); y < (height + outskirtsWidth); y ++) //y axis
        {
            InstantiateTiles((-1 - outskirtsWidth), y, borderPrefab);
            InstantiateTiles((width + outskirtsWidth), y, borderPrefab);
        } 
    }  

#endregion

}
