using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    public int levelLength = 12;
    public int numColors = 3;
    public int widthVariety = 5;
    public float jumpDistance = 6f;
    public float skipPercent = 30f;
    private int[][] level;
    public GameObject[] Platforms;
    public GameObject Checkpoint;
    public AudioController audioController;
    private void Start() {
        audioController = GameObject.Find("GenerativeAudio").GetComponent<AudioController>();
        int[] path = generateBasePath(levelLength);
        level = generateColoredPaths(path, numColors);
        for (int i = 0; i < level[0].Length; ++i){
            Debug.Log("RED: "+level[0][i]);
        }
        placePlatforms(level);
        audioController.CalledStart(level);
    }
    public void ManualStart(){
        audioController = GameObject.Find("GenerativeAudio").GetComponent<AudioController>();
        int[] path = generateBasePath(levelLength);
        level = generateColoredPaths(path, numColors);
        for (int i = 0; i < level[0].Length; ++i){
            Debug.Log("RED: "+level[0][i]);
        }
        placePlatforms(level);
        audioController.CalledStart(level);
    }

    //This function generates a base path to be used in creating
    //the actual colored paths. 
    public int[] generateBasePath(int levelLength){
        //initialize path array
        int[] path = new int[levelLength];
        //loop over the length of the level
        for (int i = 0; i < levelLength; i++){
            //create random left/right value depending on setting
            path[i] = Random.Range(-widthVariety, widthVariety+1);
        }
        return path;
    }

    //This function takes in a base path, and generates a desired amount of colored paths:
    //returns a 2d array with the color in the left index and position in the right index
    public int[][] generateColoredPaths(int[] path, int numColors){
        //initializing the 2d level array
        level = new int[numColors][];      //int  [COLOR] [POSITION] = LEFT/RIGHT VALUE
        for (int i = 0; i < numColors; ++i){
            level[i] = new int[path.Length];
        }
        //loop over each color
        for (int c = 0; c < numColors; ++c){
            //Debug.Log("current color: " + c);
            //loop over the position on the current color array
            for (int p = 0; p < path.Length; ++p){
                //if it is the main color (color 0) then generate left/right positions based on main path
                if (c == 0){
                    level[c][p] = path[p];
                }else{ // if it isnt the main path color, create a new random value between left/right maximums
                    if (Random.Range(0f, 100f) < skipPercent){
                        level[c][p] = 100;
                    }
                    else level[c][p] = Random.Range(-widthVariety, widthVariety+1);
                }
            }
        }
        return level;
    }
    public void placePlatforms(int[][] level){
        //var for saving current position
        Vector3 position = transform.position;
        //array for blocking edge cases:
        bool[] elevatedArray = new bool[level[0].Length];
        //loop over each side position INDEX of the level (looping over the forward of the array)
        for (int xPos = 0; xPos < levelLength; ++xPos){
            //create the main path platforms using the first color
            Instantiate(Platforms[0], position + new Vector3(level[0][xPos]*2.2f,0,jumpDistance), transform.rotation); //ADDED *2 FOR PLATFORM SIZE
            //set bool array to false to start
            elevatedArray[xPos] = false;
            //loop over the rest of the colors
            for (int color = 1; color < numColors; ++color){
                float blockHeight = 0;
                //
                //can add variable height to platform placememnt
                //

                //check each color (other than current) and see if it is blocked
                bool isBlocked = false;
                for (int i = 0; i < numColors; i++){
                    //blocking out same color
                    if (i != color){
                        if (level[color][xPos] == level[i][xPos]) isBlocked = true;
                    }
                }

                //for triple same location edge case:
                if (isBlocked && elevatedArray[xPos]&&level[0][xPos] == level[color][xPos])blockHeight += 1;
                
                //if it is blocked, raise the platform by 1
                if (isBlocked && !elevatedArray[xPos]){
                    blockHeight += 2f;
                    elevatedArray[xPos] = true;
                }
                
                //instantiate the other colors based on previous cube
                //Also check if the cube should be skipped
                if (level[color][xPos] != 100){
                    GameObject platform = Instantiate(Platforms[color], position + new Vector3(level[color][xPos]*2.2f,blockHeight,jumpDistance), transform.rotation);
                    if (isBlocked){
                        platform.transform.localScale = new Vector3(1.1f,1f,1.1f);
                    }
                }
            }
            //set position to next platform on the MAIN PATH
            position = position + new Vector3(level[0][xPos]*2.2f,0,jumpDistance);
            //place the next chackpoint platform if its the last platforms
            if (xPos == levelLength-1){
                Instantiate(Checkpoint, position + new Vector3(0,0,10), transform.rotation);
            }
        }
    }
    public int[][] GetLevel(){
        return level;
    }
}
