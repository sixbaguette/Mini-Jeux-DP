using Unity.VisualScripting;
using UnityEngine;

public class EnemyHeavy : EnemyBehavior
{
    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    private void Start()
    {
        base.Start();

        maxHp = 15;
        currentHp = 15;
        damage = 2;
        speed = 6;

        damage = Damage;
    }

    private void Update()
    {
        base.Update();
    }

    private void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (currentHp <= 0)
        {
            DropCoins(5, gameObject.transform);
        }

        int random = Random.Range(0, 5);
        if (random == 0)
        {
            if (currentHp <= maxHp)
            {
                currentHp += 1;
            }
            else return;
        }
    }
}
