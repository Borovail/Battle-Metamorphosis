using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _raycastDistance;
    [SerializeField] private LayerMask _interactionLayers;

    private Rigidbody2D _ridigbody;

    private void Awake()
    {
        _ridigbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        var hit = Physics2D.Raycast(transform.position, transform.right, _raycastDistance, _interactionLayers);
        Debug.DrawRay(transform.position, transform.right * _raycastDistance, Color.red, 1f);
        if (hit.collider != null)
        {
            Debug.Log($"Hit {hit.collider.name}");
            Destroy(gameObject);
        }

        _ridigbody.velocity = transform.right * _speed;
    }
}
