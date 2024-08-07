using TMPro;
using UnityEngine;

public class StoreView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMeshPro;
    [SerializeField] private Storage _storage;

    private void OnEnable()
    {
        _storage.ResourcesChanged += UpdateNumber;
    }

    private void OnDisable()
    {
        _storage.ResourcesChanged -= UpdateNumber;
    }

    private void UpdateNumber(int value)
    {
        _textMeshPro.text = $"{value}";
    }
}