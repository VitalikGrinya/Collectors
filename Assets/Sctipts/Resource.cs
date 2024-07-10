using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] private int _defaultValue = 1;

    public int Value { get; private set; }

    public Resource()
    {
        Value = _defaultValue;
    }
}