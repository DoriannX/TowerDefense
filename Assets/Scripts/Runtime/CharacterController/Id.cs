using System.Collections.Generic;
using UnityEngine;

public class Id : MonoBehaviour
{
    private static HashSet<int> _ids = new();
    private int _id;
        
    public void SetId(int id) => _id = id;
        
    public int GetId() => _id;

    private void Awake()
    {
        _id = _ids.Count;
        _ids.Add(_id);
    }

    private void OnDestroy()
    {
        _ids.Remove(_id);
    }
}