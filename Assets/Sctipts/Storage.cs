using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Scanner), typeof(UnitSpawner))]
public class Storage : MonoBehaviour
{
    [SerializeField] private ResourceData _resourceData;

    private UnitSpawner _unitSpawner;
    private Scanner _scanner;
    private Flag _flag;

    private float _resourceCollectionDelay = 0.1f;
    private float _spawnRadius = 3f;
    private int _resourcesForNewStotage = 5;
    private int _resourcesForNewUnit = 3;
    private int _resourceCount = 0;
    private int _startCountUnits = 3;

    public bool IsFlagPlaced { get; set; } = false;
    public bool IsCreatedUnit { get; set; } = false;

    public event UnityAction<int> ResourcesChanged;

    private List<Unit> _units = new List<Unit>();

    private void Awake()
    {
        _scanner = GetComponent<Scanner>();
        _unitSpawner = GetComponent<UnitSpawner>();
    }

    private void Start()
    {
        if (IsCreatedUnit == false)
        {
            CreateUnit(_startCountUnits);
        }

        StartCoroutine(CollectResources());
    }

    public void SetResourceData(ResourceData resourceData)
    {
        _resourceData = resourceData;
    }

    public void AddUnit(Unit unit)
    {
        if (unit != null && _units.Contains(unit) == false)
        {
            _units.Add(unit);
        }
    }

    public void SetFlag(Flag flag)
    {
        _flag = flag;
        IsFlagPlaced = true;
    }

    public void TakeResource(Resource resource)
    {
        if (IsFlagPlaced)
        {
            if (_resourceCount >= _resourcesForNewStotage)
            {
                SpawnNewStorage();
            }
        }
        else
        {
            int countNewUnit = _resourceCount / _resourcesForNewUnit;

            CreateNewUnit(countNewUnit);
        }

        _resourceCount++;
        ResourcesChanged?.Invoke(_resourceCount);

        _resourceData.ReleaseResource(resource);
    }

    public void RemoveFlag(Unit unit)
    {
        IsFlagPlaced = false;
        Destroy(_flag.gameObject);
        _flag = null;

        DetachUnit(unit);
    }

    private void DetachUnit(Unit unit)
    {
        if (unit != null && _units.Contains(unit))
        {
            _units.Remove(unit);
            unit.SetStorage(null);
            unit.IsBusy = false;
        }
    }

    private void SpawnNewStorage()
    {
        foreach (Unit unit in _units)
        {
            if (unit.IsBusy == false)
            {
                unit.SetDestination(_flag);
                _resourceCount -= _resourcesForNewStotage;
                break;
            }
        }
    }

    private void CreateNewUnit(int countNewUnit)
    {
        if (_resourceCount >= _resourcesForNewUnit)
        {
            for (int i = 0; i < countNewUnit; i++)
            {
                _resourceCount -= _resourcesForNewUnit;
                CreateUnit(1);
            }
        }
    }

    private void CreateUnit(int startCount)
    {
        for (int i = 0; i < startCount; i++)
        {
            Vector3 randomPosition = transform.position + new Vector3(Random.Range(-_spawnRadius, _spawnRadius), 0, Random.Range(-_spawnRadius, _spawnRadius));
            Unit unit = _unitSpawner.Spawn(randomPosition);
            unit.SetStorage(this);
            _units.Add(unit);
        }
    }

    private void SetGoals()
    {
        if (IsFlagPlaced && _resourceCount >= _resourcesForNewStotage)
        {
            foreach (Unit unit in _units)
            {
                if (unit.IsBusy == false)
                {
                    unit.SetDestination(_flag);
                    _resourceCount -= _resourcesForNewStotage;
                    ResourcesChanged?.Invoke(_resourceCount);
                    break;
                }
            }
        }
        else
        {
            List<Resource> availableResources = _scanner.Scan()
                .Where(resource => _resourceData.IsResourceBusy(resource) == false).ToList();

            if (availableResources.Count > 0)
            {
                Resource resource = availableResources.First();

                foreach (Unit unit in _units)
                {
                    if (unit.IsBusy == false)
                    {
                        _resourceData.OccupyResource(resource);
                        unit.SetDestination(resource);
                        break;
                    }
                }
            }
        }
    }

    private IEnumerator CollectResources()
    {
        var wait = new WaitForSeconds(_resourceCollectionDelay);

        while (true)
        {
            yield return wait;
            SetGoals();
        }
    }
}