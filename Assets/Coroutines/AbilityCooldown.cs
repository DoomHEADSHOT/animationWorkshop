using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AbilityCooldown : MonoBehaviour
{
    [SerializeField] private float cooldownDuration = 5f;
    [SerializeField] private Image cooldownImage;
    private bool isOnCooldown = false;

    void Start()
    {
        cooldownImage.enabled = false;
    }

    [ContextMenu("Activate Cooldown Coroutine")]
    public void ActivateAbility()
    {
        if (!isOnCooldown)
        {
            // Perform ability action here
            Debug.Log("Ability activated!");

            // Start cooldown
            StartCoroutine(CooldownRoutine());
        }
        else
        {
            Debug.Log("Ability on cooldown!");
        }
    }

    private IEnumerator CooldownRoutine()
    {
        isOnCooldown = true;
        cooldownImage.enabled = true;

        yield return new WaitForSeconds(cooldownDuration);

        cooldownImage.enabled = false;
        isOnCooldown = false;
        Debug.Log("Ability ready!");
    }
}
