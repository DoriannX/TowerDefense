#region

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

#endregion

namespace _00_Scripts.Runtime
{
    public class UiClickedObjectDetector : MonoBehaviour
    {
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.current;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                DetectClickedObject();
            }
        }

        private void DetectClickedObject()
        {
            PointerEventData pointerEventData = new(EventSystem.current)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> results = new();
            EventSystem.current.RaycastAll(pointerEventData, results);

            foreach (RaycastResult result in results)
            {
                Debug.Log("Clicked on: " + result.gameObject.name);
            }
        }
    }
}