using UnityEngine;

public class CoinMovement : MonoBehaviour
{
    // Reference to the Dino so we can check if the coin passes behind it
    private Transform dinoTransform;
    private bool hasLoggedHeight = false;

    void Awake()
    {
        // Find the Dino by tag "Player"
        GameObject dinoObject = GameObject.FindWithTag("Player");
        if (dinoObject != null)
        {
            dinoTransform = dinoObject.transform;
        }
        else
        {
            Debug.LogError("Could not find a GameObject tagged 'Player'!");
        }
    }

    void Update()
    {
        // Move the coin left based on the global speed
        //float currentSpeed = GlobalSpeed.Instance.globalSpeed;
        //transform.position = new Vector2(
          //  transform.position.x - (currentSpeed * Time.deltaTime),
          //  transform.position.y
        //);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // If the Dino collides with this coin
        if (other.CompareTag("Player"))
        {
            DinoMovement dino = other.GetComponentInParent<DinoMovement>();
            if (dino == null) return;

            if (!dino.hasCollectedCoinThisJump)
            {
                dino.hasCollectedCoinThisJump = true;

                // --- MODIFICATION ---
                // Get the parent column and notify it that a coin has been collected.
                CoinColumn parentColumn = GetComponentInParent<CoinColumn>();
                if (parentColumn != null)
                {
                    parentColumn.coinWasCollected = true;
                }
                // --- END MODIFICATION ---

                // Retrieve the Coin component to find out how many points to award
                Coin coinScript = GetComponent<Coin>();
                if (coinScript != null)
                {

                    // --- MODIFICATION START ---
                    // Get the time of the last jump from GameData.
                    float lastJumpTime = 0f;
                    // Assuming GameData.jumps is the list of JumpRecords
                    if (GameData.jumps != null && GameData.jumps.Count > 0)
                    {
                        lastJumpTime = GameData.jumps[GameData.jumps.Count - 1].time;
                    }

                    // fire-and-forget
                    Vector3 popupPos = other.transform.position + Vector3.up * 1.5f;
                    Score.Instance.AddScore(coinScript.coinValue, popupPos);
                    Debug.Log("Dino collected a coin worth " + coinScript.coinValue +
                              " at height = " + transform.position.y);

                    GameData.coinCollisions.Add( new GameData.CoinCollisionRecord(lastJumpTime-Time.timeSinceLevelLoad,dinoTransform.position.y, coinScript.coinValue/100));
                    //RuntimeAnalytics.DumpList("COLLISIONS", GameData.coinCollisions);
                }

                // Destroy this coin so it can¡¦t be collected again
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Dino tried to collect a second coin this jump - ignoring.");
            }
        }
    }
}
