using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;     // for fast string-building
using System.Linq;

public static class GameData
{
    public static int score;
    public static string id;
    public static float lowerBound;
    public static float upperBound;
    public static float jitter;
    public static bool isDelay;
    public static string packed;
    public static List<PlayerHeightAtCoinRecord> playerHeightsAtCoins = new List<PlayerHeightAtCoinRecord>();

    [System.Serializable]                     // makes debugging in the inspector easy
    public struct JumpRecord
    {
        public float time;
        public JumpRecord(float time)
        { this.time = time; }
        public override string ToString() => $"{time:F2}";
    }
    public static readonly List<JumpRecord> jumps = new();

    public struct CoinCollisionRecord
    {
        public float time, height, value;
        public CoinCollisionRecord(float time, float height, float value)
        { this.time = time;  this.height = height; this.value = value; }

        public override string ToString() => $"{time:F2},{height:F2},{value:F0}";
    }
    public static readonly List<CoinCollisionRecord> coinCollisions = new();

    public struct CoinSpawnRecord
    {
        public float time;
        public CoinSpawnRecord(float time)
        { this.time = time; }

        public override string ToString() => $"{time:F2}";
    }

    public static readonly List<CoinSpawnRecord> coinSpawns = new();

    public static void ClearRuntimeData()
    {
        jumps.Clear();
        coinCollisions.Clear();
        coinSpawns.Clear();
    }

    // 2. ADD THIS NEW STRUCT (or class)
    // This defines the structure for each height record. It stores the time of the event
    // and the player's y-position at that time.
    public struct PlayerHeightAtCoinRecord
    {
        public float time;
        public float height;
        public int score;

        public PlayerHeightAtCoinRecord(float time, float height, int score)
        {
            this.time = time;
            this.height = height;
            this.score = score;
        }

        public override string ToString() => $"{time:F2},{height:F2},{score}";
    }
    public static readonly List<PlayerHeightAtCoinRecord> PlayerHeightAtCoins = new();
}

public class ListWrapper<T>
{
    public List<T> items;
    public ListWrapper(List<T> items) => this.items = items;
}

public static class JsonHelpers
{
    // Call once per list you want to ship
    public static string ToJson<T>(List<T> list, bool pretty = false)
        => JsonUtility.ToJson(new ListWrapper<T>(list), pretty);
}

public static class BatchFormatter
{
    public static string JoinPairs<T>(IEnumerable<T> list) =>              // generic
        string.Join("|", list.Select(r => r.ToString()));                 // "2.30,500|1.70,400|..."
}

public static class RuntimeAnalytics
{
    /*-------------  ROUNDING  ----------------*/
    /// <summary>Rounds a float to 2 decimal places (0.01-second precision).</summary>
    public static float Round2(float value) =>
        Mathf.Round(value * 100f) * 0.01f;

    /*-------------  DEBUG-PRINT  --------------*/
    /// <summary>Dumps an entire list of records to the Console.</summary>
    public static void DumpList<T>(string label, List<T> list)
    {
        var sb = new StringBuilder($"{label} [{list.Count}]: ");
        for (int i = 0; i < list.Count; ++i)
            sb.Append(list[i]).Append(i < list.Count - 1 ? " | " : "");
        Debug.Log(sb.ToString());
    }
}