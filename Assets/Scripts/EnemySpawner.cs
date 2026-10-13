using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyLight;
    [SerializeField] private GameObject enemyHeavy;
    [SerializeField] private GameObject enemyBoss;

    private float lightTimer;
    private float heavyTimer;

    private int spawnAmountLight = 6;
    private int spawnAmountHeavy = 2;
    private int spawnAmountBoss = 2;

    private int waveUpgraderLight = 0;
    private int waveUpgraderHeavy = 0;
    private int waveUpgraderBoss = 0;

    private PlayerUI playerUI;

    [SerializeField] private TextMeshProUGUI textEnemySpawn;

    private bool cantSpawn = false;

    public bool CantSpawn
    {
        get { return cantSpawn; }
        set { cantSpawn = value; }
    }

    [SerializeField] List<EnemyLight> enemyLightList = new();
    [SerializeField] List<EnemyHeavy> enemyHeavyList = new();
    [SerializeField] List<EnemyBoss> enemyBossList = new();

    [SerializeField] List<Transform> spawnerPosition = new();

    private void Start()
    {
        playerUI = FindAnyObjectByType<PlayerUI>().GetComponent<PlayerUI>();
    }

    private void Update()
    {
        if (cantSpawn) return;

        lightTimer += Time.deltaTime;
        heavyTimer += Time.deltaTime;

        if (lightTimer >= 20 && enemyLightList.Count < 100)
        {
            SpawnEnemy(spawnAmountLight, 0, "Light Spawned be careful");
            lightTimer = 0;
        }

        if (heavyTimer >= 30 && enemyHeavyList.Count < 50)
        {
            SpawnEnemy(spawnAmountHeavy, 1, "Heavy Spawned be careful");
            heavyTimer = 0;
        }

        if (waveUpgraderBoss >= 20 && enemyBossList.Count < 25)
        {
            SpawnEnemy(spawnAmountBoss, 2, "Boss Spawned be careful");
            spawnAmountBoss += 1;
            waveUpgraderBoss = 0;
        }
    }

    private void SpawnLight(int amount)
    {
        if (enemyLight == null) return;

        for (int i = 0; i < amount; i++)
        {
            GameObject enemyL = Instantiate(enemyLight, SpawnPos(), Quaternion.identity);
            enemyLightList.Add(enemyL.GetComponent<EnemyLight>());
        }

        waveUpgraderLight += 1;

        if (waveUpgraderLight == 1)
        {
            spawnAmountLight += 6;
            waveUpgraderBoss += 6;
            waveUpgraderLight = 0;
        }

        playerUI.UpdateWave();
    }

    private void SpawnHeavy(int amount)
    {
        if (enemyHeavy == null) return;

        for (int i = 0; i < amount; i++)
        {
            GameObject enemyH = Instantiate(enemyHeavy, SpawnPos(), Quaternion.identity);
            enemyHeavyList.Add(enemyH.GetComponent<EnemyHeavy>());
        }

        waveUpgraderHeavy += 1;

        if (waveUpgraderHeavy == 1)
        {
            spawnAmountHeavy += 2;
            waveUpgraderBoss += 6;
            waveUpgraderHeavy = 0;
        }
    }

    private void SpawnBoss(int amount)
    {
        if (enemyBoss == null) return;

        for (int i = 0; i < amount; i++)
        {
            GameObject enemyB = Instantiate(enemyBoss, SpawnPos(), Quaternion.identity);
            enemyBossList.Add(enemyB.GetComponent<EnemyBoss>());
        }
    }

    private IEnumerator TextSpawns(string text)
    {
        textEnemySpawn.text = text;

        yield return new WaitForSeconds(2);

        textEnemySpawn.text = "";
    }

    private void SpawnEnemy(int amount, int number, string text)
    {
        switch (number)
        {
            case 0:
                SpawnLight(amount);
                break;
            case 1:
                SpawnHeavy(amount);
                break;
            case 2:
                SpawnBoss(amount);
                break;
        }

        playerUI.UpdateWave();

        StartCoroutine(TextSpawns(text));
    }

    public Vector3 SpawnPos()
    {
        int random = Random.Range(0, 2);

        if (random == 0)
        {
            return spawnerPosition[0].position;
        }
        else
        {
            return spawnerPosition[1].position;
        }
    }
}
