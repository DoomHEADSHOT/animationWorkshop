using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnhancedCooldown : MonoBehaviour
{
    [SerializeField] private float cooldownDuration = 5f;
    [SerializeField] private Image cooldownImage;
    [SerializeField] private float blinkThreshold = 1f;
    [SerializeField] private float blinkInterval = 0.2f;
    private bool isOnCooldown = false;

    void Start()
    {
        cooldownImage.enabled = false;
    }
     [ContextMenu("cooldown with blinking")]
    public void ActivateAbility()
    {
        if (!isOnCooldown)
        {
            // Perform ability action here
            Debug.Log("Ability activated!");

            // Start cooldown
            StartCoroutine(CooldownRoutine());
        }
    }

   
    private IEnumerator CooldownRoutine()
    {
        isOnCooldown = true;
        cooldownImage.enabled = true;

        float timer = cooldownDuration;
        Coroutine blinkCoroutine = null;

        while (timer > 0)
        {
            timer -= Time.deltaTime;

            // Start blinking when cooldown is almost complete
            if (timer <= blinkThreshold && blinkCoroutine == null)
            {
                blinkCoroutine = StartCoroutine(BlinkImage(cooldownImage));
            }

            yield return null;
        }

        // Cleanup
        if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
        cooldownImage.enabled = false;
        isOnCooldown = false;
        Debug.Log("Ability ready!");
    }

    private IEnumerator BlinkImage(Image imageToBlink)
    {
        while (true)
        {
            imageToBlink.enabled = !imageToBlink.enabled;
            yield return new WaitForSeconds(blinkInterval);
        }
    }
}