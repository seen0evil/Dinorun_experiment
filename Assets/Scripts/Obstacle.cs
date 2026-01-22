using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Obstacle : MonoBehaviour
{
    public GameObject border;

    // A reference to the Dino's Transform so we can check its position
    private Transform dinoTransform;

    // A simple boolean to ensure we only log "jumped over" once
    private bool hasLoggedHeight = false;

    void Awake()
    {
        // Attempt to find the redBorder by name
        border = GameObject.Find("redBorder");
        if (border == null)
        {
            Debug.LogError("Could not find an object named 'redBorder'!");
        }
        else
        {
            // Set alpha to 0 at the start
            SetBorderAlpha(0f);
        }

        // Find the dinosaur. You could also use: GameObject.Find("DinoName");
        // but using tags is often cleaner.
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
        // Move the obstacle left based on the global speed
        float currentSpeed = GlobalSpeed.Instance.globalSpeed;
        transform.position = new Vector2(transform.position.x - (currentSpeed * Time.deltaTime), transform.position.y);

        // If the obstacle has passed behind the dino (no collision so far),
        // log the dino's height exactly once.
        if (!hasLoggedHeight && dinoTransform != null && transform.position.x < dinoTransform.position.x)
        {
            hasLoggedHeight = true;  // Prevent multiple logs for the same obstacle
            Debug.Log("Dino jumped over obstacle. Dino Height = " + dinoTransform.position.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If the Dino collides with the obstacle
        if (other.CompareTag("Player"))
        {
            // Collision happened, log the Dino's height at impact
            Debug.Log("Dino collided with obstacle at height = " + other.transform.position.y);
            Tinylytics.AnalyticsManager.LogCustomMetric("Collision Height", other.transform.position.y.ToString());

            // Increase score or do whatever game logic you need

            // Flash the red border
            StartCoroutine(FlashBorder());
        }
    }

    private IEnumerator FlashBorder()
    {
        // Set alpha to 20% (for example)
        SetBorderAlpha(0.2f);

        // Wait briefly
        yield return new WaitForSeconds(0.2f);

        // Then set it back to 0%
        SetBorderAlpha(0f);
    }

    // Helper function to set alpha on the border's Image
    private void SetBorderAlpha(float alpha)
    {
        if (border != null)
        {
            Image borderImage = border.GetComponent<Image>();
            if (borderImage != null)
            {
                Color c = borderImage.color;
                c.a = alpha;
                borderImage.color = c;
            }
        }
    }
}

