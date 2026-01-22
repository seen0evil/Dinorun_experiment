using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinColumn : MonoBehaviour
{
    public bool coinWasCollected = false;
    private bool passedPlayer = false;
    private float triggerXPosition = -2.0f; // A point safely behind the player

    void Start()
    {
        // Find the player to set a more accurate trigger position
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // Set the trigger a bit behind the player's initial x position
            triggerXPosition = player.transform.position.x - 2.0f;
        }
    }

    void Update()
    {
        // If the column has passed the trigger point and we haven't already processed it
        if (!passedPlayer && transform.position.x < triggerXPosition)
        {
            passedPlayer = true; // Mark as passed to prevent this from running again

            if (!coinWasCollected)
            {
                // No coin was collected from this column, so log a zero
                Debug.Log("Coin column missed. Logging 0 value.");
                GameData.coinCollisions.Add(new GameData.CoinCollisionRecord(0, 0, 0));
            }
        }
    }
}