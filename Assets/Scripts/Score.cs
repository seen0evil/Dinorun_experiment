using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    [Header("HUD References")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject popupPrefab;   // ✔ drag PopupUI prefab
    [SerializeField] private Canvas hudCanvas;

    public static Score Instance { get; private set; }
    public static int totalScore;
    private Transform playerT; 

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        playerT = GameObject.FindGameObjectWithTag("Player").transform;

    }

    public void AddScore(int value, Vector3 worldPos)
    {
        // spawn under the HUD canvas
        var pop = Instantiate(popupPrefab, hudCanvas.transform);
        pop.GetComponent<UIFW>().target = playerT;
        var rt = pop.GetComponent<RectTransform>();
        rt.anchoredPosition = WorldToCanvasPosition(worldPos);

        pop.GetComponent<ScorePopup>().Init(value);

        totalScore += value;
        scoreText.text = totalScore.ToString();
    }

    private Vector2 WorldToCanvasPosition(Vector3 worldPos)
    {
        RectTransform canvasRect = hudCanvas.transform as RectTransform;

        // 1. Convert world → viewport (0‑1 range, camera‑independent)
        Vector3 viewPos = Camera.main.WorldToViewportPoint(worldPos);

        // 2. Turn viewport into canvas space
        float x = (viewPos.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f);
        float y = (viewPos.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f);

        return new Vector2(x, y);
    }
}
