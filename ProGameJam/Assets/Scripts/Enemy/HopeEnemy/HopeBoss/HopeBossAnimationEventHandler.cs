using UnityEngine;

public class HopeBossAnimationEventHandler : MonoBehaviour
{
    private HopeBoss hopeBoss;

    void Start()
    {
        hopeBoss = GetComponentInParent<HopeBoss>();
    }

    public void TriggerActivateHitbox()
    {
        if (hopeBoss != null)
        {
            Debug.Log("Hitbox Activated");
            hopeBoss.ActivateHitbox();
        }
        else
        {
            Debug.LogWarning("hopeBoss script not found in parent!");
        }
    }

    public void TriggerDeactivateHitbox()
    {
        if (hopeBoss != null)
        {
            Debug.Log("Hitbox Activated");
            hopeBoss.DeactivateHitbox();
        }
        else
        {
            Debug.LogWarning("hopeBoss script not found in parent!");
        }
    }

    public void TriggerDie(){
        if (hopeBoss != null){
            Debug.Log("Hope Boss is dead!");
            Destroy(hopeBoss.gameObject);
        }
    }
    public void TriggerTeleport(){
        if (hopeBoss != null){
            Debug.Log("Hope Boss teleported!");
            hopeBoss.TeleportBehindTarget();
        }
    }
}