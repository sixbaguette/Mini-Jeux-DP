using UnityEngine;

public class EnemyLight : EnemyBehavior
{
    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    private void Start()
    {
        base.Start();

        maxHp = 5;
        currentHp = 5;
        damage = 1;
        speed = 10;

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
            DropCoins(3, gameObject.transform);
        }
    }
}
