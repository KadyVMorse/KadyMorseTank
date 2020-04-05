using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //states the game objects in the level and the list of players or enemy/spawnpoints for each to be generated,the volume and the differnt game states
    public static GameManager instance;
    public Slider mVolume;
    public Slider fVolume;
    public AudioSource myMusic;

    public enum GameState
    {
        MainMenu,
        OptionsMenu,
        StartMenu,
        Gameplay,
        GameOver,
        Paused
    }

    public GameState currentGameState = GameState.MainMenu;
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
    public int numberOfPlayers;
    public GameObject optionsMenu;


    // Runs before any Start() functions run
    //when the game first loads up it is awake and will call on these first before start 
    void Awake()
    {
        //if the instance equals nothing then it will equal to game manager 
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
        if (currentGameState == GameState.Gameplay && instantiatedPlayerTank == null)
        {
            SpawnPlayer(RandomSpawnPoint(playerSpawnPoints));
        }
        //sets the music volume for the slider on update 
        myMusic.volume = mVolume.value;

        
    }

    //states the switchs of the differnt game states 
    public void ChangeState(GameState newState)
    {
        switch (currentGameState)
        {
            case GameState.MainMenu:
                if (newState == GameState.OptionsMenu)
                {
                    
                    // Disable input from main menu.
                    // Activate options menu
                }
                if (newState == GameState.StartMenu)
                {
                    // Disable input from mainmenu.
                    // Activate game start menu
                }
                break;
            case GameState.OptionsMenu:
                if (newState == GameState.MainMenu)
                {
                    // Save changes to options
                    // Deactivate options menu.
                    // Reactivate Main Menu
                }
                if (newState == GameState.Paused)
                {
                    // Save changes to options
                    // Deactivate options menu
                    // Reactivate paused menu
                }
                break;
            case GameState.StartMenu:
                if (newState == GameState.MainMenu)
                {
                    // Deactivate Start Menu
                    // Reactivate Main Menu
                }
                if (newState == GameState.Gameplay)
                {
                    // Deactivate our start menu
                    // Load our level / spawn players / spawn enemies
                    MapGenerator mapGenerator = levelGameObject.GetComponent<MapGenerator>();
                    mapGenerator.StartGame();
                }
                break;
            case GameState.Gameplay:
                if (newState == GameState.Paused)
                {
                    // Pause the simulation.
                    // Pull up pause menu.
                }
                if (newState == GameState.GameOver)
                {
                    // Handle game over behaviors
                    // Saving new high scores
                }
                break;
            case GameState.Paused:
                if (newState == GameState.Gameplay)
                {
                    // Restart the simulation
                    // Remove the pause menu
                }
                if (newState == GameState.MainMenu)
                {
                    // Switch to main menu scene/end the simulation
                    // Activate main menu
                }
                if (newState == GameState.OptionsMenu)
                {
                    // Deactivate pause menu ui
                    // Activate options menu ui
                }
                break;
            case GameState.GameOver:
                if (newState == GameState.Gameplay)
                {
                    // Reload the gameplay scene/end the simulation/restart the simulation
                }
                if (newState == GameState.MainMenu)
                {
                    // Switch to main menu scene/end the sim
                    // Activate main menu
                }
                break;
            default:
                break;
        }
        previousGameState = currentGameState;
        currentGameState = newState;
    }

    public GameObject RandomSpawnPoint(List<GameObject> spawnPoints)
    {
        // Get a random spawn point from inside our list of spawn points.
        int spawnToGet = UnityEngine.Random.Range(0, spawnPoints.Count - 1);
        return spawnPoints[spawnToGet];
    }
    //spawns the player 
    public void SpawnPlayer(GameObject spawnPoint)
    {
        instantiatedPlayerTank = Instantiate(playerTankPrefab, spawnPoint.transform.position, Quaternion.identity);
    }

    //spawn the enemies
    public void SpawnEnemies()
    {
        // Write code for spawning enemies.
        if (enemyTankPrefabs.Length == 0)
        { Debug.LogWarning("Enemy tank prefabs is empty"); }
        for (int i = 0; i < enemyTankPrefabs.Length; ++i)
        {
            if (enemySpawnPoints.Count == 0)
            { Debug.LogWarning("Enemy spawn points list is empty."); }
            GameObject instantiatedEnemyTank =
                Instantiate(enemyTankPrefabs[i], RandomSpawnPoint(enemySpawnPoints).transform.position, Quaternion.identity);
            instantiatedEnemyTanks.Add(instantiatedEnemyTank);
        }
    }

    //loads the high score,Music Volme,FX Volume,and the High Score List
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
            highScores.Add(new ScoreData(PlayerPrefs.GetString("Name1"), PlayerPrefs.GetFloat("Score1")));
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

    //This saves the hiagh scores,Music volume,Fx Volume
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

    //When players quit the game it will save there prefs
    private void OnApplicationQuit()
    {
        SavePrefs();
    }

  
}