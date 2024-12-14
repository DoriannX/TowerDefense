using System.Collections.Generic;
using System.Linq;
using Runtime.CharacterController;
using Runtime.Turrets;
using UnityEngine;
using UnityEngine.Assertions;

namespace Runtime.Player
{
    public class TurretPlacer : MonoBehaviour
    {
        [Header("Prefabs")] [SerializeField] private List<SerializedTuple<GameObject, GameObject>> _turretPrefabs;

        [Header("Properties")] [SerializeField]
        private float _placeDistance;

        [SerializeField] private LayerMask _obstacleLayer;

        [Header("Components")] [SerializeField]
        private Transform _head;

        //Properties
        private bool _placingMode;
        private readonly List<BaseTurret> _previews = new();
        private int _turretIndex = 0;
        private bool _placeable;
        private Vector3 _placePos;
        private bool _placePhase = true;

        //Components
        private Id _id;
        private Transform _transform;
        private MoneyManager _moneyManager;

        private void Awake()
        {
            _id = GetComponent<Id>();
            _transform = transform;
            _moneyManager = GetComponent<MoneyManager>();
        }

        private void Start()
        {
            Assert.IsFalse(_turretPrefabs.Count == 0, "There is no turret prefabs in TurretPlacer");
            
            global::GameEvents.OnToggleMode?.AddListener(ToggleMode);
            global::GameEvents.OnShootStarted?.AddListener(TryPlaceTurret);
            global::GameEvents.OnTogglePhase?.AddListener(TogglePhase);

            foreach (SerializedTuple<GameObject, GameObject> preview in _turretPrefabs)
            {
                GameObject instantiatedPreview = Instantiate(preview.V1);
                instantiatedPreview.gameObject.SetActive(false);
                _previews.Add(instantiatedPreview.GetComponent<BaseTurret>());
            }
        }

        private void TogglePhase(params int[] ids)
        {
            if (!ids.Contains(_id.GetId()))
            {
                return;
            }

            Debug.Log(_placePhase);

            _placePhase = !_placePhase;
        }

        private void Update()
        {
            PreviewTurret();
        }

        private void ToggleMode(int[] ids)
        {
            if (!ids.Contains(_id.GetId()))
            {
                return;
            }

            foreach (BaseTurret preview in _previews)
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
                   _moneyManager.TryGetMoney(_previews[_turretIndex].Cost);
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
            Instantiate(_turretPrefabs[_turretIndex].V2, _placePos, Quaternion.identity).GetComponent<BaseTurret>()
                .Toggle();
        }

        private void PreviewTurret()
        {
            if (!_placingMode)
            {
                return;
            }

            Vector3 direction = CollideAndSlide(_head.forward * _placeDistance, _transform.rotation,
                _transform.position,
                _previews[_turretIndex].Collider.bounds.extents);

            Vector3 placingPos = _transform.position + direction;
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