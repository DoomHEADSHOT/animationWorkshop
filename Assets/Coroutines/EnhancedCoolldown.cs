using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnhancedCoolldown : MonoBehaviour
{
    [SerializeField] private float cooldownDuration = 5f;
    [SerializeField] private Image cooldownImage;
    [SerializeField] private float blinkThreshold = 3f;
    [SerializeField] private float blinkInterval = 0.2f;
    private bool isOnCooldown = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cooldownImage.enabled = false;
    }

    [ContextMenu("Activate Cooldown with blinking")]
    public void ActivateAbility()
    {
        if (!isOnCooldown)
        {
            Debug.Log("Ability activated");
            StartCoroutine(CooldownRoutine());
        } else
        {
            Debug.Log("ability still on Cooldown");
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
            if(timer<= blinkThreshold && blinkCoroutine == null)
            {
                blinkCoroutine = StartCoroutine(BlinkImage(cooldownImage));
            }
            yield return null;
        }
        if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
      
        cooldownImage.enabled = false;
        isOnCooldown = false;
        
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
