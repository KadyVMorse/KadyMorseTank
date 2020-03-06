using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    // Start is called before the first frame update
    //the start of the game 
    void Awake()
    {
        //the game manager will add the data of the enemy spawnpoints to game 
        GameManager.instance.enemySpawnPoints.Add(this.gameObject);
    }

    //when object is destroyed
    void OnDestroy()
    {
        //when the enemy dies the gamemanger will remove it 
        GameManager.instance.enemySpawnPoints.Remove(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
