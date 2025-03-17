using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public int score = 0;

    void Awake()
    {
        // Create a singleton instance
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [ContextMenu("ADD coin")] public void AddPoints()
    {
        score += 10;
        UpdateScoreDisplay();
    }

    void UpdateScoreDisplay()
    {
        // Find the score text and update it
        if (scoreText != null)
            scoreText.text = "Score: " + score.ToString();
    }

    // Reference to the UI Text component
    public TextMeshProUGUI scoreText;
}