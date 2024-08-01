using UnityEngine;

public class MouseController : MonoBehaviour
{
    [SerializeField] private Flag _flag;
    [SerializeField] private Camera _camera;

    private Storage _storage;
    private Flag _currentFlag;
    private bool _isStorageSelected = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            InputFlag();
        }
    }

    private void InputFlag()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (_isStorageSelected == true)
            {
                InstallFlag(hit);
            }
            else
            {
                SelectStorage(hit);
            }
        }
    }

    private void SelectStorage(RaycastHit hit)
    {
        if (hit.transform.TryGetComponent(out Storage storage))
        {
            _storage = storage;
            _currentFlag = storage.GetComponentInChildren<Flag>();
            _isStorageSelected = true;
        }
    }

    private void InstallFlag(RaycastHit hit)
    {
        if(hit.transform.TryGetComponent(out Ground ground))
        {
            if(_currentFlag == null)
            {
                _currentFlag = Instantiate(_flag, new Vector3(hit.point.x, hit.point.y + 1, hit.point.z), Quaternion.identity);
                _currentFlag.transform.SetParent(_storage.transform);
                _storage.SetFlag(_currentFlag);
            }
            else
            {
                _currentFlag.transform.position = new Vector3(hit.point.x, hit.point.y + 1, hit.point.z);
            }

            _storage = null;
            _isStorageSelected = false;
        }
    }
}
