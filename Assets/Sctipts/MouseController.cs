using UnityEngine;

public class FlagPlacer : MonoBehaviour
{
    [SerializeField] private Flag _flagPrefab;
    [SerializeField] private Camera _camera;

    private Storage _storage;
    private Flag _currentFlag;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent(out Storage storage))
                {
                    _storage = storage;
                }
                else if (_storage != null && hit.collider.TryGetComponent(out Ground ground))
                {
                    Vector3 hitPoint = hit.point;

                    if (ground.TryGetComponent(out Collider collider) && collider.bounds.Contains(hitPoint))
                    {
                        if (_currentFlag == null)
                        {
                            _currentFlag = Instantiate(_flagPrefab, new Vector3(hit.point.x, hit.point.y + 1, hit.point.z), Quaternion.identity);
                            _currentFlag.transform.SetParent(_storage.transform);
                            _storage.SetFlag(_currentFlag);
                        }
                        else
                        {
                            _currentFlag.transform.position = new Vector3(hit.point.x, hit.point.y + 1, hit.point.z);
                        }

                        _storage = null;
                    }
                }
            }
        }
    }
}