using UnityEngine;

public class Pointer : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private float _attackRange;
    [SerializeField] private GameObject _ripplesEffect;
    [SerializeField] private Transform _canvas;

    private Camera _camera;
    private Collider[] _enemies = new Collider[MAX_ENEMIES_PER_TAP];

    private const int MAX_ENEMIES_PER_TAP = 8;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Start()
    {
        InputController.OnTouch += Touch;
    }

    private void OnDisable()
    {
        InputController.OnTouch -= Touch;
    }

    private void Touch(Vector2 touchPos)
    {
        var ray = _camera.ScreenPointToRay(touchPos);

        if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _groundLayer))
        {
            var enemyCount = Physics.OverlapSphereNonAlloc(hit.point, _attackRange, _enemies, _enemyLayer);
            if(enemyCount > 0)
            {
                for (int i = 0; i < enemyCount; i++)
                {
                    if (_enemies[i].TryGetComponent(out Damageable damageable))
                    {
                        damageable.TakeDamage(1);
                    }
                }
            }
        }

        var ripple = Instantiate(_ripplesEffect, touchPos, Quaternion.identity, _canvas);
        Destroy(ripple, 0.5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(Vector3.zero, _attackRange);
    }
}