using Assets.Scripts;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private AttackInfo _attackInfo;
    [SerializeField] private AnimationClip _swingAnimation;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Swing()
    {
        _animator.Play(_swingAnimation.name);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Hit: {collision}");
        if (collision.TryGetComponent(out IAttackable attackable))
            attackable.TakeAttack(_attackInfo.SetAttackPosition(transform.position));
    }
  
}
