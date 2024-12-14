using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runtime.Enemy
{
    [ExecuteAlways]
    public class PathCreator : MonoBehaviour
    {
        [Header("Properties")]
        [SerializeField] private float _lineWidth = 0.1f;

        [SerializeField] private float _skinWidth = 0.01f;
        [SerializeField] private int _maxDepth = 10;
        [SerializeField] private int _maxIterations = 1000;
        [SerializeField] private float _tryingLength = 1;

        private List<Transform> _path;

        private List<Transform> GetPath
        {
            get
            {
                if (_path == null || _path.Count == 0 || _path.Any(t => t == null) ||
                    _path.Count != GetTransform.childCount - 1)
                {
                    _path = new List<Transform>();

                    if ((GetTransform.childCount <= 1 && GetLineRenderer != null) || GetTransform.childCount <= 0)
                    {
                        Debug.LogWarning("Path is empty in PathCreator");
                    }

                    List<Transform> children = GetTransform.GetComponentsInChildren<Transform>().ToList();
                    foreach (Transform child in children)
                    {
                        if (child == GetTransform || child == GetLineRenderer.transform)
                        {
                            continue;
                        }

                        _path.Add(child);
                    }
                }

                return _path;
            }
        }

        private List<Vector3> _previousPath = new();
        private LineRenderer _lineRenderer;
        private Transform _transform;

        private Transform GetTransform
        {
            get
            {
                if (_transform == null)
                {
                    _transform = transform;
                }

                return _transform;
            }
        }

        private LineRenderer GetLineRenderer
        {
            get
            {
                if (_lineRenderer == null)
                {
                    _lineRenderer = GetComponentInChildren<LineRenderer>();
                }

                return _lineRenderer;
            }
        }

        private void Update()
        {
            if (!PathChanged())
            {
                return;
            }

            _previousPath = GetPath.Select(t => t.position).ToList();
            ComputePositions();
        }

        private void AddPos(Vector3 pos)
        {
            GetLineRenderer.positionCount++;
            GetLineRenderer.SetPosition(GetLineRenderer.positionCount - 1, pos);
        }

        private bool PathChanged()
        {
            if (GetPath.Count != _previousPath.Count)
            {
                return true;
            }

            for (int i = 0; i < GetPath.Count; i++)
            {
                if (GetPath[i].position != _previousPath[i])
                {
                    return true;
                }
            }

            return false;
        }

        public void ComputePositions()
        {
            GetLineRenderer.positionCount = 0;
            if (GetPath.Count == 0)
            {
                return;
            }

            AddPos(GetPath[0].position);

            for (int i = 1; i < GetPath.Count; i++)
            {
                ConnectPoints(GetPath[i - 1].position, GetPath[i].position);
            }
        }

        public void ConnectPoints(Vector3 startPoint, Vector3 endPoint)
        {
            Vector3 currentPos = startPoint;
            Vector3 direction = endPoint - startPoint;
            int iterations = 0;

            while ((endPoint - currentPos).magnitude > 0.1f && iterations < _maxIterations)
            {
                Vector3 newPos = currentPos + DrawLineWithCollisions(direction, currentPos);
                if (newPos == Vector3.zero)
                {
                    break;
                }

                currentPos = newPos;
                direction = endPoint - currentPos;
                iterations++;
            }
        }

        public List<Vector3> GetPositions()
        {
            Vector3[] positions = new Vector3[GetLineRenderer.positionCount];
            GetLineRenderer.GetPositions(positions);
            return positions.ToList();
        }

        public Vector3 DrawLineWithCollisions(Vector3 displacement, Vector3 pos, int depth = 0)
        {
            //To stop the iteration if there's too much depth
            if (depth >= _maxDepth)
            {
                return Vector3.zero;
            }

            float dist = displacement.magnitude + _skinWidth;

            bool hitSomething =
                Physics.SphereCast(pos, _lineWidth, displacement.normalized, out RaycastHit hitInfo, dist);

            if (!hitSomething)
            {
                AddPos(pos + displacement);
                return displacement;
            }

            //To get the velocity needed to snap to the surface
            Vector3 snapToSurface =
                (hitInfo.distance - _skinWidth) * displacement.normalized;

            if (snapToSurface.magnitude <= _skinWidth)
            {
                snapToSurface = Vector3.zero;
            }

            AddPos(pos + snapToSurface);

            //To project the leftover velocity to the surface (slide)
            Vector3 leftover = displacement - snapToSurface;
            leftover = Vector3.ProjectOnPlane(leftover, hitInfo.normal).normalized * _tryingLength;

            //To iterate again in case there's more than one collision
            return snapToSurface + DrawLineWithCollisions(leftover, pos + snapToSurface, depth + 1);
        }
    }
}