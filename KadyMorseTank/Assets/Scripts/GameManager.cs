using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    //states the game objects in the level and the list of players or enemy/spawnpoints for each to be generated 
    public static GameManager instance;

    public GameObject levelGameObject;
    public GameObject instantiatedPlayerTank;
    public GameObject playerTankPrefab;
    public List<GameObject> instantiatedEnemyTanks;
    public GameObject[] enemyTankPrefabs;
    public List<GameObject> playerSpawnPoints;
    public List<GameObject> enemySpawnPoints;

    // Runs before any Start() functions run
    //when the game first loads up it is awake and will call on these first before start 
    void Awake()
    {
        //if the instance equalls nothing then it will equal to game manager 
        if (instance == null)
        {
            instance = this;
        }
        //if there more than one game manager then it destroys one of them and gives us an error
        else
        {
            Debug.LogError("ERROR: There can only be one GameManager.");
            Destroy(gameObject);
        }
        //if we load up a new scene it will not destroy the gamemanager 
        DontDestroyOnLoad(gameObject);
    }

    //tells the game when it starts what these varabiles mean and that they are a list of game objects 
    void Start()
    {
        instantiatedEnemyTanks = new List<GameObject>();
        playerSpawnPoints = new List<GameObject>();
        enemySpawnPoints = new List<GameObject>();
    }

    //on update of the game 
    void Update()
    {
        //if the playertank equals null in the scene then they will spawn at a random spawn point 
        if (instantiatedPlayerTank == null)
        {
            SpawnPlayer(RandomSpawnPoint(playerSpawnPoints));
        }
    }

    //defines the randomspwanpoint used earlier in code 
    public GameObject RandomSpawnPoint(List<GameObject> spawnPoints)
    {
        // Get a random spawn point from inside our list of spawn points.
        int spawnToGet = UnityEngine.Random.Range(0, spawnPoints.Count - 1);
        return spawnPoints[spawnToGet];
    }

    //defines the spawn player game object 
    public void SpawnPlayer(GameObject spawnPoint)
    {
        //says what our instianted player tank means and instatiates the factors such as prefab the postion and the quternion identity 
        instantiatedPlayerTank = Instantiate(playerTankPrefab, spawnPoint.transform.position, Quaternion.identity);
    }
    //defines and is the code to spawn enemies
    public void SpawnEnemies()
    {
        // Write code for spawning enemies.
        //if there is no enemy prefabs then it will send an error syaing its empty 
        if (enemyTankPrefabs.Length == 0)
        { Debug.LogWarning("Enemy tank prefabs is empty"); }
        //for the intger of the enmey tank prefs
        for (int i = 0; i < enemyTankPrefabs.Length; ++i)
        {
            //if there is no enemy spawn points then we get a messege saying that they are empty 
            if (enemySpawnPoints.Count == 0)
            { Debug.LogWarning("Enemy spawn points list is empty."); }
            //the gameobject equals the instatinated prefab,the spawnpoint,location,and the identity 
            GameObject instantiatedEnemyTank =  Instantiate(enemyTankPrefabs[i], RandomSpawnPoint(enemySpawnPoints).transform.position, Quaternion.identity);
            //adds the instiated enemy tank 
            instantiatedEnemyTanks.Add(instantiatedEnemyTank);
        }
    }
}
