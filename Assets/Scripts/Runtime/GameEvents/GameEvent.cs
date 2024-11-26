using System;
using UnityEngine;

namespace Runtime.GameEvents
{
    [CreateAssetMenu(fileName = "GameEvent", menuName = "ScriptableObjects/GameEvents/GameEvent", order = 0)]
    public class GameEvent : ScriptableObject, IGameEvent
    {
        public Action Action { get; private set; }
        
        public void Invoke()
        {
            Action?.Invoke();
        }

        public void AddListener(Action action)
        {
            Action += action;
        }
        
        public void RemoveListener(Action action)
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
        public Action<T> Action { get; private set; }
        
        public void Invoke(T obj)
        {
            Action?.Invoke(obj);
        }

        public void AddListener(Action<T> action)
        {
            Action += action;
        }
        
        public void RemoveListener(Action<T> action)
        {
            Action -= action;
        }


        public void ClearListeners()
        {
            Action = null;
        }
    }
}