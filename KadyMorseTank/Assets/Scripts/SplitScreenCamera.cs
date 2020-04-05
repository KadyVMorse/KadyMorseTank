using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitScreenCamera : MonoBehaviour
{

    // We need a reference to the camera
    private Camera playerCamera;
    public TankData data;
    // Start is called before the first frame update
    void Start()
    {
        playerCamera = gameObject.GetComponent<Camera>();
        // We need to know if this is a single-player or multiplayer game.
        // If it's single player, then we change nothing.
        // If it's multiplayer, then we need to know if this is player 1, or player 2.
        if (GameManager.instance.numberOfPlayers > 1)
        {
            if (data.playerNumber == 1)
            {
                // Draw player 1 on the top.
                playerCamera.rect = new Rect(0f, 0.5f, 1f, 0.5f);
            }
            else
            {
                // Draw player 2 on the bottom.
                playerCamera.rect = new Rect(0f, 0f, 1f, 0.5f);
            }
        }
    }

        // Update is called once per frame
        void Update()
    {
        
    }
}
