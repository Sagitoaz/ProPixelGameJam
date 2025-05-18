using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FBPhase2 : FinalBoss
{
    [SerializeField] private VideoManager videoManager;
    private bool _hasSpawned = false;
    protected override void Start()
    {
        base.Start();
        _anim.SetTrigger("Spawn");
        StartCoroutine(EndSpawnRoutine());
    }
    protected override bool TrySkills()
    {
        if (Time.time - _lastUsedTime[5] >= _skillCooldowns[5])
        {
            return ActiveSkill(5, 6);
        }
        float distance = Vector2.Distance(_target.position, transform.position);
        if (distance < 5.0f)
        {
            return ActiveSkill(3, 4);
        }
        else if (_playerInAir)
        {
            return ActiveSkill(0, 3);
        }
        else
        {
            return ActiveSkill(4, 5);
        }
    }
    protected override void Update()
    {
        if (!_hasSpawned) return;
        base.Update();
    }
    public override void Damage()
    {
        if (_isDead) return;
        health--;
        Debug.Log("Boss HP lefts: " + health);
        if (health < 1)
        {
            _isDead = true;
            CheckEnding();
            _anim.SetTrigger("Death");
        }
    }
    IEnumerator EndSpawnRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        _hasSpawned = true;
    }
    void CheckEnding()
    {
        if (NPC.totalYesCount >= NPC.totalNoCount)
        {
            videoManager.PlayVideoED(videoManager.Ending2);
        }
        else
        {
            videoManager.PlayVideoED(videoManager.Ending3);
        }
    }
}
