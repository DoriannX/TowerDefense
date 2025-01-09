#region

using System.Linq;
using Runtime.CharacterController;
using SerializedProperties;
using UnityEngine;
using UnityEngine.Assertions;

#endregion

namespace _Scripts.Runtime.CharacterController
{
    public class CustomCharacterController : MovableObject
    {
        //Serialized properties
        [SerializeField] private Transform _rootParent;
        [SerializeField] private CharacterBodyProperties _characterBodyProperties;
        [SerializeField] private CharacterCollisionDetectionProperties _characterCollisionDetectionProperties;
        [SerializeField] private CharacterMovementProperties _characterMovementProperties;
        [SerializeField] private CharacterGroundProperties _characterGroundProperties;
        private CharacterCollisionDetection _characterCollisionDetection;
        private CharacterGroundChecker _characterGroundChecker;

        //Components
        private CharacterMovement _characterMovement;
        private Id _id;
        private Transform _transform;

        private void Awake()
        {
            Assert.IsNotNull(_rootParent, "Root parent is null");
            InitComponents();
            InitObjects();
        }

        private void Start()
        {
            InitEvents();
            GameEvents.OnCreated?.Invoke(_id.GetId());
        }

        private void Update()
        {
            _characterCollisionDetection.Update();

            //To calculate collisions, slopes and overlapping push forces and move
            Vector3 collidedGravity = CalculateCollidedGravity();
            Vector3 collidedMovement =
                CalculateCollidedMovement(_characterMovement.CalculateDeltaMoveAmount(), collidedGravity);
            Vector3 collapsePushForce = _characterCollisionDetection.CollideAndSlide(
                _characterCollisionDetection.GetPushForce(),
                _characterBodyProperties.Transform.position + collidedMovement * Time.deltaTime +
                collidedGravity * Time.deltaTime, false, 0);
            _characterMovement.MoveAndApplyVel(collidedGravity, collidedMovement, collapsePushForce);
        }

        private void InitComponents()
        {
            //To store the components in variables
            _transform = transform;
            _id = _transform.parent.GetComponentInChildren<Id>();
            _characterBodyProperties.SetTransform(_rootParent);
        }

        private void InitObjects()
        {
            //To initialize different used classes
            _characterMovement = new CharacterMovement(ref _characterBodyProperties, ref _characterMovementProperties);
            _characterCollisionDetection = new CharacterCollisionDetection(ref _characterBodyProperties,
                ref _characterCollisionDetectionProperties);
            _characterGroundChecker =
                new CharacterGroundChecker(ref _characterBodyProperties, ref _characterGroundProperties);
        }

        private void InitEvents()
        {
            GameEvents.OnMovePerformed.AddListener(TrySetMoveAmount);
            GameEvents.OnMoveCanceled.AddListener(TrySetMoveAmount);
            GameEvents.OnJumpStarted.AddListener(TryStartJump);
            GameEvents.OnJumpCanceled.AddListener(TryStopJump);
            GameEvents.OnSprintStarted.AddListener(TryStartSprint);
            GameEvents.OnSprintCanceled.AddListener(TryStopSprint);
        }

        private void TrySetMoveAmount(Vector3 moveAmount, params int[] ids)
        {
            if (!ids.Contains(_id.GetId()))
            {
                return;
            }

            _characterMovement.SetMoveAmount(moveAmount);
        }


        private void TryStartSprint(params int[] ids)
        {
            if (!ids.Contains(_id.GetId()))
            {
                return;
            }

            _characterMovement.StartSprint();
        }


        private void TryStopSprint(params int[] ids)
        {
            if (!ids.Contains(_id.GetId()))
            {
                return;
            }

            _characterMovement.StopSprint();
        }


        private void TryStartJump(params int[] ids)
        {
            if (!ids.Contains(_id.GetId()) || !_characterGroundChecker.IsGrounded())
            {
                return;
            }

            _characterMovement.StartJump();
        }


        private void TryStopJump(params int[] ids)
        {
            if (!ids.Contains(_id.GetId()))
            {
                return;
            }

            _characterMovement.StopJump();
        }

        public override Vector3 GetVelocity()
        {
            return transform.InverseTransformDirection(_characterMovement.Velocity);
        }

        public override Quaternion GetDeltaRotation()
        {
            return Quaternion.identity;
        }

        private Vector3 CalculateCollidedMovement(Vector3 deltaMovement, Vector3 collidedGravity)
        {
            //To calculate collisions
            Vector3 collidedMovement = Vector3.zero;
            if (Time.deltaTime > 0)
            {
                collidedMovement = _characterCollisionDetection.CollideAndSlide(deltaMovement,
                                       _characterBodyProperties.Transform.position + collidedGravity * Time.deltaTime,
                                       false, 0) /
                                   Time.deltaTime;
            }

            //To calculate slopes
            Vector3 downSlopeMovement =
                _characterCollisionDetection.TrySlideDownSlope(collidedMovement, _characterGroundChecker.IsGrounded());

            //To then calculate again collisions
            if (Time.deltaTime > 0)
            {
                collidedMovement = _characterCollisionDetection.CollideAndSlide(downSlopeMovement * Time.deltaTime,
                                       _characterBodyProperties.Transform.position + collidedGravity * Time.deltaTime,
                                       false, 0) /
                                   Time.deltaTime;
            }

            return collidedMovement;
        }

        private Vector3 CalculateCollidedGravity()
        {
            //To get the current gravity based on the velocity
            Vector3 currentGravity = CharacterGravityHandler.CalculateCurrentGravity(_characterMovement.Velocity) +
                                     CharacterGravityHandler.HandleGravity() *
                                     (_characterBodyProperties.Mass * Time.deltaTime);

            //To calculate collisions
            Vector3 collidedGravity = Vector3.zero;
            if (Time.deltaTime > 0)
            {
                collidedGravity = _characterCollisionDetection.CollideAndSlide(currentGravity * Time.deltaTime,
                    _characterBodyProperties.Transform.position, true,
                    0) / Time.deltaTime;
            }

            return collidedGravity;
        }
    }
}