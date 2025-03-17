using UnityEngine;

public class Coin : MonoBehaviour
{
    public int pointValue = 10;
    public AudioClip collectSound;

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player touched this coin
        if (other.CompareTag("Player"))
        {
            // Add points
            ScoreManager.instance.AddPoints();

            // Play collect sound
            if (AudioManager.instance != null)
                AudioManager.instance.PlaySFX(collectSound);

            // Destroy the coin
            Destroy(gameObject);
        }
    }
}