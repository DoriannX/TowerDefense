#region

using System.Collections.Generic;
using System.Linq;
using _00_Scripts.Runtime.Turrets;
using Runtime;
using Runtime.CharacterController;
using Runtime.Events;
using Runtime.Player;
using UnityEngine;
using UnityEngine.Assertions;

#endregion

namespace _00_Scripts.Runtime.Player
{
    public class TurretPlacer : MonoBehaviour
    {
        [Header("Prefabs")] [SerializeField] private List<SerializedTuple<GameObject, GameObject>> _turretPrefabs;

        [Header("Properties")] [SerializeField]
        private float _placeDistance;

        [SerializeField] private LayerMask _obstacleLayer;

        [Header("Components")] [SerializeField]
        private Transform _head;

        private readonly List<GameObject> _previews = new();
        private readonly int _turretIndex = 0;
        private readonly List<BaseTurret> _turrets = new();

        //Components
        private Id _id;
        private MoneyManager _moneyManager;
        private bool _placeable;
        private bool _placePhase = true;
        private Vector3 _placePos;

        //Properties
        private bool _placingMode;

        private void Awake()
        {
            _id = transform.parent.GetComponentInChildren<Id>();
            _moneyManager = transform.parent.GetComponentInChildren<MoneyManager>();
            Assert.IsFalse(_turretPrefabs.Count == 0, "There is no turret prefabs in TurretPlacer");
            foreach (SerializedTuple<GameObject, GameObject> turretPrefab in _turretPrefabs)
            {
                Assert.IsNotNull(turretPrefab.V1, "Missing reference: turretPrefab.V1");
                Assert.IsNotNull(turretPrefab.V2, "Missing reference: turretPrefab.V2");
                GameObject instantiatedPreview = Instantiate(turretPrefab.V1);
                instantiatedPreview.gameObject.SetActive(false);
                _previews.Add(instantiatedPreview);
                _turrets.Add(turretPrefab.V2.GetComponentInChildren<BaseTurret>());
            }
        }

        private void Start()
        {
            global::GameEvents.OnToggleMode?.AddListener(ToggleMode);
            global::GameEvents.OnShootStarted?.AddListener(TryPlaceTurret);
            EventManager.OnTogglePhase += TogglePhase;
        }

        private void Update()
        {
            PreviewTurret();
        }

        private void TogglePhase()
        {
            _placePhase = !_placePhase;
        }

        private void ToggleMode(int[] ids)
        {
            if (!ids.Contains(_id.GetId()))
            {
                return;
            }

            foreach (GameObject preview in _previews)
            {
                if (preview.gameObject.activeSelf)
                {
                    preview.gameObject.SetActive(false);
                }
            }

            _placingMode = !_placingMode;
        }

        private bool CanPlaceTurret()
        {
            return _placingMode && _placeable && _placePhase &&
                   _moneyManager.TryGetMoney(_turrets[_turretIndex].Cost);
        }

        private void TryPlaceTurret(params int[] ids)
        {
            if (!ids.Contains(_id.GetId()) || !CanPlaceTurret())
            {
                return;
            }

            PlaceTurret();
        }

        private void PlaceTurret()
        {
            Instantiate(_turretPrefabs[_turretIndex].V2, _placePos, Quaternion.identity)
                .GetComponentInChildren<BaseTurret>()
                .Toggle();
        }

        private void PreviewTurret()
        {
            if (!_placingMode)
            {
                return;
            }

            Vector3 direction = CollideAndSlide(_head.forward * _placeDistance, _head.rotation,
                _head.position,
                _turrets[_turretIndex].Collider.bounds.extents);

            Vector3 placingPos = _head.position + direction;
            _placePos = placingPos;
            _previews[_turretIndex].transform.position = placingPos;

            if (!_previews[_turretIndex].gameObject.activeSelf)
            {
                _previews[_turretIndex].gameObject.SetActive(true);
            }
        }

        private Vector3 CollideAndSlide(Vector3 vel, Quaternion rot, Vector3 pos, Vector3 extents)
        {
            float dist = vel.magnitude;

            bool hitSomething = Physics.BoxCast(pos, extents, vel.normalized, out RaycastHit hitInfo, rot, dist,
                _obstacleLayer);

            if (!hitSomething)
            {
                _placeable = false;
                return vel;
            }

            _placeable = true;

            //To get the velocity needed to snap to the surface
            Vector3 snapToSurface = hitInfo.distance * vel.normalized;

            //To iterate again in case there's more than one collision
            return snapToSurface;
        }
    }
}