using UnityEngine;

public abstract class Boss : Enemy, IDamageable
{
    public int Health {get; set;}
    public override void Init()
    {
        Health = base.health;
        base.Init();
    }
    public void Damage()
    {
        throw new System.NotImplementedException();
    }
}
