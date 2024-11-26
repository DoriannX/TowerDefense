using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEventId", menuName = "ScriptableObjects/GameEvents/NoArg/Id", order = 0)]
public class GameEventId : ScriptableObject, IGameEvent
{
    public Action<int[]> Action { get; private set; }
        
    public void Invoke(params int[] id)
    {
        Action?.Invoke(id);
    }

    public void AddListener(Action<int[]> action)
    {
        Action += action;
    }
        
    public void RemoveListener(Action<int[]> action)
    {
        Action -= action;
    }


    public void ClearListeners()
    {
        Action = null;
    }
}
    
public class GameEventId<T> : ScriptableObject, IGameEvent
{
    public Action<T, int[]> Action { get; private set; }

    public void Invoke(T arg, params int[] id)
    {
        Action?.Invoke(arg, id);
    }

    public void AddListener(Action<T, int[]> action)
    {
        Action += action;
    }
        
    public void RemoveListener(Action<T, int[]> action)
    {
        Action -= action;
    }
        
    public void ClearListeners()
    {
        Action = null;
    }
}