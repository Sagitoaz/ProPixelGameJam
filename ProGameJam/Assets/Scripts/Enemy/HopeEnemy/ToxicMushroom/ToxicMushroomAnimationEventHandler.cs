using UnityEngine;

public class ToxicMushroomAnimationEventHandler : MonoBehaviour
{
    private ToxicMushroom toxicMushroom;

    void Start()
    {
        toxicMushroom = GetComponentInParent<ToxicMushroom>();
    }

    public void TriggerSpawnFire()
    {
        if (toxicMushroom != null)
        {
            toxicMushroom.SpawnFire();
        }
        else
        {
            Debug.LogWarning("ToxicMushroom script not found in parent!");
        }
    }
}