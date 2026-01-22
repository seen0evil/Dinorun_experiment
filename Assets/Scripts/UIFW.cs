using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFW : MonoBehaviour
{
    public Transform target;    // set from ScoreManager
    public Vector3 offset = Vector3.up * -10f;
    public float lifetime = 0.8f;

    RectTransform rect;
    Canvas canvas;
    float t;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector2 anchored;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Camera.main.WorldToScreenPoint(target.position + offset),
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main,
            out anchored);

        rect.anchoredPosition = anchored;

        if ((t += Time.deltaTime) > lifetime) Destroy(gameObject);
    }
}
