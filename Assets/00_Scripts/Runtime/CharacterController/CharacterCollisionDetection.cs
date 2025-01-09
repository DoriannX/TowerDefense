using System;
using CharacterDebugger;
using SerializedProperties;
using UnityEngine;

namespace Runtime.CharacterController
{
    public class CharacterCollisionDetection
    {
        //Properties
        private readonly CharacterBodyProperties _characterBodyProperties;
        private readonly CharacterCollisionDetectionProperties _characterCollisionDetectionProperties;

        private readonly Collider[] _colliders = new Collider[10];

        //Components
        private Bounds _bounds;


        public CharacterCollisionDetection(ref CharacterBodyProperties characterBodyProperties,
            ref CharacterCollisionDetectionProperties characterCollisionDetectionProperties)
        {
            _characterBodyProperties = characterBodyProperties;
            _characterCollisionDetectionProperties = characterCollisionDetectionProperties;
        }

        public void Update()
        {
            //To recalculate the bounds each time the player moves
            _bounds = new Bounds(_characterBodyProperties.Transform.position,
                _characterBodyProperties.Size);
            _bounds.Expand(-2 * _characterBodyProperties.SkinWidth);
        }

        public Vector3 CollideAndSlide(Vector3 vel, Vector3 pos, bool gravityPass, int depth)
        {
            //To stop the iteration if there's too much depth
            if (depth >= _characterCollisionDetectionProperties.MaxCollisionDepth)
            {
                return Vector3.zero;
            }

            float dist = vel.magnitude + _characterBodyProperties.SkinWidth;

            bool hitSomething = Physics.SphereCast(pos, _bounds.extents.x, vel.normalized, out RaycastHit hitInfo, dist,
                _characterCollisionDetectionProperties.ObstacleLayer);

            if (!hitSomething || hitInfo.collider.isTrigger)
            {
                return vel;
            }

            //To get the velocity needed to snap to the surface
            Vector3 snapToSurface =
                (hitInfo.distance - _characterBodyProperties.SkinWidth) * vel.normalized;

            Vector3 leftover = vel - snapToSurface;

            if (snapToSurface.magnitude <= _characterBodyProperties.SkinWidth)
            {
                snapToSurface = Vector3.zero;
            }

            float angle = Vector3.Angle(Vector3.up, hitInfo.normal);

            float scale = 1;

            if (angle <= _characterCollisionDetectionProperties.MaxSlopeAngle)
            {
                if (gravityPass)
                {
                    //To prevent the player from sliding down slopes with a certain angle
                    return snapToSurface;
                }
            }
            else
            {
                //So that the more the player's velocity is angled in relation to the wall,
                //the faster he will slide
                Vector3 flatBaseVel = vel;
                Vector3 flatNormal = hitInfo.normal;
                flatBaseVel.y = 0;
                flatNormal.y = 0;
                scale = 1 - Vector3.Dot(flatNormal.normalized, -flatBaseVel.normalized);
                scale = Mathf.Clamp01(scale);
            }

            //To project the leftover velocity to the surface (slide)
            float mag = leftover.magnitude;
            leftover = Vector3.ProjectOnPlane(leftover, hitInfo.normal).normalized;
            leftover *= mag * scale;

            //To iterate again in case there's more than one collision
            return snapToSurface + CollideAndSlide(leftover, pos + snapToSurface, gravityPass, depth + 1);
        }

        public Vector3 TrySlideDownSlope(Vector3 currentMove, bool isGrounded)
        {
            if (!isGrounded)
            {
                return currentMove;
            }

            //Sphere cast to get the ground
            Physics.SphereCast(_characterBodyProperties.Transform.position,
                _characterBodyProperties.Extents.z - _characterBodyProperties.SkinWidth,
                _characterBodyProperties.Size.x / 2 * Vector3.down,
                out RaycastHit hitInfo);

            DebugShapeCasts.DebugDrawSphere(hitInfo.point, _characterBodyProperties.Size.x / 2, Color.red);
            if (hitInfo.collider is not null && !Mathf.Approximately(Vector3.Dot(Vector3.up, hitInfo.normal), 1))
            {
                //Project the input to the slope
                currentMove = Vector3.ProjectOnPlane(currentMove, hitInfo.normal).normalized *
                              currentMove.magnitude;
            }

            return currentMove;
        }

        public Vector3 GetPushForce()
        {
            Array.Clear(_colliders, 0, _colliders.Length);
            
            //To detect if objects inside the character
            int hitCount = Physics.OverlapSphereNonAlloc(
                _characterBodyProperties.Transform.position,
                _characterBodyProperties.Size.x / 2 - _characterBodyProperties.SkinWidth, _colliders,
                _characterCollisionDetectionProperties.ObstacleLayer);
            
            bool hitSomething = hitCount > 0;
            if (!hitSomething)
            {
                return Vector3.zero;
            }

            Vector3 pushForce = Vector3.zero;
            foreach (Collider collider in _colliders)
            {
                if (collider is null)
                {
                    continue;
                }
                //To add to the push force the necessary force to get out of the overlapped object
                Vector3 closestPoint = Physics.ClosestPoint(_characterBodyProperties.Transform.position, collider,
                    collider.transform.position, collider.transform.rotation);
                Vector3 direction = (_characterBodyProperties.Transform.position - closestPoint).normalized;
                
                //Works only with spheres and not capsules
                pushForce += direction * (_characterBodyProperties.Size.x / 2 - _characterBodyProperties.SkinWidth);
            }


            return pushForce;
        }
    }
}