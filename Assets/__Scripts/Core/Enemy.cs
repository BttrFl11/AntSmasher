using MoreMountains.NiceVibrations;
using System;
using System.Collections;
using UnityEngine;

public abstract class Enemy : Damageable
{
    [SerializeField] protected EnemyData _enemyData;
    [SerializeField] protected LayerMask _playerLayer;
    [SerializeField] protected float _rotationOffsetY;
    [SerializeField] private GameObject _bloodTrailPrefab;
    [SerializeField] private float _bloodTrailRadiusMult;
    [SerializeField] protected GameObject _deathModel;

    protected float _moveSpeed;
    protected float _damage;
    protected float _timeBtwAttacks;
    protected float _attackRange;
    protected Transform _target;

    private Collider[] _player = new Collider[1];

    public event Action OnDied;

    public virtual void Init(Transform target)
    {
        _target = target;
    }

    protected virtual void Start()
    {
        _target = FindObjectOfType<PlayerHealth>().transform;

        _moveSpeed = _enemyData.MoveSpeed;
        _damage = _enemyData.Damage;
        _timeBtwAttacks = _enemyData.TimeBtwAttacks;
        _attackRange = _enemyData.AttackRange;
    }

    protected bool PlayerInRange()
    {
        return Physics.OverlapSphereNonAlloc(transform.position, _attackRange, _player, _playerLayer) > 0;
    }

    protected virtual void Rotate()
    {
        var distance = _target.position - transform.position;
        var rotation = Quaternion.LookRotation(distance).eulerAngles;
        transform.rotation = Quaternion.AngleAxis(rotation.y + _rotationOffsetY, Vector3.up);
    }

    protected override void Die()
    {
        for (int i = 0; i < 3; i++)
        {
            var randPos = UnityEngine.Random.insideUnitCircle * _bloodTrailRadiusMult;
            var blood = Instantiate(_bloodTrailPrefab, transform.position + new Vector3(randPos.x, 0.01f, randPos.y), Quaternion.Euler(90, 0, 0));
            Destroy(blood, 4f);
        }

        OnDied?.Invoke();

        var deathModel = Instantiate(_deathModel, transform.position, transform.GetChild(0).rotation);
        Destroy(deathModel, 4f);

        MMVibrationManager.Vibrate();

        Destroy(gameObject);
    }

    protected abstract IEnumerator StartFindTarget();
    protected abstract void Move();
    protected abstract void Attack(PlayerHealth player);

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
}