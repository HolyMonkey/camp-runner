using UnityEngine;
using PathCreation;

namespace PathCreationTools
{
    internal static class Extentions
    {
        public static PathCreator FindNearestPath(this Vector3 position)
        {
            var allPaths = Object.FindObjectsOfType<PathCreator>();

            float minDistance = float.MaxValue;
            PathCreator nearestPath = null;

            foreach (var pathCreator in allPaths)
            {
                var nearestPoint = pathCreator.path.GetClosestPointOnPath(position);
                var distance = Vector3.Distance(position, nearestPoint);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestPath = pathCreator;
                }
            }

            return nearestPath;
        }

        public static PathCreator FindNearestPath(this Transform transform)
        {
            return transform.position.FindNearestPath();
        }
    }
}