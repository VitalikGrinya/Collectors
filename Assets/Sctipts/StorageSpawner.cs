using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageSpawner : MonoBehaviour
{
    [SerializeField] private Storage _storagePrefab;
    [SerializeField] private Flag _flag;

    public Storage Spawn(Vector3 position, Flag flag)
    {
        Storage newStorage = Instantiate(_storagePrefab, position, Quaternion.identity);
        newStorage.SetFlag(flag);
        newStorage.SetFlagPlaced();

        return newStorage;
    }
}
