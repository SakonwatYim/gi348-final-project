using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, ITakeDamage
{
    public static event Action OnPlayerDeadEvent;
    [Header("Player")]
    [SerializeField] private PlayerConfig playerConfig;

    [Header("Armor Regeneration")]
    [Tooltip("Amount of armor to add each tick (1 per request).")]
    [SerializeField] private int armorRegenAmount = 1;
    [Tooltip("Seconds between each armor increment when out of combat.")]
    [SerializeField] private float armorRegenInterval = 1f;
    [Tooltip("Seconds to wait after last damage before starting armor regen.")]
    [SerializeField] private float armorRegenDelay = 3f;

    private float lastDamageTime = -Mathf.Infinity;
    private float lastRegenTime = -Mathf.Infinity;
    private PlayerDetection detection;

    private void Awake()
    {
        detection = GetComponentInChildren<PlayerDetection>();
    }

    private void Update()
    {
        // Only try to regen if below max armor
        if (playerConfig.Armor < playerConfig.MaxArmor)
        {
            bool noEnemyNearby = detection == null || detection.EnemyTarget == null;
            bool delayElapsed = Time.time - lastDamageTime >= armorRegenDelay;
            bool intervalElapsed = Time.time - lastRegenTime >= armorRegenInterval;

            if (noEnemyNearby && delayElapsed && intervalElapsed)
            {
                // Increase armor by discrete steps (1 per tick by default)
                playerConfig.Armor = Mathf.Min(playerConfig.Armor + armorRegenAmount, playerConfig.MaxArmor);
                lastRegenTime = Time.time;
            }
        }
    }

    public void RecoverHealth(float amount) 
    {
        playerConfig.CurrentHealth += amount;
        if (playerConfig.CurrentHealth > playerConfig.MaxHealth)
        {
            playerConfig.CurrentHealth = playerConfig.MaxHealth;
        }
    }

    public void TakeDamage(float amount)
    {
        // record last damage time to delay regen
        lastDamageTime = Time.time;

        DamageManager.Instance.ShowDamage(amount, transform);
        if (playerConfig.Armor > 0)
        {
            // Break armor
            // Damage health
            float remainingDamage = amount - playerConfig.Armor;
            playerConfig.Armor = Mathf.Max(playerConfig.Armor - amount, 0f);
            if (remainingDamage > 0)
            {
                playerConfig.CurrentHealth =
                    Mathf.Max(playerConfig.CurrentHealth - remainingDamage, 0f);
            }
        }
        else
        {
            // Damage health
            playerConfig.CurrentHealth =
                Mathf.Max(playerConfig.CurrentHealth - amount, 0f);
        }

        if (playerConfig.CurrentHealth <= 0)
        {
            PlayerDead();
        }
    }

    private void PlayerDead()
    {
        // play player dead SFX
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayPlayerDead();
        }

        OnPlayerDeadEvent?.Invoke();
        Destroy(gameObject);
    }
}