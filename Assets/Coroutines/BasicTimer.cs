using UnityEngine;
using UnityEngine.UI;
public class BasicTimer : MonoBehaviour
{
    [SerializeField] private float cooldownDuration = 5f;
    [SerializeField] private Image cooldownImage;
    private bool isOnCooldown = false;
    private float cooldownTimeRemaining = 0f;

    void Start()
    {
        // Initialize
        cooldownImage.enabled = false;
    }

    void Update()
    {
        if (isOnCooldown)
        {
            // Decrease timer
            cooldownTimeRemaining -= Time.deltaTime;

            // Check if cooldown is complete
            if (cooldownTimeRemaining <= 0)
            {
                // Reset cooldown
                cooldownTimeRemaining = 0;
                isOnCooldown = false;
                cooldownImage.enabled = false;
                Debug.Log("Ability ready!");
            }
        }
    }

    [ContextMenu("activate Cooldown")]
    public void ActivateCooldown()
    {
        if (!isOnCooldown)
        {
            isOnCooldown = true;
            cooldownTimeRemaining = cooldownDuration;
            cooldownImage.enabled = true;
            Debug.Log("Ability on cooldown!");
        }
    }
}