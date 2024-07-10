using System.Collections.Generic;
using UnityEngine;

public class Pool<Template> where Template : MonoBehaviour
{
    private readonly List<Template> _pool = new();

    private readonly Template _prefab;
    private readonly Transform _storage;
    private readonly Transform _spawnPoint;
    private readonly int _startAmount;

    private int PoolCount => _pool.Count;

    public Pool(Template prefab, Transform container, Transform spawnPoint, int startAmount)
    {
        _prefab = prefab;
        _storage = container;
        _spawnPoint = spawnPoint;
        _startAmount = startAmount;

        for (int i = 0; i < _startAmount; i++)
        {
            Create();
        }
    }

    public void Reset()
    {
        foreach (Template template in _pool)
        {
            template.gameObject.SetActive(false);
        }
    }

    public Template Peek()
    {
        if (TryGetObject(out Template template) == false)
        {
            template = Create();
        }

        template.gameObject.SetActive(true);
        return template;
    }

    private bool TryGetObject(out Template template)
    {
        template = null;

        for (int i = 0; i < PoolCount; i++)
        {
            if (_pool[i].gameObject.activeInHierarchy == false)
            {
                template = _pool[i];
                return true;
            }
        }

        return false;
    }

    private Template Create()
    {
        Template template = Object.Instantiate(_prefab, _storage, _spawnPoint);
        template.gameObject.SetActive(false);
        _pool.Add(template);

        return template;
    }
}