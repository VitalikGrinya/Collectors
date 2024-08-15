using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageSpawner : MonoBehaviour
{
    [SerializeField] private Storage _storagePrefab;
    [SerializeField] private ResourceData _resourceData;
    [SerializeField] private MouseController _mouseController;

    public Storage Spawn(Vector3 position, Flag flag)
    {
        Storage newStorage = Instantiate(_storagePrefab, position, Quaternion.identity);

        newStorage.SetFlag(flag);
        newStorage.IsCreatedUnit = true;
        newStorage.IsFlagPlaced = false;
        newStorage.SetResourceData(_resourceData);

        return newStorage;
    }
}
