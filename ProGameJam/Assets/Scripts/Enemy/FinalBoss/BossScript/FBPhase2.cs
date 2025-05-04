using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FBPhase2 : FinalBoss
{
    private bool _hasSpawned = false;
    protected override void Start()
    {
        base.Start();
        _anim.SetTrigger("Spawn");
        StartCoroutine(EndSpawnRoutine());
    }
    protected override bool TrySkills()
    {
        float distance = Vector2.Distance(_target.position, transform.position);
        if (distance < 5.0f) {
            return ActiveSkill(3, 4);
        } else if (_playerInAir) {
            return ActiveSkill(0, 3);
        } else {
            return ActiveSkill(4, 5);
        }
    }
    protected override void Update()
    {
        if (!_hasSpawned) return;
        base.Update();
    }
    IEnumerator EndSpawnRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        _hasSpawned = true;
    }
}
