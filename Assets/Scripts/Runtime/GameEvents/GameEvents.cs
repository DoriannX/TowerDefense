using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;
using Runtime.GameEvents;

public static class GameEvents
{
    private static Dictionary<string, IGameEvent> _events = new Dictionary<string, IGameEvent>();

    public static GameEventId OnShootStarted => GetEvent<GameEventId>("OnShootStarted");
    public static GameEventId OnSprintStarted => GetEvent<GameEventId>("OnSprintStarted");
    public static GameEventId OnJumpCanceled => GetEvent<GameEventId>("OnJumpCanceled");
    public static GameEventId OnJumpStarted => GetEvent<GameEventId>("OnJumpStarted");
    public static Vector3GameEventId OnMovePerformed => GetEvent<Vector3GameEventId>("OnMovePerformed");
    public static Vector2GameEventId OnLookPerformed => GetEvent<Vector2GameEventId>("OnLookPerformed");
    public static GameEventId OnCreated => GetEvent<GameEventId>("OnCreated");
    public static GameEventId OnPossess => GetEvent<GameEventId>("OnPossess");
    public static GameEventId OnDead => GetEvent<GameEventId>("OnDead");
    public static Vector2GameEventId OnLookCanceled => GetEvent<Vector2GameEventId>("OnLookCanceled");
    public static GameEventId OnSprintCanceled => GetEvent<GameEventId>("OnSprintCanceled");
    public static GameEventId OnShootCanceled => GetEvent<GameEventId>("OnShootCanceled");
    public static GameObjectGameEvent OnHit => GetEvent<GameObjectGameEvent>("OnHit");
    public static GameEvent OnRevive => GetEvent<GameEvent>("OnRevive");
    public static Vector3GameEventId OnMoveCanceled => GetEvent<Vector3GameEventId>("OnMoveCanceled");

    private static T GetEvent<T>(string eventName) where T : IGameEvent
    {
        if (_events.TryGetValue(eventName, out IGameEvent gameEvent))
        {
            return (T)gameEvent;
        }

        AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(eventName);
        handle.WaitForCompletion();
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            gameEvent = handle.Result;
            _events[eventName] = gameEvent;
            return (T)gameEvent;
        }
        else
        {
            Debug.LogError($"Failed to load GameEvent: {eventName}");
            return default;
        }
    }
}