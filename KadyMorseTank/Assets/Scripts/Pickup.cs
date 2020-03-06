using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public Powerup powerup;
    public AudioClip FeedbackAudioClip;
    private Transform tf;

    // Start is called before the first frame update
    void Start()
    {
        tf = gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        // Get the other object's powerup controller
        PowerupController powerupController = other.gameObject.GetComponent<PowerupController>();

        // Check to see if our other object had a powerup controller
        if (powerupController != null)
        {
            powerupController.Add(powerup);
            if (FeedbackAudioClip != null)
            {
                AudioSource.PlayClipAtPoint(FeedbackAudioClip, tf.position, 1.0f);
            }
            Destroy(this.gameObject);
        }
    }
}
