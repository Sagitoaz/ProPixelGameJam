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
    public void Dash() {
        _anim.SetTrigger("Dash");
    }
    public void Attack(int attackType) {
        switch (attackType) {
            case 1:
                _anim.SetTrigger("Attack1");
                break;
            case 2:
                _anim.SetTrigger("Attack2");
                break;
            case 3:
                _anim.SetTrigger("Attack3");
                break;
        }
    }
    public void Death() {
        _anim.SetTrigger("Death");
    }
}
