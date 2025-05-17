using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : Enemy
{
    [SerializeField] protected Transform _targetP;
    protected Animator _anim;
    protected SpriteRenderer _sprite;
    [SerializeField] protected Transform _hitbox;
    [SerializeField] protected bool _facingRight = true;
    [SerializeField] protected float[] _skillCooldowns;
    [SerializeField] protected float[] _skillLast;
    [SerializeField] protected float[] _lastUsedTime;
    protected bool _isUsingSkil = false;
    protected bool _isRetreat = false;
    protected bool _playerInAir = false;
    [SerializeField] protected float _retreatSpeed = 2.0f;
    protected virtual void Start()
    {
        _anim = GetComponentInChildren<Animator>();
        _sprite = GetComponentInChildren<SpriteRenderer>();
        for (int i = 0; i < _lastUsedTime.Length; i++) {
            _lastUsedTime[i] = -_skillCooldowns[i];
        }
    }
    protected virtual void Update()
    {
        if (_isDead) {
            return;
        }
        _playerInAir = !_targetP.GetComponent<Player>().GetIsGround();
        if (_isUsingSkil) return;
        if (!_isRetreat) {
            if (!TrySkills() && (Vector2.Distance(transform.position, _targetP.position) < 3.0f)) {
                StartCoroutine(RetreatRoutine());
            } else {
                Movement();
            }
        }
    }
    protected void Movement() {
        if (_isDead) return;
        Vector3 direction = _targetP.position;
        float distance = transform.position.x - direction.x;
        Flip(distance);
        if (Mathf.Abs(distance) < 2.0f) {
            _anim.SetBool("Moving", false);
            return;
        } 
        transform.position = Vector3.MoveTowards(transform.position, direction, speed * Time.deltaTime);
        _anim.SetBool("Moving", true);
    }
    protected void Flip(float distance) {
        if (distance > 0) {
            _sprite.flipX = true;
            if (_facingRight) {
                _facingRight = false;
                FlipHitbox();
            }
        } else if (distance < 0) {
            _sprite.flipX = false;
            if (!_facingRight) {
                _facingRight = true;
                FlipHitbox();
            }
        }
    }
    protected void FlipHitbox() {
        Vector3 scale = _hitbox.localScale;
        scale.x = _facingRight ? 1 : -1;
        _hitbox.localScale = scale;
    }
    protected virtual bool TrySkills() {
        float distance = Vector2.Distance(_targetP.position, transform.position);
        if (distance < 6.0f) {
            return ActiveSkill(3, 5);
        } else if (_playerInAir) {
            return ActiveSkill(0, 3);
        } else {
            return ActiveSkill(5, 6);
        }
    }
    protected bool ActiveSkill(int from, int to) {
        List<int> usableSkills = new List<int>();
        for (int i = from; i < to; i++) {
            if (Time.time - _lastUsedTime[i] >= _skillCooldowns[i] && !_isUsingSkil) {
                usableSkills.Add(i);
            }
        }

        if (usableSkills.Count > 0) {
            int randomIndex = Random.Range(0, usableSkills.Count);
            int chosenSkill = usableSkills[randomIndex];
            StartCoroutine(SkillRoutine(chosenSkill));
            return true;
        }
        return false;
    }
    IEnumerator SkillRoutine(int skillIndex) {
        if (_isDead) yield break;
        _isUsingSkil = true;
        if (skillIndex == 5 && this is FBPhase2) {
            Vector3 playerPos = _targetP.position;
            Vector3 dirToBack = (_facingRight ? -1 : 1) * transform.right;
            float[] checkDistances = new float[] { 3.0f, 2.0f, 1.0f };

            Vector3? validTeleportPos = null;

            foreach (float dist in checkDistances)
            {
                Vector3 candidatePos = playerPos + dirToBack * dist;

                bool hasGround = Physics2D.Raycast(candidatePos, Vector2.down, 2.0f, _groundLayer);
                bool isBlocked = Physics2D.OverlapCircle(candidatePos, 0.3f, _groundLayer);

                if (hasGround && !isBlocked)
                {
                    validTeleportPos = candidatePos;
                    break;
                }
            }

            if (validTeleportPos == null)
            {
                Debug.Log("Teleport failed: no valid position.");
                _isUsingSkil = false;
                yield break;
            }

            // Teleport
            transform.position = new Vector3(validTeleportPos.Value.x, transform.position.y, transform.position.z);

            // Flip to player
            Flip(_targetP.position.x - transform.position.x);

            _anim.SetTrigger("Counter");
            _lastUsedTime[skillIndex] = Time.time;
            yield return new WaitForSeconds(_skillLast[skillIndex]);
            _isUsingSkil = false;
            yield break;
        }
        _anim.SetTrigger("Attack" + (skillIndex + 1));
        _lastUsedTime[skillIndex] = Time.time;
        yield return new WaitForSeconds(_skillLast[skillIndex]);
        _isUsingSkil = false;
    }
    IEnumerator RetreatRoutine() {
        if (_isDead) yield break;
        _isRetreat = true;
        _anim.SetBool("Moving", true);
        float retreatTime = 3.5f;
        float timer = 0f;
        Vector3 direction = (transform.position - _targetP.position).normalized;
        while (timer < retreatTime) {
            Flip(-direction.x);
            transform.position += direction * _retreatSpeed * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }
        _anim.SetBool("Moving", false);
        _isRetreat = false;
        yield break;
    }
    public virtual void Damage() {
        if (_isDead) return;
        health--;
        Debug.Log("Boss HP lefts: " + health);
        if (health < 1)
        {
            _isDead = true;
            _anim.SetTrigger("Death");
        }
    }
    public virtual void DestroyBoss() {
        Destroy(this.gameObject);
    }

}
