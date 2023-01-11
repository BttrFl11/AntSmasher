using System.Collections;
using UnityEngine;

public class RedAnt : Enemy
{
    private bool _canMove = true;

    protected override void Start()
    {
        base.Start();

        StartCoroutine(StartFindTarget());
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
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

        transform.position = Vector3.MoveTowards(transform.position, _target.position, _moveSpeed * Time.fixedDeltaTime);

        if ((transform.position - _target.position).magnitude <= _attackRange)
        {
            _canMove = false;
        }
    }
}