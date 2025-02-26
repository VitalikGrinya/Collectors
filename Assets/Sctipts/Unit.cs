using System;
using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private UnitMove _move;

    private StorageSpawner _storageSpawner;
    private Storage _storage;
    private Resource _carriedResource;
    private Flag _targetFlag;

    private float _moveSpeed = 6f;
    private float _pickupRange = 0.5f;
    private float _carryDistance = 0.5f;
    private float _buildDistance = 1f;

    public bool IsBusy { get; set; } = false;

    public void SetStorageSpawner(StorageSpawner storageSpawner)
    {
        _storageSpawner = storageSpawner;
    }

    public void SetStorage(Storage storage)
    {
        _storage = storage;
    }

    public void SetDestination(Component targetComponent)
    {
        if (targetComponent == null)
            return;

        IsBusy = true;

        if (targetComponent is Resource resource)
        {
            _carriedResource = resource;
            StartCoroutine(MoveTo(resource.transform, _pickupRange, PickupResource));
        }
        else if (targetComponent is Flag flag)
        {
            _targetFlag = flag;
            StartCoroutine(MoveTo(flag.transform, _buildDistance, CreateNewStorage));
        }
    }

    private void CreateNewStorage()
    {
        Vector3 newStoragePosition = new Vector3(_targetFlag.transform.position.x, 1f, _targetFlag.transform.position.z);
        Storage newStorage = _storageSpawner.Spawn(newStoragePosition, _targetFlag);

        _storage.RemoveFlag(this);

        _storage = newStorage;

        newStorage.AddUnit(this);

        _targetFlag = null;
    }

    private void PickupResource()
    {
        if (_carriedResource == null)
            return;

        _carriedResource.transform.SetParent(transform);
        _carriedResource.transform.localPosition = Vector3.forward * _carryDistance;
        _carriedResource.transform.localRotation = Quaternion.identity;

        StartCoroutine(MoveTo(_storage.transform, _carryDistance, DropResource));
    }

    private void DropResource()
    {
        if (_carriedResource == null || _storage == null)
            return;

        _carriedResource.transform.SetParent(null);
        _storage.TakeResource(_carriedResource);
        _carriedResource.Release();
        _carriedResource = null;

        IsBusy = false;
    }

    public IEnumerator MoveTo(Transform target, float stopDistance, Action OnComplete)
    {
        while ((target.position - transform.position).sqrMagnitude > stopDistance)
        {
            _move.Move(target, _moveSpeed);
            yield return null;
        }

        OnComplete();
    }
}