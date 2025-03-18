using UnityEngine;

public class Coin : MonoBehaviour
{
    public int value = 10;
    public AudioClip collectSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ScoreManager.instance.AddPoints(value);
            AudioManager.instance.PlaySFX(collectSound);
            Destroy(gameObject);


        }
    }
}
