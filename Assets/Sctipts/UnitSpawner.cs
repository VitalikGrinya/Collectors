using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] private Unit _unitPrefab;
    [SerializeField] private StorageSpawner _storageSpawner;

    public Unit Spawn(Vector3 randomPositon)
    {
        var newUnit = Instantiate(_unitPrefab, randomPositon, Quaternion.identity);
        newUnit.SetStorageSpawner(_storageSpawner);

        return newUnit;
    }
}
