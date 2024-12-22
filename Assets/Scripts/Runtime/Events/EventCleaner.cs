using System;
using UnityEngine;

namespace Runtime.Events
{
    public class EventCleaner : MonoBehaviour
    {
        private EventCleaner _instance;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            EventManager.ResetActions();
        }
    }
}