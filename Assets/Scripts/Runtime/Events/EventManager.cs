using System;
using System.Reflection;
using UnityEngine;

namespace Runtime.Events
{
    public static class EventManager
    {
        public static Action OnLose;
        public static Action OnWin;
        public static Action<int> OnEnemyKilled; // money reward
        public static Action OnPause;
        public static Action OnEnd;

        public static void ResetActions()
        {
            foreach (var field in typeof(EventManager).GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                if (field.FieldType == typeof(Action) || field.FieldType.IsSubclassOf(typeof(Delegate)))
                {
                    field.SetValue(null, null);
                }
            }
        }
    }
}