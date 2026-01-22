using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This script replaces the original CoinSpawner. It spawns a single coin at a time
/// at a specified Y position, using the same timing logic as before.
/// </summary>
public class CoinSpawner_Refactored : MonoBehaviour
{
    public static int bucketIndex = 0;

    [Tooltip("The coin prefab. It MUST have the new CoinController script on it.")]
    [SerializeField] GameObject coinPrefab;

    [Header("Spawning Settings")]
    [SerializeField] float spawnInterval = 3f;
    [SerializeField] float runLength = 90f;
    [SerializeField] float xSpawnPosition = 10f;
    [Tooltip("The fixed Y position where the single coin will spawn.")]
    [SerializeField] float spawnYPosition = 4.5f;
    [Tooltip("How long the coin exists before being destroyed if missed. Should be long enough to cross the screen.")]
    [SerializeField] float lifetime = 10f;

    private int maxSpawns;
    private float nextSpawnTime;
    private float jitter;

    private void Start()
    {
        maxSpawns = Mathf.FloorToInt(runLength / spawnInterval);
        // Assuming GameData.jitter exists. If not, you can set a default value.
        if (GameData.jitter > 0)
        {
            jitter = GameData.jitter / 1000;
        }

        bucketIndex = 0; // Reset on start
        ScheduleNext();
    }

    void Update()
    {
        float t = Time.timeSinceLevelLoad;
        if (bucketIndex <= maxSpawns && t >= nextSpawnTime)
        {
            // This trial-specific logic is kept from the original script
            if (SceneManager.GetActiveScene().name == "GamePlaytrial" && bucketIndex > 10)
            {
                return;
            }
            SpawnCoin();
            ScheduleNext();
        }
    }

    /// <summary>
    /// Schedules the next spawn time with a random jitter.
    /// </summary>
    void ScheduleNext()
    {
        float bucketCentre = bucketIndex * spawnInterval;
        float offset = Random.Range(-jitter, jitter);
        nextSpawnTime = Mathf.Clamp(bucketCentre + offset, 0f, runLength);
        bucketIndex++;
    }

    /// <summary>
    /// Spawns a single coin at the specified position.
    /// </summary>
    void SpawnCoin()
    {
        // Log the spawn event for analytics, same as before.
        GameData.coinSpawns.Add(new GameData.CoinSpawnRecord(Time.timeSinceLevelLoad));

        // 1. Instantiate a single coin prefab.
        GameObject coinObject = Instantiate(coinPrefab);

        // 2. Set its position.
        coinObject.transform.position = new Vector2(xSpawnPosition, spawnYPosition);

        // 3. Set a lifetime for the coin as a fallback to clean it up.
        // The CoinController also handles destruction, but this is a good safety measure.
        Destroy(coinObject, lifetime);
    }
}
