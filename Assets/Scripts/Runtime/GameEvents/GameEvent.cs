using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvent", menuName = "ScriptableObjects/GameEvents/NoArg", order = 0)]
public class GameEvent : ScriptableObject, IGameEvent
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
    
public class GameEvent<T> : ScriptableObject, IGameEvent
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