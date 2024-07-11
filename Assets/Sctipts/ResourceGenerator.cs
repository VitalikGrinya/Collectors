using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class ResourceGenerator : MonoBehaviour
{
    [SerializeField] private Resource _prefab;
    [SerializeField] private float _delay;
    [SerializeField] private float _spawnPointY;
    [SerializeField] private float _minSpawnPointX;
    [SerializeField] private float _maxSpawnPointX;
    [SerializeField] private float _minSpawnPointZ;
    [SerializeField] private float _maxSpawnPointZ;
    [SerializeField] private int _poolCapacity = 1;
    [SerializeField] private int _poolMaxSize = 3;

    private ObjectPool<Resource> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Resource>(
            createFunc: () => Instantiate(_prefab),
            actionOnGet: OnGet,
            actionOnRelease: (resource) => resource.gameObject.SetActive(false),
            actionOnDestroy: (resource) => Destroy(resource),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize
            );
    }

    private void Start()
    {
        StartCoroutine(StartSpawning());
    }

    private void OnGet(Resource resource)
    {
        resource.transform.position = new Vector3(UnityEngine.Random.Range(_minSpawnPointX, _maxSpawnPointX), _spawnPointY, UnityEngine.Random.Range(_minSpawnPointZ, _maxSpawnPointZ));
        resource.gameObject.SetActive(true);
    }

    private IEnumerator StartSpawning()
    {
        var wait = new WaitForSeconds(_delay);

        while (true)
        {
            _pool.Get();
            yield return wait;
        }
    }
}