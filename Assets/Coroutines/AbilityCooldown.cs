using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AbilityCooldown : MonoBehaviour
{
    [SerializeField] private float cooldownDuration = 5f;
    [SerializeField] private Image cooldownImage;
    private bool isOnCooldown = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cooldownImage.enabled = false;
    }

    [ContextMenu("Activate Cooldown")]

    public void ActivateAbility()
    {
        if (!isOnCooldown)
        {
            Debug.Log("Ability activated");
            StartCoroutine(CooldownRoutine());
        } else
        {
            Debug.Log("you are on cooldown");
        }
    }

    private IEnumerator CooldownRoutine()
    {
        isOnCooldown = true;
        cooldownImage.enabled = true;

        yield return new WaitForSeconds(cooldownDuration);

        cooldownImage.enabled = false;
        isOnCooldown = false;
        Debug.Log("Ability ready");
    }
}
