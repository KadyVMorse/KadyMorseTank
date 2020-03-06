using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    //defines the variables
    public Powerup powerup;
    public AudioClip FeedbackAudioClip;
    private Transform tf;

    // Start is called before the first frame update
    void Start()
    {
        //defines the gameobject at the begging of the game 
        tf = gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //states what happens when the player or AI enters the trigger box for the powerup
    public void OnTriggerEnter(Collider other)
    {
        // Get the other object's powerup controller
        PowerupController powerupController = other.gameObject.GetComponent<PowerupController>();

        // Check to see if our other object had a powerup controller
        if (powerupController != null)
        {
            //if the powerup is collected then it will play a sound 
            powerupController.Add(powerup);
            if (FeedbackAudioClip != null)
            {
                AudioSource.PlayClipAtPoint(FeedbackAudioClip, tf.position, 1.0f);
            }
            //destroys the object once it is collected 
            Destroy(this.gameObject);
        }
    }
}
