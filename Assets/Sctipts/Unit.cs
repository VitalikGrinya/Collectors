using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Transform _holdPoint;

    private Resource _resource;
    private Transform _resourceTransform;
    private Storage _storage;
    private Transform _storageTransform;
    private Coroutine _coroutine;

    public WorkStatuses WorkStatus { get; private set; }

    private void Awake()
    {
        WorkStatus = WorkStatuses.Rest;
    }

    public void SetParentBase(Storage basement)
    {
        _storage = basement;
        _storageTransform = basement.GetComponent<Transform>();
    }

    public void RecieveResource(Resource resource)
    {
        _resource = resource;
        _resourceTransform = resource.transform;
        WorkStatus = WorkStatuses.GoResource;

        LaunchCoroutine(CollectingResource());
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
        Carry();
    }

    private IEnumerator MovingTo(Transform target)
    {
        while (transform.position != target.position)
        {
            FollowTarget(target);
            yield return null;
        }
    }

    private void LaunchCoroutine(IEnumerator routine)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(routine);
    }

    private void Carry()
    {
        _storage.Store(_resource);
        WorkStatus = WorkStatuses.Rest;
    }

    private void Grab(Resource resource)
    {
        resource.transform.parent = transform;
        resource.transform.position = _holdPoint.position;
        WorkStatus = WorkStatuses.GoBase;
    }

    private void FollowTarget(Transform target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, _speed * Time.deltaTime);
    }
}