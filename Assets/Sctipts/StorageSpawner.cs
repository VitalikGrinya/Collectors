using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageSpawner : MonoBehaviour
{
    [SerializeField] private Storage _storagePrefab;
    [SerializeField] private ResourceData _resourceData;

    public Storage Spawn(Vector3 position, Flag flag)
    {
        Storage newStorage = Instantiate(_storagePrefab, position, Quaternion.identity);

        newStorage.SetFlag(flag);
        newStorage.SetUnitCreated();
        newStorage.SetFlagPlaced();
        newStorage.SetResourceData(_resourceData);

        return newStorage;
    }

    public Storage AddNewUnit(Unit unit)
    {
        List<Unit> units= new List<Unit>();

        Storage newStorage = _storagePrefab;

        newStorage.SetUnits(units);
        
        if (unit != null && units.Contains(unit) == false)
        {
            units.Add(unit);
        }

        return newStorage;
    }
}
