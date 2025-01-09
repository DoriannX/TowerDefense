using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEventId", menuName = "ScriptableObjects/GameEvents/NoArg/Id", order = 0)]
public class GameEventId : ScriptableObject, IGameEvent
{
    public Action<int[]> Action { get; private set; }
    
    public override string ToString()
    {
        return GetCleanName();
    }
        
    private string GetCleanName()
    {
        // Utilise la propriété `name` du ScriptableObject
        string rawName = name;

        // Cherche les parenthèses et coupe tout ce qui suit
        int index = rawName.IndexOf('(');
        if (index >= 0)
        {
            return rawName.Substring(0, index).Trim();
        }

        // Retourne le nom tel quel si pas de parenthèses
        return rawName;
    }
        
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
    
    public override string ToString()
    {
        return GetCleanName();
    }
        
    private string GetCleanName()
    {
        // Utilise la propriété `name` du ScriptableObject
        string rawName = name;

        // Cherche les parenthèses et coupe tout ce qui suit
        int index = rawName.IndexOf('(');
        if (index >= 0)
        {
            return rawName.Substring(0, index).Trim();
        }

        // Retourne le nom tel quel si pas de parenthèses
        return rawName;
    }

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