using System;
using UnityEngine;

public class Checker : MonoBehaviour
{
    public event Action<Resource> ResourceFinded;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Resource resource))
        {
            ResourceFinded?.Invoke(resource);
        }
    }
}