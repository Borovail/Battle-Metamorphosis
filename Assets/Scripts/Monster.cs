using System;
using Assets.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Monster : MonoBehaviour
{
    [SerializeField] private LayerMask _detectionLayers;
    [SerializeField] private float _detectionRadius = 2.5f;
    [SerializeField] private float _attackRange = 1f;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private int _damage = 10;

    [SerializeField] private Transform _leftPatrolPoint;
    [SerializeField] private Transform _rightPatrolPoint;
   
    private Vector3 _localScale;

    private void Awake()
    {
        _localScale = transform.localScale;
    }

    private void Update()
    {
        var collider = Physics2D.OverlapCircle(transform.position, _detectionRadius, _detectionLayers);
        if (collider == null)
        {
            PatrolArea();
            return;
        }

        Debug.Log("Detected: " + collider.name);

        Turn(collider.transform.position);

        if (Vector2.Distance(transform.position, collider.transform.position) <= _attackRange)
            TryAttack(collider);
    }

    private void TryAttack(Collider2D collider)
    {
        if (!collider.TryGetComponent(out IAttackable attackable)) return;

        if (Random.Range(0, 5) == 3)//temp solution
        {
            Debug.Log("Attack Successful");
            attackable.TakeDamage(new AttackInfo(_damage));
        }
        else
            Debug.Log("Attack missed");
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

        if (transform.localScale.x > 0) // Движение вправо
        {
            if (currentPosition.x >= _rightPatrolPoint.position.x)
            {
                // Разворот влево
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                // Движение вправо
                newPosition.x += _speed * Time.deltaTime;
            }
        }
        else // Движение влево
        {
            if (currentPosition.x <= _leftPatrolPoint.position.x)
            {
                // Разворот вправо
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                // Движение влево
                newPosition.x -= _speed * Time.deltaTime;
            }
        }

        // Применяем новую позицию
        transform.position = newPosition;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }



}
