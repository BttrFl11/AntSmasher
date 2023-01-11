using System.Collections;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private float _startTimeBtwShots;
    [SerializeField] private float _attackRange;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private float _timeBtwEnemySearches;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private float _rotationAcceleration;
    [SerializeField] private float _bulletLifetime;

    private float _timeBtwShots;
    private Collider[] _enemiesInRange = new Collider[MAX_ENEMIES_IN_RANGE];
    private Transform _nearestEnemy;

    private const int MAX_ENEMIES_IN_RANGE = 20;

    private bool CanShoot => _timeBtwShots <= 0 && _nearestEnemy != null;

    private void Awake()
    {
        StartCoroutine(StartSearchEnemies());
    }

    private void FixedUpdate()
    {
        _timeBtwShots -= Time.fixedDeltaTime;

        TryRotate();

        if (CanShoot)
            Shoot();
    }

    private void TryRotate()
    {
        if (_nearestEnemy == null)
            return;

        var distance = transform.position - _nearestEnemy.position;
        var lookRotation = Quaternion.LookRotation(distance);
        var rotation = Quaternion.Lerp(transform.rotation, lookRotation, _rotationAcceleration).eulerAngles;
        transform.rotation = Quaternion.Euler(0, rotation.y, 0);
    }

    private void Shoot()
    {
        var bullet = Instantiate(_bulletPrefab, _firePoint.position, Quaternion.identity);
        bullet.Init(-transform.forward);

        Destroy(bullet, _bulletLifetime);

        _timeBtwShots = _startTimeBtwShots;
    }

    private Transform FindNearestEnemy()
    {
        var enemiesCount = Physics.OverlapSphereNonAlloc(transform.position, _attackRange, _enemiesInRange, _enemyLayer);
        if (enemiesCount == 0)
            return null;

        Transform nearestEnemy = _enemiesInRange[0].transform;
        for (int i = 1; i < enemiesCount; i++)
        {
            if((transform.position - nearestEnemy.position).magnitude > (transform.position - _enemiesInRange[i].transform.position).magnitude)
            {
                nearestEnemy = _enemiesInRange[i].transform;
            }
        }

        return nearestEnemy;
    }

    private IEnumerator StartSearchEnemies()
    {
        yield return new WaitForSeconds(_timeBtwEnemySearches);

        _nearestEnemy = FindNearestEnemy();

        StartCoroutine(StartSearchEnemies());
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
}
