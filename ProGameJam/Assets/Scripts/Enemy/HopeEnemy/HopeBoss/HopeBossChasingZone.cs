using UnityEngine;

public class HopeBossChasingZone : MonoBehaviour
{
    [SerializeField] private HopeBoss _enemy;
    private Animator hopeBossAnim;
    private void Start()
    {
        hopeBossAnim = _enemy.Animator;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            hopeBossAnim.SetTrigger("Teleport");
            _enemy.speedUp(2);
            _enemy.StartChase(collision.transform);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            _enemy.speedDown(2);
            _enemy.StopChase();
        }
    }
}
