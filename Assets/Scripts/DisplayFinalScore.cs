using UnityEngine;
using TMPro; // Required for using TextMeshPro components

/// <summary>
/// This script should be placed on a GameObject in your "Game Over" scene.
/// It finds the final score from the GameData class and displays it on a
/// TextMeshPro UI text element.
/// </summary>
public class DisplayFinalScore : MonoBehaviour
{
    [Tooltip("Assign the TextMeshPro UI element that will display the final score here.")]
    [SerializeField] private TextMeshProUGUI scoreText;

    void Start()
    {
        // Check if the scoreText has been assigned in the Inspector
        if (scoreText == null)
        {
            Debug.LogError("Score Text (TextMeshPro) has not been assigned in the Inspector!");
            return;
        }

        // Retrieve the final score from the static GameData class
        // Assuming your score is stored in a static variable like 'GameData.score'
        int finalScore = GameData.score;

        // Update the text property of the TextMeshPro component
        scoreText.text = $"{finalScore.ToString()}/40";
    }
}
