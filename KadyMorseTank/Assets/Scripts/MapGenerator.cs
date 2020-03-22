using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    //declares the list of differnt maps 
    public enum MapType
    {
        Seeded,
        Random,
        MapOfTheDay
    }

    //differnt variables and integers that define the map,and the dimensions of the size of map 
    public MapType mapType = MapType.Random;

    public int mapSeed;

    public int rows;

    public int columns;

    private float roomWidth = 50.0f;

    private float roomHeight = 50.0f;

    public GameObject[] gridPrefabs;

    private Room[,] grid;
    // Start is called before the first frame update

        //when the game starts 
    void Start()
    {
        //the gamemanger on start will get the map generator and switch it to a certain map type
        GameManager.instance.levelGameObject = this.gameObject;
  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //defines the gameobject and what it is going to do 
    public GameObject RandomRoomPrefab()
    {
        //uses the grid prefabs to generate random grids and returns them 
        return gridPrefabs[UnityEngine.Random.Range(0, gridPrefabs.Length)];
    }

    //defines the intger date to int 
    public int DateToInt(DateTime dateToUse)
    {
        //returns the year month, hour , etc for the map of the day 
        return dateToUse.Year + dateToUse.Month + 
               dateToUse.Day + dateToUse.Hour + 
               dateToUse.Minute + dateToUse.Second + 
               dateToUse.Millisecond;
    }
    public void StartGame()
    {
        switch (mapType)
        {
            //the map of the day is the map that uses the current time and date that selects its map 
            case MapType.MapOfTheDay:
                mapSeed = DateToInt(DateTime.Now.Date);
                break;
            //the case of the random map it generates a random map 
            case MapType.Random:
                mapSeed = DateToInt(DateTime.Now);
                break;
            //the orginal map 
            case MapType.Seeded:
                break;
            default:
                //if the map generator is not implemented then it will send a error 
                Debug.LogError("[MapGenerator] Map type not implemented.");
                break;
        }
        //generates the grid 
        GenerateGrid();
        //spawnsplayer from the game manager to a spawn point on each map randomly 
        GameManager.instance.SpawnPlayer(GameManager.instance.RandomSpawnPoint(GameManager.instance.playerSpawnPoints));
        // //spawnsenmies from the game manager to a spawn point on each map randomly 
        GameManager.instance.SpawnEnemies();
    }

    public void GenerateGrid()
    {
        //using the unity engine it will generate random maps when it is random
        UnityEngine.Random.seed = mapSeed;
        grid = new Room[columns,rows];
        // for each row
        for (int row = 0; row < rows; row++)
        {
            // for each column in that row
            for (int col = 0; col < columns; col++)
            {
                float xPosition = roomWidth * col;
                float zPosition = roomHeight * row;
                Vector3 newPosition = new Vector3(xPosition, 0.0f, zPosition);

                GameObject tempRoomObj = Instantiate(RandomRoomPrefab(), newPosition, Quaternion.identity) as GameObject;

                // Set our room's parent object
                tempRoomObj.transform.parent = this.transform;

                tempRoomObj.name = "Room_" + col + "," + row;

                Room tempRoom = tempRoomObj.GetComponent<Room>();
                //sets the doors and which one is going to be open at certain times 
                if (row == 0)
                {
                    tempRoom.doorNorth.SetActive(false);
                }
                else if (row == rows - 1)
                {
                    tempRoom.doorSouth.SetActive(false);
                }
                else
                {
                    tempRoom.doorSouth.SetActive(false);
                    tempRoom.doorNorth.SetActive(false);
                }

                if (col == 0)
                {
                    tempRoom.doorEast.SetActive(false);
                }
                else if (col == columns - 1)
                {
                    tempRoom.doorWest.SetActive(false);
                }
                else
                {
                    tempRoom.doorEast.SetActive(false);
                    tempRoom.doorWest.SetActive(false);
                }
                grid[col, row] = tempRoom;

            }
        }
    }
}
