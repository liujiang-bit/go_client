using UnityEngine;

namespace Client.Utils
{
    public class MathExtension
    {
        public static bool IsInsideRange( int source, int min, int max)
        {
            return source >= min && source <= max;
        }

        public static bool GetInsideRange( float source, float min, float max)
        {
            return (double) source >= (double) min && (double) source <= (double) max;
        }

        public static Vector3 NearestVertexTo( MeshFilter meshFilter, Vector3 point)
        {
            return MathExtension.NearestVertexTo(((Component) meshFilter).transform, meshFilter.mesh, point);
        }

        public static Vector3 NearestVertexTo( MeshCollider collider, Vector3 point)
        {
            return MathExtension.NearestVertexTo(((Component) collider).transform, collider.sharedMesh, point);
        }

        private static Mesh s_mesh;
        private static Vector3[] s_vertices;

        public static Vector3 NearestVertexTo(Transform transform, Mesh mesh, Vector3 point)
        {
            point = transform.InverseTransformPoint(point);
            float minDistanceSqr = float.PositiveInfinity;
            Vector3 nearestVertex = Vector3.zero;

            if (s_mesh==null)
            {
                s_mesh = mesh;
                s_vertices = s_mesh.vertices;
            }
            
            foreach (Vector3 vertex in s_vertices)
            {
                float sqrMagnitude = (point - vertex).sqrMagnitude;
                if (sqrMagnitude <  minDistanceSqr)
                {
                    minDistanceSqr = sqrMagnitude;
                    nearestVertex = vertex;
                }
            }
            return transform.TransformPoint(nearestVertex);
        }

        public static void ClearMesh()
        {
            s_mesh = null;
            s_vertices = null;
        }
    }
}