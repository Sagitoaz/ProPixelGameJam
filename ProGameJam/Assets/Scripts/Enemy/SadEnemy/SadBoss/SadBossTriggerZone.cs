using UnityEngine;

public class SadBossTriggerZone : MonoBehaviour
{
    private SadBoss _boss;

    void Start()
    {
        _boss = GetComponentInParent<SadBoss>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _boss.ActivateBoss(other.transform);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _boss.DeactivateBoss();
        }
    }
}
