using Unity.VisualScripting;
using UnityEngine;

public class EnemyBoss : EnemyBehavior
{
    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    private void Start()
    {
        base.Start();

        maxHp = 50;
        currentHp = 50;
        damage = 5;
        speed = 2;

        damage = Damage;
    }

    private void Update()
    {
        base.Update();
        
    }

    private void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (currentHp <= 15)
        {
            damage = 7;
            speed = 3;
            agent.speed = speed;
        }

        if (currentHp <= 0)
        {
            DropCoins(10, gameObject.transform);
        }
    }
}
