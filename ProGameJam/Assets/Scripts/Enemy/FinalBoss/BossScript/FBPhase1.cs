using UnityEngine;

public class FBPhase1 : FinalBoss
{
    [SerializeField] private GameObject _bossPhase2;
    public override void DestroyBoss()
    {
        _bossPhase2.SetActive(true);
        _bossPhase2.transform.position = transform.position;
        base.DestroyBoss();
    }
}
