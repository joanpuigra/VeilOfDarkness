using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private GameObject SpawnPoint;

    private void Start()
    {
        foreach (GameObject enemy in enemies)
        {
            enemy.SetActive(false);
        }
        SpawnEnemy();
    }
    
    private void SpawnEnemy()
    {
        foreach (GameObject enemy in enemies)
        {
            enemy.SetActive(true);
            GameObject spawnEnemy = Instantiate(enemy, SpawnPoint.transform.position, SpawnPoint.transform.rotation);
            spawnEnemy.transform.SetParent(SpawnPoint.transform);
        }
    }
}
