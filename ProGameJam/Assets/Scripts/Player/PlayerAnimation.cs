using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator _anim;
    void Start()
    {
        _anim = GetComponentInChildren<Animator>();
    }
    public void Move(float move) {
        _anim.SetFloat("Move", Mathf.Abs(move));
    }
    public void Jump(bool jump) {
        _anim.SetBool("Jumping", jump);
    }
    public void Fall(float velocityY) {
        _anim.SetFloat("VelocityY", velocityY);
    }
}
