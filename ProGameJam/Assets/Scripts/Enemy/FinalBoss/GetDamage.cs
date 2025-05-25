using UnityEngine;

public class GetDamage : MonoBehaviour, IDamageable
{
    [SerializeField] private bool _isHead;
    private FinalBoss _boss;
    public int Health { get; set; }
    private void Awake()
    {
        _boss = GetComponentInParent<FinalBoss>();
    }
    public void Damage()
    {
        if (_boss == null) return;
        int dmg = _isHead ? 2 : 1;
        for (int i = 0; i < dmg; i++) {
            _boss.Damage();
        }
    }
}
