using UnityEngine;

public class BossSprite : MonoBehaviour
{
    [SerializeField] private FinalBoss _boss;
    public void CallDestroyPlayer() {
        if (_boss != null) {
            _boss.DestroyBoss();
        }
    }
}
