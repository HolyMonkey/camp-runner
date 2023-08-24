using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using System;
using UnityEditor;

namespace PathCreationTools
{
    public class PathObjectRedactor : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField, HideInInspector] private List<CachedObject> _cachedObjects = new List<CachedObject>();
        [SerializeField] private PathObject _template;
        [SerializeField] private Transform _container;
        [Space(10)]
        [SerializeField] private float _offset;
        [SerializeField] private float _height;
        [SerializeField] private float _distanceBetweenObjects;

        public int CachedCount => _cachedObjects.Count;
        public float DistanceBetweenObjects => _distanceBetweenObjects;
        public bool TemplateInitialized => _template != null;

        private void OnValidate()
        {
            if (_distanceBetweenObjects < 0)
                _distanceBetweenObjects = 0;
        }

        public void InstantiateObject(RaycastHit hitInfo)
        {
            if (_template == null)
                throw new NullReferenceException("Template is null");

            var nearestPath = hitInfo.point.FindNearestPath();
            if (nearestPath == null)
                throw new NullReferenceException("Can't find nearest PathCreator");

            var distance = nearestPath.path.GetClosestDistanceAlongPath(hitInfo.point);

            var pathObject = PrefabUtility.InstantiatePrefab(_template, _container) as PathObject;
            pathObject.Setup(nearestPath, distance, _offset, _height);
        }

        public void RemoveObject(RaycastHit hitInfo)
        {
            if (hitInfo.collider.TryGetComponent(out PathObject pathObject))
                DestroyImmediate(pathObject.gameObject);
        }

        public void Bake()
        {
            Clear();

            var pathCreatorList = FindObjectsOfType<PathCreator>();
            foreach (var pathCreator in pathCreatorList)
                CacheMesh(pathCreator);
        }

        public void Clear()
        {
            foreach (var cacheObject in _cachedObjects)
            {
                if (cacheObject.Collider)
                    DestroyImmediate(cacheObject.Collider.gameObject);
            }

            _cachedObjects.Clear();
        }

        public Vector3 GetNearestPathPoint(Vector3 position)
        {
            var nearestPath = position.FindNearestPath();
            if (nearestPath == null)
                throw new NullReferenceException("Can't find nearest PathCreator");

            return nearestPath.path.GetClosestPointOnPath(position);
        }

        private void CacheMesh(PathCreator pathCreator)
        {
            var vertices = new List<Vector3>();
            var triangles = new List<int>();

            for (int distance = 0; distance < pathCreator.path.length; distance++)
            {
                var point = pathCreator.path.GetPointAtDistance(distance);
                var normal = pathCreator.path.GetNormalAtDistance(distance);

                vertices.Add(point + normal * 3);
                vertices.Add(point - normal * 3);
            }

            for (int i = 0; i < vertices.Count - 2; i++)
            {
                triangles.Add(i);
                triangles.Add(i + 1);
                triangles.Add(i + 2);
                triangles.Add(i + 2);
                triangles.Add(i + 1);
                triangles.Add(i);
            }

            var mesh = new Mesh();
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.RecalculateBounds();

            var instance = new GameObject(pathCreator.name);
            instance.transform.parent = transform;
            instance.AddComponent<MeshFilter>().mesh = mesh;
            var collider = instance.AddComponent<MeshCollider>();

            _cachedObjects.Add(new CachedObject(pathCreator, collider));
        }

        [Serializable]
        public class CachedObject
        {
            [SerializeField] private PathCreator _pathCreator;
            [SerializeField] private Collider _collider;

            public CachedObject(PathCreator pathCreator, Collider collider)
            {
                _pathCreator = pathCreator;
                _collider = collider;
            }

            public PathCreator PathCreator => _pathCreator;
            public Collider Collider => _collider;
        }
#endif
    }
}