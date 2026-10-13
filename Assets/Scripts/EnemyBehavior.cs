using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] protected int currentHp = 5;
    [SerializeField] protected int maxHp = 5;
    [SerializeField] protected int damage = 3;
    [SerializeField] protected float speed = 3f;
    protected GameObject target;
    protected NavMeshAgent agent;

    protected Coins coins;

    protected void Start()
    {
        target = FindAnyObjectByType<PlayerControl>().gameObject;
        agent = GetComponent<NavMeshAgent>();

        agent.speed = speed;

        coins = FindAnyObjectByType<GameManager>().GetComponent<Coins>();
    }

    protected void Update()
    {
        if (target == null || agent == null) return;

        agent.destination = target.transform.position;
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hit"))
        {
            int incomingDamage = 1;
            PlayerStats playerStats = FindAnyObjectByType<PlayerStats>();
            if (playerStats != null)
            {
                incomingDamage = playerStats.Damage;
            }

            currentHp -= incomingDamage;

            if (currentHp <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    protected void DropCoins(int amount, Transform enemyPos)
    {
        if (currentHp <= 0)
        {
            coins.EnemyDropCoin(amount, enemyPos);
        }
    }
}
