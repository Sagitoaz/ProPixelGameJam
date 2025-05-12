using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [System.Serializable]
    public class SpawnPoint
    {
        public Transform _spawnLocation;
        public GameObject _currentEnemy;
    }
    public List<SpawnPoint> _spawnPoints;
    public float _respawnDelay = 60f;

    void Start()
    {
        foreach (var point in _spawnPoints)
        {
            SpawnEnemyAt(point);
        }
    }

    void SpawnEnemyAt(SpawnPoint point)
    {
        GameObject enemy = Instantiate(point._currentEnemy, point._spawnLocation.position, Quaternion.identity);
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        enemyScript.OnEnemyDeath = () => StartCoroutine(RespawnCoroutine(point));
    }

    IEnumerator RespawnCoroutine(SpawnPoint point)
    {
        yield return new WaitForSeconds(_respawnDelay);
        SpawnEnemyAt(point);
    }
}
