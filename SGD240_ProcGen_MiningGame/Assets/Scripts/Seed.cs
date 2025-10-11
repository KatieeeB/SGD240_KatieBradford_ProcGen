using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{
    [SerializeField] private string customSeed = "Default";    
    [SerializeField] private int seed = 0;
    [SerializeField] private bool useCustomSeed;

    void Awake()
    {
        if (useCustomSeed) //if using a custom seed
        {
            seed = StringToInt(customSeed); //convert customSeed to an int
            Random.InitState(seed); //initialize the random number generator with the seed.
        }
        else //not using a custom seed
        {
            int seed = Random.Range(1, 1000000); //randomise seed
            Random.InitState(seed); //initialize the random number generator with the seed.
            Debug.Log(seed);
        }
    }

    int StringToInt(string input) //convert a string to an int
    {
        if (int.TryParse(input, out int convertedSeed)) //attempt to convert the string to an int (only works if the string is only numbers)
        {
            return convertedSeed;
        }

        else //if the string has other characters, instead get the hash code.
        {
            return input.GetHashCode();
        }
    }

}
