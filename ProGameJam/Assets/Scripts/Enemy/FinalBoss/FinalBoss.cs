using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;

public class FinalBoss : Enemy
{
    [SerializeField] private Transform _target;
    private Animator _anim;
    private SpriteRenderer _sprite;
    [SerializeField] private Transform _hitbox;
    [SerializeField] private bool _facingRight = true;
    [SerializeField] private float[] _skillCooldowns = new float[6];
    [SerializeField] private float[] _skillLast = new float[6];
    [SerializeField] private float[] _lastUsedTime = new float[6];
    private bool _isUsingSkil = false;
    private bool _isRetreat = false;
    private bool _playerInAir = false;
    private bool _isDead = false;
    [SerializeField] private float _retreatSpeed = 2.0f;

    void Start()
    {
        _anim = GetComponentInChildren<Animator>();
        _sprite = GetComponentInChildren<SpriteRenderer>();
        for (int i = 0; i < _lastUsedTime.Length; i++) {
            _lastUsedTime[i] = -_skillCooldowns[i];
        }
    }
    void Update()
    {
        if (_isDead) {
            return;
        }
        _playerInAir = !_target.GetComponent<Player>().GetIsGround();
        if (_isUsingSkil) return;
        if (!_isRetreat) {
            if (!TrySkills() && (Vector2.Distance(transform.position, _target.position) < 3.0f)) {
                StartCoroutine(RetreatRoutine());
            } else {
                Movement();
            }
        }
    }
    private void Movement() {
        if (_isDead) return;
        Vector3 direction = _target.position;
        float distance = transform.position.x - direction.x;
        Flip(distance);
        if (Mathf.Abs(distance) < 2.0f) {
            _anim.SetBool("Moving", false);
            return;
        } 
        transform.position = Vector3.MoveTowards(transform.position, direction, speed * Time.deltaTime);
        _anim.SetBool("Moving", true);
    }
    private void Flip(float distance) {
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
    private void FlipHitbox() {
        Vector3 scale = _hitbox.localScale;
        scale.x = _facingRight ? 1 : -1;
        _hitbox.localScale = scale;
    }
    private bool TrySkills() {
        float distance = Vector2.Distance(_target.position, transform.position);
        if (distance < 6.0f) {
            return ActiveSkill(3, 5);
        } else if (_playerInAir) {
            return ActiveSkill(0, 3);
        } else {
            return ActiveSkill(5, 6);
        }
    }
    private bool ActiveSkill(int from, int to) {
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
        Vector3 direction = (transform.position - _target.position).normalized;
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
    public void Damage() {
        if (_isDead) return;
        health--;
        Debug.Log("Boss HP lefts: " + health);
        if (health < 1)
        {
            _isDead = true;
            _anim.SetTrigger("Death");
        }
    }
    public void DestroyBoss() {
        Destroy(this.gameObject);
    }
}
