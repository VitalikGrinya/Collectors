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

    private HashSet<Resource> _resources;

    private bool _isAddUnit = false;
    private int _storedResources = 0;
    private int _unitCost = 3;
        
    private Coroutine _coroutine;

    public event Action<int> StoredResourcesChanged;

    private void Awake()
    {
        _resources = new HashSet<Resource>();

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

    private void Start()
    {


        _coroutine = StartCoroutine(OrderingResources());
    }

    public void CreateUnit()
    {
        if (_storedResources >= _unitCost)
        {
            var unit = Instantiate(_unit, transform.position, Quaternion.identity);
            _units.Add(unit);
            _isAddUnit = true;
        }
    }

    public void StoreResource(Resource resource)
    {
        Add(resource.Value);
        resource.transform.parent = transform;
        resource.gameObject.SetActive(false);
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

        if(_isAddUnit == true)
        {
            _storedResources -= _unitCost;
            _isAddUnit = false;
        }
        
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