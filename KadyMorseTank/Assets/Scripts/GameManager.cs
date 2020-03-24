﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    //states the game objects in the level and the list of players or enemy/spawnpoints for each to be generated 
    public static GameManager instance;

    public enum GameState 
    { 
        MainMenu,
        OptionsMenu,
        StartMenu,
        Gameplay,
        GameOver,
        Paused
    }
    public GameState cureentGameState =GameState.MainMenu;
    public GameState previousGameState;
    public GameObject levelGameObject;
    public GameObject instantiatedPlayerTank;
    public GameObject playerTankPrefab;
    public List<GameObject> instantiatedEnemyTanks;
    public GameObject[] enemyTankPrefabs;
    public List<GameObject> playerSpawnPoints;
    public List<GameObject> enemySpawnPoints;
    public int highScore;
    public float fxVolume;
    public float musicVolume;
    public List<ScoreData> highScores;

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
        highScores = new List<ScoreData>();
        instantiatedEnemyTanks = new List<GameObject>();
        playerSpawnPoints = new List<GameObject>();
        enemySpawnPoints = new List<GameObject>();
        LoadPrefs();
        highScores.Sort();
        highScores.Reverse();
        highScores = highScores.GetRange(0, 5);
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

    public void ChangeState(GameState newstate)
    {
        switch (cureentGameState)
        {
            case GameState.MainMenu:
                if(newstate == GameState.OptionsMenu)
                {
                    //Diable the input from main menu
                    //Activate options menu
                }
                if(newstate == GameState.StartMenu)
                {
                    //disable the input from main menu
                    //activate game start menu
                }
                break;
            case GameState.OptionsMenu:
                if (newstate == GameState.MainMenu)
                {
                    //save changes to option
                    //deativate options menu 
                    //deactivate Mani Menu
                }
                if (newstate == GameState.Paused)
                {
                    //save changes to options 
                    //deactivate options menu 
                    //Reactivate paused menu
                }

                break;
            case GameState.StartMenu:
                if(newstate == GameState.MainMenu)
                {
                    //deactivate start menu 
                    //reactivate main menu
                }
                if(newstate == GameState.Gameplay)
                {
                    //deactivate our start menu 
                    //Load our level//enemies
                   MapGenerator mapGenerator = levelGameObject.GetComponent<MapGenerator>();
                    mapGenerator.StartGame();
                }
                break;
            case GameState.Gameplay:
                if (newstate == GameState.Paused)
                {
                    //Pause the level
                    //pull up pause menu
                }
                if(newstate == GameState.GameOver)
                {
               // handle gameover behaviors
               //save new highscores
               //replay the game
                }
                break;
            case GameState.Paused:
                if(newstate == GameState.Gameplay)
                {
                    //restart the level 
                    //activate main menu  
                }
                if(newstate == GameState.MainMenu)
                {
                    //switch to main menu and end level
                    //Activate main menu
                }
                if(newstate == GameState.OptionsMenu)
                {
                    //deactivate pause menu ui
                    //activate options menu 
                }
                break;
            case GameState.GameOver:
                if(newstate == GameState.Gameplay)
                {
                    //reload the gameplay score/end the level/restart the level
                }
                if (newstate == GameState.MainMenu)
                {
                    //switch to main menu scene/end the level
                    //Activate main menu
                }
                break;
            default:
                break;
        }
       if(cureentGameState == GameState.Gameplay && newstate == GameState.MainMenu)
        {

        }
        previousGameState = cureentGameState;
        cureentGameState = newstate;
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
    public void LoadPrefs()
    {
        if (PlayerPrefs.HasKey("HighScore"))
        {
            highScore = PlayerPrefs.GetInt("HighScore");
        }
        else
        {
            highScore = 0;
        }
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            musicVolume = PlayerPrefs.GetFloat("MusicVolume");

        }
        else
        {
            musicVolume = 1.0f;
        }
        if (PlayerPrefs.HasKey("FXVolume"))
        {
            fxVolume = PlayerPrefs.GetFloat("FXVolume");
        }
        else
        {
            fxVolume = 1.0f;
        }
        if (PlayerPrefs.HasKey("Score1"))
        {
            highScores[0].name = PlayerPrefs.GetString("Name1");
            highScores[0].score = PlayerPrefs.GetFloat("Score1");
        }
        else
        {
            highScores.Add(new ScoreData("Adam", 0));
        }
        if (PlayerPrefs.HasKey("Score2"))
        {
            highScores.Add(new ScoreData(PlayerPrefs.GetString("Name2"), PlayerPrefs.GetFloat("Score2")));
        }
        else
        {
            highScores.Add(new ScoreData("Bob", 0));
        }
        if (PlayerPrefs.HasKey("Score3"))
        {
            highScores.Add(new ScoreData(PlayerPrefs.GetString("Name3"), PlayerPrefs.GetFloat("Score3")));
        }
        else
        {
            highScores.Add(new ScoreData("Charles", 0));
        }
        if (PlayerPrefs.HasKey("Score4"))
        {
            highScores.Add(new ScoreData(PlayerPrefs.GetString("Name4"), PlayerPrefs.GetFloat("Score4")));
        }
        else
        {
            highScores.Add(new ScoreData("Dan", 0));
        }
        if (PlayerPrefs.HasKey("Score5"))
        {
            highScores.Add(new ScoreData(PlayerPrefs.GetString("Name5"), PlayerPrefs.GetFloat("Score5")));
        }
        else
        {
            highScores.Add(new ScoreData("Eugene", 0));
        }

    }


    public void SavePrefs()
    {
        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("FXVolume", fxVolume);
        PlayerPrefs.SetString("Name1", highScores[0].name);
        PlayerPrefs.SetString("Name2", highScores[1].name);
        PlayerPrefs.SetString("Name3", highScores[2].name);
        PlayerPrefs.SetString("Name4", highScores[3].name);
        PlayerPrefs.SetString("Name5", highScores[4].name);
        PlayerPrefs.SetFloat("Score1", highScores[0].score);
        PlayerPrefs.SetFloat("Score2", highScores[1].score);
        PlayerPrefs.SetFloat("Score3", highScores[2].score);
        PlayerPrefs.SetFloat("Score4", highScores[3].score);
        PlayerPrefs.SetFloat("Score5", highScores[4].score);
        PlayerPrefs.Save();

    }
    private void OnApplicationQuit()
    {
        SavePrefs();
    }
}
