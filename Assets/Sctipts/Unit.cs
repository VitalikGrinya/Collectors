using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Transform _holdPoint;

    private Resource _resource;
    private Transform _resourceTransform;
    private Storage _storage;
    private StorageSpawner _storageSpawner;
    private Flag _flag;
    private Transform _storageTransform;
    private Coroutine _coroutine;

    public WorkStatuses WorkStatus { get; private set; }

    private void Awake()
    {
        WorkStatus = WorkStatuses.Rest;
    }

    public void SetParentBase(Storage parentStorage)
    {
        _storage = parentStorage;
        _storageTransform = parentStorage.transform;
    }

    public void MoveToResource(Component newTarget)
    {
        if (newTarget == null)
            return;

        if (newTarget is Resource resource)
        {
            _resource = resource;
            _resourceTransform = resource.transform;
            WorkStatus = WorkStatuses.GoResource;

            LaunchCoroutine(CollectingResource());
        }
        else if (newTarget is Flag flag)
        {
            _flag = flag;
            _resourceTransform = flag.transform;
            WorkStatus = WorkStatuses.GoResource;

            LaunchCoroutine(CollectingResource());
        }
    }

    public void SetStorageSpawner(StorageSpawner storageSpawner)
    {
        _storageSpawner = storageSpawner;
    }

    private IEnumerator CollectingResource()
    {
        yield return MovingTo(_resourceTransform);

        Grab(_resource);
        LaunchCoroutine(GoingBase());
    }

    private IEnumerator GoingBase()
    {
        yield return MovingTo(_storageTransform);
        TransferResourceToBase();
    }

    private IEnumerator MovingTo(Transform target)
    {
        while (transform.position != target.position)
        {
            MoveTo(target);
            yield return null;
        }

        if (transform.position != target.position)
            MoveTo(target);
    }

    private void LaunchCoroutine(IEnumerator routine)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(routine);
    }

    private void TransferResourceToBase()
    {
        _storage.StoreResource(_resource);
        WorkStatus = WorkStatuses.Rest;
    }

    private void Grab(Resource resource)
    {
        resource.transform.parent = transform;
        resource.transform.position = _holdPoint.position;
        WorkStatus = WorkStatuses.GoBase;
    }

    private void MoveTo(Transform target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, _speed * Time.deltaTime);
    }
}