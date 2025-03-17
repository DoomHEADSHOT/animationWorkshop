using UnityEngine;

public class PlatformDown : MonoBehaviour
{
    private PlatformEffector2D currentPlatform;

    void Update()
    {
        // Check if the player is pressing down
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            // If we're standing on a one-way platform
            if (currentPlatform != null)
            {
                // Temporarily disable the platform's collider
                StartCoroutine(DisablePlatform());
            }
        }
    }

    System.Collections.IEnumerator DisablePlatform()
    {
        // Change the platform's surface arc to allow dropping through
        float originalRotation = currentPlatform.rotationalOffset;
        currentPlatform.rotationalOffset = 180f;

        // Wait a short time
        yield return new WaitForSeconds(0.5f);

        // Reset the platform
        if (currentPlatform != null)
            currentPlatform.rotationalOffset = originalRotation;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if this is a one-way platform
        PlatformEffector2D platform = collision.gameObject.GetComponent<PlatformEffector2D>();
        if (platform != null)
        {
            currentPlatform = platform;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Check if we're leaving the current platform
        if (collision.gameObject.GetComponent<PlatformEffector2D>() == currentPlatform)
        {
            currentPlatform = null;
        }
    }
}
