using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _groundCheckRadius;
    [SerializeField] private Transform _feet;
    [SerializeField] private LayerMask _groundLayers;
    [SerializeField] private Gun _gun;

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
    private void OnAttack(UnityEngine.InputSystem.InputAction.CallbackContext obj) => _gun.Shoot();
    private void Move(Vector2 direction) => _rigidbody.velocity = new Vector2(direction.x * _speed, _rigidbody.velocity.y);



}
