using UnityEngine;

public class SpriteControll : MonoBehaviour
{
    [SerializeField] private Player _player;
    public void CallDestroyPlayer() {
        if (_player != null) {
            _player.DestroyPlayer();
        }
    }
}
