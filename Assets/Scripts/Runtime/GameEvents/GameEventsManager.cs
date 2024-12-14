using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Runtime.GameEvents
{
    public class GameEventsManager : MonoBehaviour
    {
        private List<IGameEvent> _allGameEvents;
        
        private GameEventsManager _instance;
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(this);
            }
            
            LoadGameEvents();
        }

        private void LoadGameEvents()
        {
            Addressables.LoadAssetsAsync<IGameEvent>("GameEvents").Completed += OnEventsLoaded;
        }

        private void OnEventsLoaded(AsyncOperationHandle<IList<IGameEvent>> handle)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                _allGameEvents = handle.Result.ToList();
            }
        }

        private void ClearAllListeners()
        {
            foreach (IGameEvent gameEvent in _allGameEvents)
            {
                gameEvent.ClearListeners();
            }
        }

        private void OnDestroy()
        {
            ClearAllListeners();
        }
    }
}