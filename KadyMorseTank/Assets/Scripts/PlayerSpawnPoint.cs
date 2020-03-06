using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnPoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        //adds the players spwnpoints from the game manger to the map 
        GameManager.instance.playerSpawnPoints.Add(this.gameObject);
    }

    void OnDestroy()
    {
        //will destroy teh playerspwn point once spawned in 
        GameManager.instance.playerSpawnPoints.Remove(this.gameObject);
    }
}
