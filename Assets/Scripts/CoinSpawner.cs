using UnityEngine;
using UnityEngine.SceneManagement;

public class CoinSpawner : MonoBehaviour
{
    public static int bucketIndex = 0;

    [SerializeField] GameObject coinPrefab; // Must have the Coin script on it
    [SerializeField] GameObject moveLeftPrefab; // Prefab with just the MoveLeft script on it

    [Header("Spawning Settings")]
    [SerializeField] float spawnInterval = 3f;
    [SerializeField] float runLength = 90f;
    [SerializeField] float xSpawnPosition = 10f;
    [SerializeField] float startY = -0.5f;
    [SerializeField] float topOfScreenY = 5f;
    [SerializeField] float yIncrement = 0.5f;
    [SerializeField] float lifetime = 5f;
    private int maxSpawns; // == runLength / baseInterval

    private float nextSpawnTime;

    // Define two colors for interpolation
    private Color bronzeColor = new Color(0.75f, 0.75f, 0.75f);
    private Color goldColor = new Color(1f, 0.84f, 0f);
    private float jitter;

    private void Start()
    {
        maxSpawns = Mathf.FloorToInt(runLength / spawnInterval);
        jitter = GameData.jitter / 1000;
        ScheduleNext();
    }

    void Update()
    {
        float t = Time.timeSinceLevelLoad;
        if (bucketIndex <= maxSpawns && t >= nextSpawnTime)
        {
            if (SceneManager.GetActiveScene().name == "GamePlaytrial" && bucketIndex > 10)
            {
                return;
            }
            SpawnCoins();
            ScheduleNext();
        }
    }

    void ScheduleNext()
    {
        // 1. Centre of current bucket (0, 3, 6 … 87)
        float bucketCentre = bucketIndex * spawnInterval;

        // 2. Offset inside bucket, clamped so we never go < 0 or ≥ runLength
        float offset = Random.Range(-jitter, jitter);
        nextSpawnTime = Mathf.Clamp(bucketCentre + offset, 0f, runLength);

        bucketIndex++; // *** now move on to the next bucket ***
    }

    void SpawnCoins()
    {
        // --- MODIFICATION START ---
        // 1. Create a parent object for the entire column.
        //    This object will manage the lifecycle of the column.
        GameObject columnParent = new GameObject("CoinColumn_" + bucketIndex);
        columnParent.transform.position = new Vector2(xSpawnPosition, 0);

        // 2. Add the CoinColumn helper script to this parent.
        //    This script will be responsible for checking if the column was missed.
        columnParent.AddComponent<CoinColumn>();

        // 3. Add a movement component to the parent.
        //    This ensures the whole column moves together.
        //    (Make sure your coin prefabs no longer have their own movement script).
        if (moveLeftPrefab != null)
        {
            Instantiate(moveLeftPrefab, columnParent.transform);
        }
        else
        {
            // Or add a default movement component if you don't use a prefab
            columnParent.AddComponent<MoveLeft>();
        }


        // 4. Destroy the entire column after its lifetime has passed.
        Destroy(columnParent, lifetime);
        // --- MODIFICATION END ---


        float yRange = topOfScreenY - startY;
        int index = 0; // This will let us give each coin an increasing value
        float fract = 0f;

        GameData.coinSpawns.Add(
            new GameData.CoinSpawnRecord(Time.timeSinceLevelLoad));

        // Start from 'startY' and increment by 'yIncrement' until 'topOfScreenY'
        for (float y = startY; y <= topOfScreenY; y += yIncrement)
        {
            if (index < 5)
            {
                GameObject coinObject = Instantiate(coinPrefab);

                // --- MODIFICATION ---
                // Parent the coin to the column and set its local position.
                coinObject.transform.SetParent(columnParent.transform);
                coinObject.transform.localPosition = new Vector2(0, y);

                // NOTE: The individual coin destruction is now handled by the parent.
                // Destroy(coinObject, lifetime); // This line is no longer needed.

                // Now configure the coin’s color and value
                // -- First, compute how far we are from bottom to top in [0..1]
                float fraction = (y - startY) / yRange;
                fract += 0.2f;
                Color coinColor = Color.Lerp(bronzeColor, goldColor, fract);

                // Get the Coin script
                Coin coinScript = coinObject.GetComponent<Coin>();

                // Assign the color and value
                coinScript.SetCoinColor(coinColor);

                // Each coin is worth (index+1) * 100
                coinScript.coinValue = (index + 1) * 100;

                index++;
            }
        }
    }
}
