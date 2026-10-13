using UnityEngine;

[CreateAssetMenu(fileName = "NewPowerUp", menuName = "Game/PowerUp Data")]
public class PowerUpData : ScriptableObject
{
    [Header("Informations Générales")]
    public string powerUpName = "PowerUp";
    public Sprite icon;

    [Header("Modificateurs de Stats (Ajout brut)")]
    [Tooltip("Augmente la portée des poings (attackRange)")]
    public float attackRangeBonus = 0f;

    [Tooltip("Réduit le cooldown d'attaque (en secondes, ex: -0.1f pour attaquer plus vite)")]
    public float attackCooldownBonus = 0f;

    [Tooltip("Augmente la vitesse de déplacement de base (speed)")]
    public float speedBonus = 0f;

    [Tooltip("Augmente les dégâts infligés par le joueur")]
    public int damageBonus = 0;

    [Tooltip("Augmente la vie maximale et soigne immédiatement du même montant")]
    public int maxHealthBonus = 0;

    [Header("Régénération de vie")]
    [Tooltip("Soigne X points de vie toutes les Y secondes")]
    public int regenAmount = 0;
    public float regenInterval = 1f;

    [Header("Durée")]
    [Tooltip("Cocher si le bonus dure pour toujours (permanent)")]
    public bool isPermanent = true;
    [Tooltip("Si non permanent, durée en secondes")]
    public float duration = 5f;
}