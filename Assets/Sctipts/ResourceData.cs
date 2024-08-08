using System.Collections.Generic;
using UnityEngine;

public class ResourceData : MonoBehaviour
{
    public Dictionary<Resource, bool> _resourceStates = new Dictionary<Resource, bool>();

    public bool IsResourceBusy(Resource resource)
    {
        _resourceStates.TryGetValue(resource, out bool busy);
        return busy;
    }

    public void OccupyResource(Resource resource)
    {
            _resourceStates[resource] = true;
    }

    public void ReleaseResource(Resource resource)
    {
        if (_resourceStates.ContainsKey(resource))
        {
            _resourceStates.Remove(resource);
        }
    }
}
