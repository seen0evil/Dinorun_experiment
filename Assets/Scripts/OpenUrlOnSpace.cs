using UnityEngine;

/// <summary>
/// This script opens a specified web URL in the user's default browser
/// when the spacebar key is pressed.
/// </summary>
public class OpenUrlOnSpace : MonoBehaviour
{
    [Tooltip("The full web URL to open (e.g., https://www.google.com)")]
    [SerializeField] private string webURL = "https://docs.google.com/forms/d/1M_czlGXZF6FT0BkuRigLFlo41KTA2nUVm-SukXrQKdY/edit";
    private bool hasEntered;

    private void Start()
    {
        hasEntered = false;
    }

    void Update()
    {
        // Check if the spacebar was pressed down during this frame
        if (Input.GetKeyDown(KeyCode.Space)&& !hasEntered)
        {
            // Check if the URL is not empty or null before trying to open it
            if (!string.IsNullOrEmpty(webURL))
            {
                // Open the specified URL in the default web browser
                Application.OpenURL(webURL);
                Tinylytics.AnalyticsManager.LogCustomMetric($"{GameData.id} CONDITION", $"{GameData.isDelay.ToString()},{GameData.score.ToString()}");
                Debug.Log("Opening URL: " + webURL);
                hasEntered = true;
            }
            else
            {
                Debug.LogWarning("Web URL is not set or has entered space once");
            }
        }
    }
}
