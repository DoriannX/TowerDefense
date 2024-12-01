using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Runtime.GameEvents
{
    public class GameEventsConstructor : AssetPostprocessor
    {
        private static readonly string CLASS_FILE_PATH =
            Path.Combine(Application.dataPath, "Scripts/Runtime/GameEvents/GameEvents.cs");

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            GenerateGameEvents();
        }

        [MenuItem("Tools/Generate GameEvents Class")]
        private static void GenerateGameEvents()
        {
            Addressables.LoadAssetsAsync<IGameEvent>("GameEvents").Completed += OnEventsLoaded;
        }

        private static void OnEventsLoaded(AsyncOperationHandle<IList<IGameEvent>> handle)
        {
            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                return;
            }

            List<IGameEvent> allGameEvents = handle.Result.ToList();

            // Générer le code
            string classCode =
                "using UnityEngine;\nusing UnityEngine.AddressableAssets;\nusing UnityEngine.ResourceManagement.AsyncOperations;\nusing System.Collections.Generic;\nusing Runtime.GameEvents;\n\n";
            classCode += "public static class GameEvents\n{\n";
            classCode +=
                "    private static Dictionary<string, IGameEvent> _events = new Dictionary<string, IGameEvent>();\n\n";

            foreach (IGameEvent gameEvent in allGameEvents)
            {
                classCode +=
                    $@"    public static {gameEvent.GetType().Name} {gameEvent} => GetEvent<{gameEvent.GetType().Name}>(""{gameEvent}"");
";
            }

            classCode += @"
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
            Debug.LogError($""Failed to load GameEvent: {eventName}"");
            return default;
        }
    }
}";

            // Écrire dans le fichier
            File.WriteAllText(CLASS_FILE_PATH, classCode);
            AssetDatabase.Refresh();
            Debug.Log("GameEvents class generated successfully!");
        }
    }
}