using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    [SerializeField] private Transform _transform;
    [SerializeField] private Resource _prefabResource;

    private Queue<Resource> _pool;

    public IEnumerable<Resource> PooledObjects => _pool;

    private void Awake()
    {
        _pool= new Queue<Resource>();
    }

    public Resource GetObject()
    {
        if(_pool.Count == 0)
        {
            var resource = Instantiate(_prefabResource);
            resource.transform.parent = _transform;

            return resource;
        }

        return _pool.Dequeue();
    }

    public void PutObject(Resource resource)
    {
        _pool.Enqueue(resource);
        resource.gameObject.SetActive(false);
    }
}
