using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Vector3 _direction = Vector3.zero;

    public void Init(Vector3 direction)
    {
        _direction = direction;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        transform.Translate(_speed * Time.fixedDeltaTime * _direction);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.TryGetComponent(out Enemy enemy))
        {
            enemy.TakeDamage(1);

            Destroy(gameObject);
        }
    }
}
