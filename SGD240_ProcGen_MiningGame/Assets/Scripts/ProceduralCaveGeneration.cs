using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralCaveGeneration : MonoBehaviour
{
   [SerializeField] private int width;
   [SerializeField] private int height;
   [Range(0,100)] [SerializeField] private int fillPercent;
    private enum squareType{
        EMPTY,
        FILL
    }

    [SerializeField] private GameObject filledTilePrefab;



    private squareType[,] map;

    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        map = new squareType[width,height];
        RandomNoiseGrid();
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

    void DrawMap()
    {
        for (int x = 0; x < width; x ++) {
            for (int y = 0; y < height; y ++) {
                Vector2 position = new Vector2(x, y);

                if (map[x,y] == squareType.FILL)
                {
                    Instantiate(filledTilePrefab, position, Quaternion.identity, transform);
                }
            }
        }
    }
}
