using System.Collections;
using UnityEngine;

public class ToxicMushroom : Enemy, IDamageable
{
    [SerializeField] private GameObject _firePrefab;
    [SerializeField] private GameObject _pointA;
    [SerializeField] private GameObject _pointB;
    private Coroutine _attackCoroutine;
    protected bool _isChasing = false;

    public int Health { get; set; }

    void Start()
    {
        base.Init();
        Health = base.health;
        _rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if (_isAttack)
        {
            FaceTarget();
        }
        else if (_isChasing)
        {
            MoveToTarget();
        }
        else
        {
            Patrol();
        }
    }
    protected override void Patrol()
    {
        if (_isIdle)
        {
            return;
        }
        if (_pointA != null && _pointB != null)
        {
            float distanceToA = Vector2.Distance(transform.position, _pointA.transform.position);
            float distanceToB = Vector2.Distance(transform.position, _pointB.transform.position);
            if (_canFlip && (distanceToA < 1.0f || distanceToB < 1.0f))
            {
                StartCoroutine(IdleToFlip());
                return;
            }
        }
        Move();
    }
    public void Damage()
    {
        Health--;
        Debug.Log("Health point lefts: " + Health);
        anim.SetTrigger("Hit");
        if (Health < 1)
        {
            anim.SetTrigger("Death");
            Destroy(gameObject);
        }
    }
    public void StartAttack(Transform player)
    {
        anim.SetBool("Moving", false);
        _isIdle = false;
        _isAttack = true;
        _target = player;
        if (_attackCoroutine == null)
        {
            _attackCoroutine = StartCoroutine(AttackRoutine());
        }
    }
    public void StopAttack()
    {
        _isAttack = false;
        _isIdle = false;
        anim.SetBool("Moving", true);
    }
    public virtual void StartChase(Transform player)
    {
        if (_isAttack) return;
        _isChasing = true;
        _isAttack = false;
        _isIdle = false;
        _target = player;
        anim.SetBool("Moving", true);
    }
    public virtual void StopChase()
    {
        if (_isAttack) return;
        _isChasing = false;
        _target = null;
        anim.SetBool("Moving", false);
    }
    IEnumerator AttackRoutine()
    {
        while (true)
        {
            if (_isAttack)
            {
                anim.SetTrigger("Attack");
                yield return new WaitForSeconds(1.0f);
            }
            yield return new WaitForSeconds(1.0f);
        }
    }
    public void SpawnFire()
    {
        if (_firePrefab != null && _target != null)
        {
            Vector2 spawnPosition = new Vector2(transform.position.x, transform.position.y);
            GameObject fire = Instantiate(_firePrefab, spawnPosition, Quaternion.identity);
            Vector2 direction = _target.position - transform.position;
            fire.GetComponent<Toxicball>().SetDirection(direction);
        }
    }
}