using UnityEngine;
using UnityEngine.UI;

public class BasicTimer : MonoBehaviour
{
    [SerializeField] private float cooldownDuration = 5f;
    [SerializeField] private Image cooldownImage;
    private bool isOnCooldown = false;
    private float cooldownTimeRemaining = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cooldownImage.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOnCooldown)
        {
            cooldownTimeRemaining -= Time.deltaTime;

            if(cooldownTimeRemaining <= 0f)
            {
                cooldownTimeRemaining = 0;
                isOnCooldown = false;
                cooldownImage.enabled = false;
                Debug.Log("cooldown complete");
            }
        }

    }

    [ContextMenu("Activate Cooldown")]
    private void ActivateCooldown()
    {
        if (!isOnCooldown)
        {
            isOnCooldown = true;
            cooldownTimeRemaining = cooldownDuration;
            cooldownImage.enabled = true;
            Debug.Log("Ability on cooldown");
        }
    }
}
