using System.Collections;
using UnityEngine;

public class PlatformDown : MonoBehaviour
{

    private PlatformEffector2D currentPlatform;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            if(currentPlatform != null)
            {
                //Disable the Platform
                StartCoroutine(DisablePlatform());
            }
        }
    }


    IEnumerator DisablePlatform()
    {
        float originalRotation = currentPlatform.rotationalOffset;
        currentPlatform.rotationalOffset = 180f;

        yield return new WaitForSeconds(0.5f);

        if(currentPlatform != null) 
            currentPlatform.rotationalOffset = originalRotation;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlatformEffector2D platform = collision.gameObject.GetComponent<PlatformEffector2D>();
        if (platform != null) { 
            currentPlatform = platform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<PlatformEffector2D>() == currentPlatform)
        {
            currentPlatform = null;
        }
    }
}
