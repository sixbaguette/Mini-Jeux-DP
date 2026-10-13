using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DoorUnlocker : MonoBehaviour
{
    private PlayerUI playerUI;
    [SerializeField] private int requiredCoins = 15;
    [SerializeField] private BoxCollider boxCol;
    [SerializeField] private TextMeshProUGUI textStats;
    [SerializeField] private EnemySpawner[] enemySpawner;

    private Coroutine textCoroutine;

    private void Start()
    {
        playerUI = FindAnyObjectByType<PlayerUI>().GetComponent<PlayerUI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerUI != null && playerUI.CoinsAmount >= requiredCoins)
            {
                playerUI.CoinsAmount -= requiredCoins;
                boxCol.enabled = false;
                ShowMessage("Tu as ouvert la porte !");

                foreach (EnemySpawner spawner in enemySpawner)
                {
                    if (spawner != null)
                    {
                        StartCoroutine(CantSpawn(spawner));
                    }
                }
            }
            else
            {
                ShowMessage($"Il te faut {requiredCoins} pièces pour ouvrir la porte !");
            }
        }
    }

    private void ShowMessage(string message)
    {
        if (textCoroutine != null)
        {
            StopCoroutine(textCoroutine);
        }
        textCoroutine = StartCoroutine(TextError(message));
    }

    private IEnumerator TextError(string text)
    {
        textStats.text = text;

        yield return new WaitForSeconds(2);

        textStats.text = "";
    }

    private IEnumerator CantSpawn(EnemySpawner spawner)
    {
        spawner.CantSpawn = true;

        yield return new WaitForSeconds(30);

        spawner.CantSpawn = false;
    }
}
