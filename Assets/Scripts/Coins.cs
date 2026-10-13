using Unity.VisualScripting;
using UnityEngine;

public class Coins : MonoBehaviour
{
    [SerializeField] private GameObject coin;

    public void EnemyDropCoin(int coinsAmount, Transform enemyTransform)
    {
        for (int i = 0; i < coinsAmount; i++)
        {
            float randomX = Random.Range(-1, 2);
            float randomZ = Random.Range(-1, 2);
            Instantiate(coin, 
                new Vector3(enemyTransform.position.x + randomX, 
                0.06f, 
                enemyTransform.position.z + randomZ), 
                Quaternion.identity);
        }
    }
}
