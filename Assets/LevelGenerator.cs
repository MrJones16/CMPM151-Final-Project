using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    // Platform prefabs
    public GameObject[] platformColors;
    // Number of platform colors
    private int numColors;

    // Object to show generated path
    public GameObject pathObject;

    // Length and width of pit in terms of possible positions
    // Actual positions can be scaled and spaced out differently
    public int length = 8;
    public int width = 8;

    // Max jumpable distance between platforms
    public int maxInterval = 3;

    // Array representing the positions of all platforms in the level
    private int[,] level;

    //float value to put distance between blocks
    public float jumpDistance = 1f;

    //float to pull back block if moved left/right to make jumps possible
    public float sideJumpScale = 1f;

    private void Start()
    {
        // Get the number of colors from the array of platform prefabs
        numColors = platformColors.Length;

        GenerateLevel();
    }

    private void GenerateLevel()
    {
        // Keep track of at least one possible path
        int[] path = GeneratePath();

        // Generate all platforms using path as needed
        level = GeneratePlatforms(path);
        //variable for previous location
        Vector3 position = transform.position;
        // Instantiate platforms
        for (int j = 0; j < numColors; ++j)
        {
            position = transform.position;
            for (int i = 0; i < length; ++i)
            {
                // Check for blocking
                for (int k = 0; k < j; ++k)
                {
                    if (level[i,j] == level[i,k])
                    {
                        // Raise height if this platform blocks another
                        position.y += 1;
                    }
                }
                //set position left or right and forward
                Debug.Log(level[i,j]);
                float nextDistance = jumpDistance - Mathf.Abs(sideJumpScale * level[i,j]);
                position += new Vector3(level[i,j],0,nextDistance);
                //instantiate and get new position
                position = Instantiate(platformColors[j], position, Quaternion.identity).transform.position;
            }

            // Show path underneath for debugging purposes
            //Vector3 pathPos = new Vector3(path[i], -1, i);
            //pathPos+= this.transform.position;
            //Instantiate(pathObject, pathPos, transform.rotation);
        }
    }

    private int[] GeneratePath()
    {
        int[] path = new int[length];

        // First platform can have any x position
        path[0] = Random.Range(0, width);
        //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^Instead, set the start of the path to the center:
        //because with how the code is set up, if it picks something that isn't the center, it would
        //go out of the overall bounds. Also, if we do infinite sprawling of parkour, it would work
        //better to start at specific location so the parkour can continue
        //path[0] = 0;
        //                             
        // Generate each next platform by ASSIGNING A LEFT/RIGHT VALUE, within maxInterval
        for (int i = 1; i < length; ++i)
        {
            int newX = Random.Range(-maxInterval, maxInterval + 1);
            path[i] = newX;
        }

        return path;
    }

    private int[,] GeneratePlatforms(int[] path)
    {
        // All platform positions in one (two-dimensional) array
        int[,] newLevel = new int[length,numColors];

        for (int i = 0; i < length; ++i)
        {
            // Pick a random color to use the path, the rest will be random
            int pathColor = Random.Range(0, numColors);
            Debug.Log(pathColor);
            Debug.Log(path[i]);
            newLevel[i,pathColor] = path[i];

            // Keep track of positions that are already blocked once
            bool[] isBlocked = new bool[numColors];
            for (int j = 0; j < numColors; ++j)
            {
                // Skip if this is the path color
                if (j == pathColor)
                {
                    continue;
                }

                // Loop until we get an appropriate new x position
                bool tryAgain = true;
                while (tryAgain)
                {
                    // Random x position
                    int newX = Random.Range(-maxInterval, maxInterval + 1);

                    // Assume this position won't block anything
                    tryAgain = false;

                    // But then check if it does block another color
                    for (int k = 0; k < j; ++k)
                    {
                        if (newX == newLevel[i,k])
                        {
                            // If already blocked, break for loop and try again
                            if (isBlocked[k])
                            {
                                tryAgain = true;
                            }
                            // Otherwise set isBlocked to true for both and proceed
                            else
                            {
                                isBlocked[j] = true;
                                isBlocked[k] = true;
                            }
                            // Either way we don't have to check remaining colors
                            break;
                        }
                    }
                    // Set position if there was no blocking
                    if (!tryAgain)
                    {
                        newLevel[i,j] = newX;
                    }
                }
            }
        }

        return newLevel;
    }
}
