using UnityEngine;

/// <summary>
/// This is an all-in-one script for the Coin Prefab. It handles movement, collision,
/// scoring, and the new height-recording logic.
/// It solves the race condition by logging a "miss" by default when the coin passes the player,
/// and then updates that specific log to a "hit" upon successful collision.
/// It also handles the edge case where a collision occurs before the pass is logged.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class CoinController : MonoBehaviour
{
    private float speed;
    private Transform playerTransform;

    // State tracking for the coin's lifecycle
    private bool isCollected = false;
    private bool hasLoggedOutcome = false;

    // This will store the index of the collision record we log, so we can update it later.
    private int loggedCollisionIndex = -1;

    void Start()
    {
        // Set speed from the global speed manager
        speed = GlobalSpeed.Instance.globalSpeed;

        // Find the player GameObject by its tag
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
        else
        {
            Debug.LogError("CoinController: Player object not found! Make sure your player is tagged 'Player'.");
            // Destroy self if player doesn't exist to avoid errors
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // If the coin has already been collected, do nothing further.
        if (isCollected) return;

        // 1. Move the coin to the left
        transform.Translate(Vector2.left * speed * Time.deltaTime, Space.World);

        // 2. Log the outcome as soon as the coin passes the player, assuming it's a miss.
        // This only runs if an outcome hasn't already been logged by a collision.
        if (!hasLoggedOutcome && playerTransform != null && transform.position.x <= playerTransform.position.x)
        {
            float timeToRecord = 0f;
            float currentTime = Time.timeSinceLevelLoad;

            if (GameData.jumps != null && GameData.jumps.Count > 0)
            {
                float lastJumpTime = GameData.jumps[GameData.jumps.Count - 1].time;
                float timeSinceJump = currentTime - lastJumpTime;

                if (timeSinceJump <= 1.5f)
                {
                    timeToRecord = timeSinceJump;
                }
            }

            float playerHeight = playerTransform.position.y;

            // Add the record to the list with a default value of 0 (miss).
            GameData.coinCollisions.Add(
                new GameData.CoinCollisionRecord(timeToRecord, playerHeight, 0)
            );

            // Store the index of this new record so we can find and update it if a collision occurs.
            loggedCollisionIndex = GameData.coinCollisions.Count - 1;

            Debug.Log($"Logged potential outcome at index {loggedCollisionIndex}. Time since jump: {timeToRecord:F2}s. Assuming miss for now.");

            hasLoggedOutcome = true; // Ensure this only runs once per coin
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // If already collected, ignore further collisions
        if (isCollected) return;

        // Check if the collider belongs to the player
        if (other.CompareTag("Player"))
        {
            DinoMovement dino = other.GetComponentInParent<DinoMovement>();
            if (dino == null) return;

            // Use the same logic to prevent collecting multiple coins in one jump
            if (!dino.hasCollectedCoinThisJump)
            {
                isCollected = true;
                dino.hasCollectedCoinThisJump = true;

                // Add score of 1
                Vector3 popupPos = other.transform.position + Vector3.up * 1.5f;
                Score.Instance.AddScore(1, popupPos);

                // --- UPDATE/CREATE LOGIC ---
                // Check if the outcome was already logged in Update().
                if (hasLoggedOutcome)
                {
                    // If yes, this is a "late hit". Update the existing record.
                    if (loggedCollisionIndex != -1)
                    {
                        var originalRecord = GameData.coinCollisions[loggedCollisionIndex];
                        var updatedRecord = new GameData.CoinCollisionRecord(
                            originalRecord.time,
                            originalRecord.height,
                            1 // Update value to 1 for a successful collection
                        );
                        GameData.coinCollisions[loggedCollisionIndex] = updatedRecord;
                        Debug.Log($"Player collected a coin. Updated record at index {loggedCollisionIndex} to a success.");
                    }
                }
                else
                {
                    // If no, this is an "early hit". The log doesn't exist yet, so create it now.
                    float timeToRecord = 0f;
                    float currentTime = Time.timeSinceLevelLoad;

                    if (GameData.jumps != null && GameData.jumps.Count > 0)
                    {
                        float lastJumpTime = GameData.jumps[GameData.jumps.Count - 1].time;
                        float timeSinceJump = currentTime - lastJumpTime;
                        if (timeSinceJump <= 1.5f)
                        {
                            timeToRecord = timeSinceJump;
                        }
                    }
                    float playerHeight = playerTransform.position.y;
                    GameData.coinCollisions.Add(
                        new GameData.CoinCollisionRecord(timeToRecord, playerHeight, 1) // Log with value 1 immediately
                    );
                    Debug.Log($"Player collected a coin (early hit). Created new success record.");
                    // Set the flag to prevent Update() from creating a duplicate miss record.
                    hasLoggedOutcome = true;
                }

                // Destroy the coin immediately upon collection
                Destroy(gameObject);
            }
        }
    }
}