using UnityEngine;

public interface IDamageable
{
    int Health {get; set;}
    void Damage();
}
