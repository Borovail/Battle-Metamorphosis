using System;
using Assets.Scripts;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private AttackInfo _attackInfo;
    [SerializeField] private float _speed;
    [SerializeField] private float _raycastDistance;
    [SerializeField] private LayerMask _interactionLayers;

    private Rigidbody2D _ridigbody;
    private Vector2 _direction;

    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }

    private void Awake()
    {
        _ridigbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        var hit = Physics2D.Raycast(transform.position, transform.right, _raycastDistance, _interactionLayers);
        var collider =hit.collider;

        Debug.DrawRay(transform.position, transform.right * _raycastDistance, Color.red, 1f);
        if (collider != null)
        {
            if (collider.TryGetComponent(out IAttackable attackable))
                attackable.TakeAttack(_attackInfo.SetAttackPosition(transform.position));
       
            Debug.Log($"Hit {hit.collider.name}");
            Destroy(gameObject);
        }

        _ridigbody.velocity = _direction * _speed;
    }
}
