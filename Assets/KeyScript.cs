using UnityEngine;

public class KeyScript : MonoBehaviour
{
    [SerializeField] GameObject door;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            door.SetActive(true);
            Destroy(gameObject);
        }
    }
}