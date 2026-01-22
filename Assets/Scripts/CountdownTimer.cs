using System.Collections;
using UnityEngine;
using UnityEngine.UI;          // Required for UI elements (Text)
using UnityEngine.SceneManagement;  // Required for SceneManager
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    [SerializeField] public int startTime = 90; // How many seconds to start from
    [SerializeField] private TMP_Text timerText;     // Reference to a UI Text component

    private int currentTime;

    void Start()
    {
        // Set the current time to the start value
        currentTime = startTime;

        // Start the coroutine that handles the countdown
        StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        // While we still have time left
        while (currentTime > 0)
        {
            // Update the UI text to show the current time
            timerText.text = currentTime.ToString();

            // Wait for 1 second
            yield return new WaitForSeconds(1f);

            // Decrement the time
            currentTime--;
        }


        // Once the loop ends (time = 0), update UI and then load GameOver scene
        timerText.text = "0";
        if (SceneManager.GetActiveScene().name != "GamePlaytrialv3")
        {
            Tinylytics.AnalyticsManager.LogCustomMetric($"{GameData.id} TEST_1", 
                $"SPAWNS {BatchFormatter.JoinPairs(GameData.coinSpawns)}" + 
                $"JUMPS {BatchFormatter.JoinPairs(GameData.jumps)}");
            Tinylytics.AnalyticsManager.LogCustomMetric($"{GameData.id} TEST_2",
    $" COLLISIONS {BatchFormatter.JoinPairs(GameData.coinCollisions)}");
            /*
            if (GameData.jumps.Count > 0)
                Tinylytics.AnalyticsManager.LogCustomMetric(
                    "JumpBatch",
                    BatchFormatter.JoinPairs(GameData.jumps));          // no commas removed ¡÷ fine

            if (GameData.coinCollisions.Count > 0)
                Tinylytics.AnalyticsManager.LogCustomMetric(
                    "CollisionBatch",
                    BatchFormatter.JoinPairs(GameData.coinCollisions));

            if (GameData.coinSpawns.Count > 0)
                Tinylytics.AnalyticsManager.LogCustomMetric(
                    "SpawnBatch",
                    BatchFormatter.JoinPairs(GameData.coinSpawns));
            */
            GameData.score = Score.totalScore;
            GameData.ClearRuntimeData();
            CoinSpawner.bucketIndex = 0;
            SceneManager.LoadScene("GameOver");
        }
        else {
            Tinylytics.AnalyticsManager.LogCustomMetric($"{GameData.id} PRAC",
                            $"SPAWNS {BatchFormatter.JoinPairs(GameData.coinSpawns)}" +
                            $"JUMPS {BatchFormatter.JoinPairs(GameData.jumps)}" +
                            $" COLLISIONS {BatchFormatter.JoinPairs(GameData.coinCollisions)}");
            /*
            if (GameData.jumps.Count > 0)
                Tinylytics.AnalyticsManager.LogCustomMetric(
                    "JumpBatch",
                    BatchFormatter.JoinPairs(GameData.jumps));          // no commas removed ¡÷ fine

            if (GameData.coinCollisions.Count > 0)
                Tinylytics.AnalyticsManager.LogCustomMetric(
                    "CollisionBatch",
                    BatchFormatter.JoinPairs(GameData.coinCollisions));

            if (GameData.coinSpawns.Count > 0)
                Tinylytics.AnalyticsManager.LogCustomMetric(
                    "SpawnBatch",
                    BatchFormatter.JoinPairs(GameData.coinSpawns));
            */

            GameData.ClearRuntimeData();
            Score.totalScore = 0;
            CoinSpawner.bucketIndex = 0;
            SceneManager.LoadScene("GameTransition");
        }
            
    }
}