using UnityEngine;

public class HopeBossAnimationEventHandler : MonoBehaviour
{
    private HopeBoss hopeBoss;

    void Start()
    {
        hopeBoss = GetComponentInParent<HopeBoss>();
        if (hopeBoss == null)
        {
            Debug.LogError("HopeBossAnimationEventHandler: HopeBoss script not found in parent!", this);
        }
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
            Debug.LogWarning("HopeBossAnimationEventHandler: HopeBoss script not found in parent!");
        }
    }

    public void TriggerDeactivateHitbox()
    {
        if (hopeBoss != null)
        {
            Debug.Log("Hitbox Deactivated");
            hopeBoss.DeactivateHitbox();
        }
        else
        {
            Debug.LogWarning("HopeBossAnimationEventHandler: HopeBoss script not found in parent!");
        }
    }

    public void TriggerDie()
    {
        if (hopeBoss != null)
        {
            Debug.Log("HopeBoss is dead!");
            Destroy(hopeBoss.gameObject);
        }
        else
        {
            Debug.LogWarning("HopeBossAnimationEventHandler: HopeBoss script not found in parent!");
        }
    }

    public void TriggerTeleport()
    {
        if (hopeBoss != null)
        {
            Debug.Log("HopeBoss teleported!");
            hopeBoss.TryTeleportBehindTarget();
        }
        else
        {
            Debug.LogWarning("HopeBossAnimationEventHandler: HopeBoss script not found in parent!");
        }
    }

    public void TriggerSpawnCoin()
    {
        if (hopeBoss != null)
        {
            Debug.Log("HopeBoss Spawned Coin!");
            hopeBoss.SpawnCoin();
        }
        else
        {
            Debug.LogWarning("HopeBossAnimationEventHandler: HopeBoss script not found in parent!");
        }
    }
}