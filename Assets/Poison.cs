using UnityEngine;

public class Poison : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ScoreManager.instance.AddPoints(-5);
            EnhancedCoolldown.instance.ActivateAbility();
            Destroy(gameObject);
        }
    }
}
