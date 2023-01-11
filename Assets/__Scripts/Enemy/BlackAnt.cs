using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackAnt : Enemy
{
    [SerializeField] private Vector2 _minWaypointPosition;
    [SerializeField] private Vector2 _maxWaypointPosition;

    private bool _canMove = true;
    private List<Vector3> _waypoints = new();
    private int _currentWaypointIndex;

    public override void Init(Transform target)
    {
        var position = new Vector3(Random.Range(_minWaypointPosition.x, _maxWaypointPosition.x), 0,
            Random.Range(_minWaypointPosition.y, _maxWaypointPosition.y));

        _waypoints = new();
        _waypoints.Add(position);
        _waypoints.Add(_target.position);

        _currentWaypointIndex = 0;
    }

    protected override void Start()
    {
        base.Start();
        Init(_target);

        StartCoroutine(StartFindTarget());
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
    }

    protected override void Rotate()
    {
        var distance = _waypoints[_currentWaypointIndex] - transform.position;
        var rotation = Quaternion.LookRotation(distance).eulerAngles;
        transform.rotation = Quaternion.AngleAxis(rotation.y + _rotationOffsetY, Vector3.up);
    }

    protected override IEnumerator StartFindTarget()
    {
        yield return new WaitForSeconds(_timeBtwAttacks);

        var targets = Physics.OverlapSphere(transform.position, _attackRange, _playerLayer);
        if(targets.Length > 0)
        {
            if (targets[0].TryGetComponent(out PlayerHealth player))
                Attack(player);
        }

        StartCoroutine(StartFindTarget());
    }

    protected override void Attack(PlayerHealth player)
    {
        // Attack animation
        player.TakeDamage(_damage);
    }

    protected override void Move()
    {
        if (_canMove == false)
            return;

        var targetPos = _waypoints[_currentWaypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetPos, _moveSpeed * Time.fixedDeltaTime);

        if ((transform.position - targetPos).magnitude <= _attackRange)
        {
            if (_currentWaypointIndex + 1 >= _waypoints.Count)
                _canMove = false;
            else
                _currentWaypointIndex++;
        }
    }
}