using UnityEngine;

public class PowerUpPickup : MonoBehaviour
{
    [SerializeField] private PowerUpData powerUpData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent(out PlayerStats stats))
            {
                stats.ApplyPowerUp(powerUpData);
            }

            Destroy(gameObject);
        }
    }
}