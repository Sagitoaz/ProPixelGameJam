using UnityEngine;

public class FearBossTriggerZone : MonoBehaviour
{
    private FearBoss _boss;

    void Start()
    {
        _boss = GetComponentInParent<FearBoss>();
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
