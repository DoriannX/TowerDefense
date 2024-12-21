using System;
using UnityEngine;

namespace Runtime.Events
{
    public static class EventManager
    {
        public static Action OnLose;
        public static Action OnWin;
        public static Action<int> OnEnemyKilled; // money reward
    }
}