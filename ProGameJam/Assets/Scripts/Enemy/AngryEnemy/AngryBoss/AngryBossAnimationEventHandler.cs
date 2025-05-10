using UnityEngine;

public class AngryBossAnimationEventHandler : MonoBehaviour
{
    private AngryBoss angryBoss;

    void Start()
    {
        angryBoss = GetComponentInParent<AngryBoss>();
    }

    public void TriggerActivateHitbox()
    {
        if (angryBoss != null)
        {
            Debug.Log("Hitbox Activated");
            angryBoss.ActivateHitbox();
        }
        else
        {
            Debug.LogWarning("AngryBoss script not found in parent!");
        }
    }

    public void TriggerDeactivateHitbox()
    {
        if (angryBoss != null)
        {
            Debug.Log("Hitbox Activated");
            angryBoss.DeactivateHitbox();
        }
        else
        {
            Debug.LogWarning("AngryBoss script not found in parent!");
        }
    }

    public void TriggerDie(){
        if (angryBoss != null){
            Debug.Log("Angry Boss is dead!");
            Destroy(angryBoss.gameObject);
        }
    }
}