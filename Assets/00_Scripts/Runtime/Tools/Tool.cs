#region

using Runtime.Events;
using UnityEditor;

#endregion

namespace _00_Scripts.Runtime.Tools
{
    public abstract class Tool
    {
        [MenuItem("Tools/Add Money")]
        public static void AddMoney()
        {
            EventManager.OnMoneyAdded?.Invoke(9999);
        }
    }
}