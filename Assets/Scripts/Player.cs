using Assets.Scripts;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour, IAttackable
{
    [SerializeField] private float _speed=5f;
    [SerializeField] private float _jumpForce =7f;
    [SerializeField] private float _groundCheckRadius =.1f;
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
    }

    private void FixedUpdate()
    {
        Move(_userInput.Player.Move.ReadValue<Vector2>());
    }

    private void OnDisable()
    {
        _userInput.Player.Jump.performed -= OnJump;
        _userInput.Player.Attack.performed -= OnAttack;
        _userInput.Player.Disable();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_feet.position, _groundCheckRadius);
    }

    private void OnJump(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        var grounded = Physics2D.OverlapCircle(_feet.position, _groundCheckRadius, _groundLayers);
        if (!grounded) return;
        _rigidbody.velocityY = _jumpForce;
    }
    private void OnAttack(UnityEngine.InputSystem.InputAction.CallbackContext obj) => _gun.Shoot(); /*_sword.Swing();*/
    private void Move(Vector2 direction) => transform.position += (Vector3)direction * _speed * Time.deltaTime;

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
