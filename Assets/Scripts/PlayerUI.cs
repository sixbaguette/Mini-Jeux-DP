using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public static event Action<int, int> OnHealthChanged;
    public static event Action OnPlayerDied;
    public static event Action<int> OnCoinsChanged;

    [SerializeField] private Slider healthBarSlider;
    [SerializeField] private TextMeshProUGUI coinsCounter;
    [SerializeField] private TextMeshProUGUI waveText;

    [SerializeField] private int coinsAmount = 0;
    private int wave = 0;
    [SerializeField] private int health = 50;
    private int damage;

    public int Health => health;

    public int CoinsAmount
    {
        get { return coinsAmount; }
        set
        {
            coinsAmount = value;
            OnCoinsChanged?.Invoke(coinsAmount);
        }
    }

    private void Start()
    {
        PlayerStats stats = GetComponent<PlayerStats>();
        int maxHP = stats != null ? stats.MaxHealth : 10;

        OnHealthChanged?.Invoke(health, maxHP);
        OnCoinsChanged?.Invoke(coinsAmount);

        healthBarSlider.maxValue = maxHP;
        healthBarSlider.value = health;
    }

    public void UpdateWave()
    {
        wave += 1;
        waveText.text = "Wave N° " + wave.ToString();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            collision.gameObject.TryGetComponent(out EnemyBoss enemy);
            collision.gameObject.TryGetComponent(out EnemyHeavy enemy1);
            collision.gameObject.TryGetComponent(out EnemyLight enemy2);

            if (enemy != null)
            {
                damage = enemy.Damage;
            }
            else if (enemy1 != null)
            {
                damage = enemy1.Damage;
            }
            else if (enemy2 != null)
            {
                damage = enemy2.Damage;
            }

            TakeDamage(damage);
        }
    }

    public void TakeDamage(int amount)
    {
        PlayerStats stats = GetComponent<PlayerStats>();
        int maxHP = stats != null ? stats.MaxHealth : 10;

        health -= amount;

        if (healthBarSlider != null) healthBarSlider.value = health;

        OnHealthChanged?.Invoke(health, maxHP);

        if (health <= 0)
        {
            OnPlayerDied?.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            coinsAmount += 1;
            if (coinsCounter != null)
            {
                coinsCounter.text = "Coins : " + coinsAmount.ToString();
            }

            OnCoinsChanged?.Invoke(coinsAmount);
            Destroy(other.gameObject);
        }
    }

    public void Heal(int amount)
    {
        PlayerStats stats = GetComponent<PlayerStats>();
        int maxHP = stats != null ? stats.MaxHealth : 10;

        health = Mathf.Clamp(health + amount, 0, maxHP);

        if (healthBarSlider != null)
        {
            healthBarSlider.maxValue = maxHP;
            healthBarSlider.value = health;
        }

        OnHealthChanged?.Invoke(health, maxHP);
    }
}