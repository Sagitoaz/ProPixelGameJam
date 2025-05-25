using System.Collections;
using UnityEngine;

public class Mine : Enemy, IDamageable
{
    public int Health { get; set; }


    void Start()
    {
        base.Init();
        Health = 1000;
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

    }

    public void Damage()
    {
        if (_isDead) return;
        Health--;
        Debug.Log("Mine hit! Remaining health: " + Health);
        if (Health < 1)
        {
            SpawnCoin();
            _isDead = true;
            OnEnemyDeath?.Invoke();
            StopAllCoroutines();
            StartCoroutine(DeathRoutine());
        }
    }

    IEnumerator DeathRoutine()
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }
}
