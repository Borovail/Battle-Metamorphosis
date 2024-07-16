using UnityEngine;

public class Sword : MonoBehaviour
{
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
        Debug.Log("Hit: " + collision.name);
    }
  
}
