using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScorePopup : MonoBehaviour
{
    [Header("Timing / Motion")]
    [SerializeField] private float lifetime = 0.8f;
    [SerializeField] private Vector2 floatOffset = new Vector2(0, 0); // px
    [SerializeField] private AnimationCurve scaleCurve;

    [Header("Visuals")]
    [SerializeField] private Gradient fireColor;
    [SerializeField] private ParticleSystem uiFlames;   // optional

    private RectTransform rt;
    private TextMeshProUGUI txt;
    private Vector2 startPos;
    private float t;

    public void Init(int value)
    {
        rt = GetComponent<RectTransform>();
        txt = GetComponentInChildren<TextMeshProUGUI>();

        // --- new line: make sure scale starts at 1 ---
        rt.localScale = Vector3.one;

        startPos = rt.anchoredPosition;
        txt.text = $"+{value}";

        float lerp = Mathf.InverseLerp(100, 500, value);
        txt.color = fireColor.Evaluate(lerp);

        if (uiFlames != null)
        {
            var m = uiFlames.main;
            m.startSizeMultiplier = 0.5f + lerp;
            uiFlames.Play();
        }
    }

    private void Update()
    {
        t += Time.deltaTime;
        float n = t / lifetime;

        // ease upward inside canvas
        rt.anchoredPosition = Vector2.Lerp(startPos, startPos + floatOffset, n);
        rt.localScale = Vector3.one * scaleCurve.Evaluate(n);

        if (t >= lifetime) Destroy(gameObject);
    }
}