using System.Collections;
using UnityEditor.Search;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private int baseDamage = 1;
    public int Damage { get; private set; }

    [SerializeField] private int maxHealth = 10;
    [SerializeField] private int regenAmountBase = 1;
    [SerializeField] private int baseInterval = 2;
    public int MaxHealth => maxHealth;

    private PlayerControl playerControl;
    private PlayerUI playerUI;

    private void Awake()
    {
        playerControl = GetComponent<PlayerControl>();
        playerUI = GetComponent<PlayerUI>();
        Damage = baseDamage;
    }

    public void ApplyPowerUp(PowerUpData data)
    {
        playerControl.AttackCooldown = Mathf.Max(0.05f, playerControl.AttackCooldown + data.attackCooldownBonus);
        playerControl.AttackRange = Mathf.Max(0.5f, playerControl.AttackRange + data.attackRangeBonus);
        playerControl.BaseSpeed += data.speedBonus;

        Damage += data.damageBonus;

        if (data.maxHealthBonus > 0)
        {
            maxHealth += data.maxHealthBonus;
            playerUI.Heal(data.maxHealthBonus);
        }

        if (data.regenAmount + regenAmountBase > 0)
        {
            StartCoroutine(RegenRoutine(data.regenAmount, data.regenInterval, data.isPermanent ? -1f : data.duration));
        }

        if (!data.isPermanent)
        {
            StartCoroutine(RemovePowerUpAfterDuration(data));
        }
    }

    private IEnumerator RemovePowerUpAfterDuration(PowerUpData data)
    {
        yield return new WaitForSeconds(data.duration);

        playerControl.AttackCooldown -= data.attackCooldownBonus;
        playerControl.AttackRange -= data.attackRangeBonus;
        playerControl.BaseSpeed -= data.speedBonus;
        Damage -= data.damageBonus;
    }

    private IEnumerator RegenRoutine(int amount, float interval, float totalDuration)
    {
        float timer = 0f;
        while (totalDuration < 0f || timer < totalDuration)
        {
            yield return new WaitForSeconds(interval + baseInterval);
            playerUI.Heal(amount);
            if (totalDuration > 0f) timer += interval;
        }
    }
}