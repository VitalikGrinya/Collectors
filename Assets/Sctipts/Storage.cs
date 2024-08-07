using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Storage : MonoBehaviour
{
    [SerializeField] private Scanner _checker;
    [SerializeField] private List<Unit> _units;
    [SerializeField] private Unit _unit;
    [SerializeField] private float _orderDelay = 0.5f;
    [SerializeField] private Flag _flag;

    private bool _isFlagPlaced = false;
    private List<Resource> _resources;
    private int _storedResources = 0;
    private int _unitCost = 3;
    private Coroutine _coroutine;
    private StorageSpawner _storageSpawner;


    public event Action<int> StoredResourcesChanged;

    private void Awake()
    {
        _resources = new List<Resource>();
    }

    private void Start()
    {
        _coroutine = StartCoroutine(OrderingResources());
    }

    private void Update()
    {
        AddUnits(_unit);

        if (_isFlagPlaced == false)
        {
            CreateUnit();
        }

        foreach (Unit unit in _units)
        {
            InitUnit(unit);
        }
    }

    private void OnEnable()
    {
        _checker.ResourceFinded += WriteResource;
    }

    private void OnDisable()
    {
        _checker.ResourceFinded -= WriteResource;
    }

    private void CreateUnit()
    {
        if (_storedResources >= _unitCost)
        {
            var unit = Instantiate(_unit, transform.position, Quaternion.identity);
            _storedResources -= _unitCost;
            unit.SetStorageSpawner(_storageSpawner);
            _units.Add(unit);
            StoredResourcesChanged?.Invoke(_storedResources);
        }
    }

    private void AddUnits(Unit unit)
    {
        if(_units.Count == 0)
        {
            _units.Add(unit);
        }
    }

    public void SetFlagPlaced()
    {
        _isFlagPlaced = false;
    }

    public void SetFlag(Flag flag)
    {
        _flag = flag;
        _isFlagPlaced = true;

    }

    public void StoreResource(Resource resource)
    {
        Add(resource.Value);
        resource.transform.parent = transform;
        Destroy(resource.gameObject);
    }

    private bool TryGetRestUnit(out Unit result)
    {
        foreach (Unit unit in _units)
        {
            if (unit.WorkStatus == WorkStatuses.Rest)
            {
                result = unit;
                return true;
            }
        }

        result = null;
        return false;
    }

    private void Add(int value)
    {
        if (value <= 0)
            return;

        _storedResources += value;

        StoredResourcesChanged?.Invoke(_storedResources);
    }

    private void InitUnit(Unit unit)
    {
        unit.SetParentBase(transform.GetComponent<Storage>());
    }

    private void OrderResource()
    {
        if (_resources.Count > 0)
        {
            if (TryGetRestUnit(out Unit unit))
            {
                Resource resource = _resources.ElementAt(0);
                unit.MoveToResource(resource);
                _resources.Remove(resource);
            }
        }
    }

    private void WriteResource(Resource resource)
    {
        _resources.Add(resource);
    }

    private IEnumerator OrderingResources()
    {
        var wait = new WaitForSeconds(_orderDelay);

        while (true)
        {
            OrderResource();
            yield return wait;
        }
    }
}