using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Monster : MonoBehaviour, IAttackable
{
    [SerializeField] private LayerMask _detectionLayers;
    [SerializeField] private AttackInfo _attackInfo;

    [SerializeField] private float _detectionRadius = 2.5f;
    [SerializeField] private float _attackRange = 1f;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _attackCooldown = 1f;
    [SerializeField] private int _health = 100;

    [SerializeField] private Transform _leftPatrolPoint;
    [SerializeField] private Transform _rightPatrolPoint;

    private Rigidbody2D _rigidbody;
    private Vector3 _localScale;
    private bool _canAttack = true;

    private void Awake()
    {
        _localScale = transform.localScale;
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        var collider = Physics2D.OverlapCircle(transform.position, _detectionRadius, _detectionLayers);
        if (collider == null)
        {
            PatrolArea();
            return;
        }

        Turn(collider.transform.position);

        if (Vector2.Distance(transform.position, collider.transform.position) <= _attackRange)
            TryAttack(collider);
    }

    private void TryAttack(Collider2D collider)
    {
        if (!_canAttack) return;

        if (!collider.TryGetComponent(out IAttackable attackable)) return;

        if (Random.Range(0, 5) == 3) //temp solution
        {
            Debug.Log("Attack Successful");
            attackable.TakeAttack(_attackInfo.SetAttackPosition((Vector2)transform.position));
        }
        else
            Debug.Log("Attack missed");

        StartCoroutine(AttackCooldown());
    }

    private IEnumerator AttackCooldown()
    {
        _canAttack = false;
        yield return new WaitForSeconds(_attackCooldown);
        _canAttack = true;
    }

    private void Turn(Vector3 target)
    {
        transform.localScale =
            target.x < transform.position.x
                ? new Vector3(-Math.Abs(_localScale.x), _localScale.y, _localScale.z)
                : new Vector3(Math.Abs(_localScale.x), _localScale.y, _localScale.z);
    }

    private void PatrolArea()
    {
        Vector2 currentPosition = transform.position;
        Vector2 newPosition = currentPosition;

        if (transform.localScale.x > 0)
        {
            if (currentPosition.x >= _rightPatrolPoint.position.x)
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y,
                    transform.localScale.z);
            else
                newPosition.x += _speed * Time.deltaTime;
        }
        else
        {
            if (currentPosition.x <= _leftPatrolPoint.position.x)
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y,
                    transform.localScale.z);
            else
                newPosition.x -= _speed * Time.deltaTime;
        }

        transform.position = newPosition;
    }

    void IAttackable.ApplyDamage(AttackInfo attackInfo)
    {
        if (_health - attackInfo.Damage <= 0)
            Destroy(gameObject);

        _health -= attackInfo.Damage;
    }

    void IAttackable.ApplyKnockback(AttackInfo attackInfo)
    {
        var knockbackDirection = ((Vector2)transform.position - attackInfo.AttackPosition).normalized;

        var hit = Physics2D.Raycast((Vector2)transform.position + knockbackDirection, knockbackDirection, attackInfo.PushForce);
        Debug.Log($"Hit: {hit.collider}");

        Debug.DrawRay((Vector2)transform.position + knockbackDirection, knockbackDirection,Color.black,1f);

        var newKnockbackDistance = hit.collider != null ? hit.distance : attackInfo.PushForce;
        var knockbackForce = knockbackDirection * newKnockbackDistance;

        Debug.Log($"knockbackForce: {knockbackForce} , newKnockbackDistance: {newKnockbackDistance} , knockbackDirection: {knockbackDirection} ");
        _rigidbody.AddForce(knockbackForce, ForceMode2D.Impulse);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
}
