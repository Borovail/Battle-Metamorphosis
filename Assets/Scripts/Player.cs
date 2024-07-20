using System;
using Assets.Scripts;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour, IAttackable
{
    [SerializeField] private float _speed=5f;
    [SerializeField] private float _jumpForce =7f;
    [SerializeField] private Vector2 _groundCheckSize;
    [SerializeField] private int _health=100;

    [SerializeField] private Transform _feet;
    [SerializeField] private LayerMask _groundLayers;
    [SerializeField] private Gun _gun;
    [SerializeField] private Sword _sword;

    private Rigidbody2D _rigidbody;
    private InputSystem_Actions _userInput;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _userInput = new InputSystem_Actions();
    }
    private void OnEnable()
    {
        _userInput.Player.Enable();
        _userInput.Player.Jump.performed += OnJump;
        _userInput.Player.Attack.performed += OnAttack;
        _userInput.Player.Shoot.performed += OnShoot;
    }

    private void FixedUpdate()
    {
        Move(_userInput.Player.Move.ReadValue<Vector2>());
    }

    private void OnDisable()
    {
        _userInput.Player.Jump.performed -= OnJump;
        _userInput.Player.Attack.performed -= OnAttack;
        _userInput.Player.Shoot.performed -= OnShoot;
        _userInput.Player.Disable();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_feet.position, _groundCheckSize);
    }

    private void OnJump(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        var grounded = Physics2D.OverlapBox(_feet.position, _groundCheckSize, _groundLayers);
        if (!grounded) return;
        _rigidbody.velocityY = _jumpForce;
    }

    private void OnShoot(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        var direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        _gun.Shoot(direction);
    }
    private void OnAttack(UnityEngine.InputSystem.InputAction.CallbackContext obj) => _sword.Swing();
    private void Move(Vector2 direction)
    {
        if(direction == Vector2.zero) return;

        var localScaleX = direction.x > 0 ? Math.Abs(transform.localScale.x) : -Math.Abs(transform.localScale.x);
        transform.localScale = new Vector3(localScaleX, transform.localScale.y);
        transform.position += (Vector3)direction * _speed * Time.deltaTime;
    }

    void IAttackable.ApplyDamage(AttackInfo attackInfo)
    {
        if (_health - attackInfo.Damage <= 0)
            Destroy(gameObject);

        _speed += 0.3f;
        _jumpForce += 0.3f;
        _health -= attackInfo.Damage;
    }

    void IAttackable.ApplyKnockback(AttackInfo attackInfo)
    {
        var knockbackDirection = ((Vector2)transform.position - attackInfo.AttackPosition).normalized;

        var hit = Physics2D.Raycast((Vector2)transform.position + knockbackDirection, knockbackDirection, attackInfo.PushForce);
        Debug.Log($"Hit: {hit.collider}");

        Debug.DrawRay((Vector2)transform.position + knockbackDirection, knockbackDirection, Color.black,1f);

        var newKnockbackDistance = hit.collider != null ? hit.distance : attackInfo.PushForce;
        var knockbackForce = knockbackDirection * newKnockbackDistance;

        Debug.Log($"knockbackForce: {knockbackForce} , newKnockbackDistance: {newKnockbackDistance} , knockbackDirection: {knockbackDirection} ");
        _rigidbody.AddForce(knockbackForce, ForceMode2D.Impulse);
    }
}
