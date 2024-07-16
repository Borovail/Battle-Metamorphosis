using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private float _coolDown;

    private bool _canShoot = true;

    public void Shoot()
    {
        if (!_canShoot)
        {
            Debug.Log("Gun is reloading");
            return;
        }

        Instantiate(_bulletPrefab, _bulletSpawnPoint);
        Debug.Log("Shoot");
        StartCoroutine(ShootCoroutine());

    }

    private IEnumerator ShootCoroutine()
    {
        _canShoot = false;
        Debug.Log("Gun can not shoot");
        yield return new WaitForSeconds(_coolDown);
        _canShoot = true;
        Debug.Log("Gun can shoot now");
    }

}
