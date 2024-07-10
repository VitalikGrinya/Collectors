using System.Collections;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    [SerializeField] private Resource _prefab;
    [SerializeField] private float _delay;

    [SerializeField] private float _spawnPositionY;
    [SerializeField] private float _maxBoundX;
    [SerializeField] private float _maxBoundY;

    [SerializeField] private int _maxInstance;
    [SerializeField] private int _minInstance;

    private Pool<Resource> _pool;

    private void Awake()
    {
        _pool = new Pool<Resource>(_prefab, transform, transform, _minInstance);
    }

    private void Start()
    {
        StartCoroutine(Spawning());
    }

    public void Reset()
    {
        _pool.Reset();
    }

    private IEnumerator Spawning()
    {
        var waitSpawn = new WaitForSeconds(_delay);

        while (enabled)
        {
            Spawn();
            yield return waitSpawn;
        }
    }

    private void Spawn()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-_maxBoundX, _maxBoundX), _spawnPositionY, Random.Range(-_maxBoundY, _maxBoundY));
        Resource resource = _pool.Peek();
        resource.transform.position = spawnPosition;
    }
}