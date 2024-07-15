using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _groundCheckRadius;
    [SerializeField] private Transform _feet;
    [SerializeField] private LayerMask _groundLayers;

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
    }

    private void FixedUpdate()
    {
        Move(_userInput.Player.Move.ReadValue<Vector2>());
    }

    private void OnDisable()
    {
        _userInput.Player.Jump.performed -= OnJump;
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
    private void Move(Vector2 direction) => _rigidbody.velocityX = direction.x * _speed;
    
}
