using UnityEngine;

namespace CharacterDebugger
{
    public static class DebugShapeCasts
    {
        
        public static void DebugDrawSphere(Vector3 position, float radius, Color color, float duration = 0f, bool depthTest = true)
        {
            const int segments = 40;
            const float step = 360f / segments;

            for (int i = 0; i < segments; i++)
            {
                float angle = i * step;
                float nextAngle = (i + 1) * step;

                // Draw circle on XZ plane
                Vector3 from = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * radius, 0, Mathf.Sin(angle * Mathf.Deg2Rad) * radius) + position;
                Vector3 to = new Vector3(Mathf.Cos(nextAngle * Mathf.Deg2Rad) * radius, 0, Mathf.Sin(nextAngle * Mathf.Deg2Rad) * radius) + position;
                Debug.DrawLine(from, to, color, duration, depthTest);

                // Draw circle on XY plane
                from = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * radius, Mathf.Sin(angle * Mathf.Deg2Rad) * radius, 0) + position;
                to = new Vector3(Mathf.Cos(nextAngle * Mathf.Deg2Rad) * radius, Mathf.Sin(nextAngle * Mathf.Deg2Rad) * radius, 0) + position;
                Debug.DrawLine(from, to, color, duration, depthTest);

                // Draw circle on YZ plane
                from = new Vector3(0, Mathf.Cos(angle * Mathf.Deg2Rad) * radius, Mathf.Sin(angle * Mathf.Deg2Rad) * radius) + position;
                to = new Vector3(0, Mathf.Cos(nextAngle * Mathf.Deg2Rad) * radius, Mathf.Sin(nextAngle * Mathf.Deg2Rad) * radius) + position;
                Debug.DrawLine(from, to, color, duration, depthTest);
            }
        }
        
        public static void DebugDrawCylinder(Vector3 position, float radius, float height, Color color, float duration = 0f, bool depthTest = true)
        {
            const int segments = 16;
            const float step = 360f / segments;

            for (int i = 0; i < segments; i++)
            {
                float angle = i * step;
                float nextAngle = (i + 1) * step;

                // Draw top circle
                Vector3 from = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * radius, height / 2, Mathf.Sin(angle * Mathf.Deg2Rad) * radius) + position;
                Vector3 to = new Vector3(Mathf.Cos(nextAngle * Mathf.Deg2Rad) * radius, height / 2, Mathf.Sin(nextAngle * Mathf.Deg2Rad) * radius) + position;
                Debug.DrawLine(from, to, color, duration, depthTest);

                // Draw bottom circle
                from = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * radius, -height / 2, Mathf.Sin(angle * Mathf.Deg2Rad) * radius) + position;
                to = new Vector3(Mathf.Cos(nextAngle * Mathf.Deg2Rad) * radius, -height / 2, Mathf.Sin(nextAngle * Mathf.Deg2Rad) * radius) + position;
                Debug.DrawLine(from, to, color, duration, depthTest);

                // Connect top and bottom circles
                from = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * radius, height / 2, Mathf.Sin(angle * Mathf.Deg2Rad) * radius) + position;
                to = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * radius, -height / 2, Mathf.Sin(angle * Mathf.Deg2Rad) * radius) + position;
                Debug.DrawLine(from, to, color, duration, depthTest);
            }
        }
        
        public static void DebugDrawCapsule(Vector3 position, float radius, float height, Color color, float duration = 0f, bool depthTest = true)
        {
            // Draw the cylinder part
            DebugDrawCylinder(position, radius, height - 2 * radius, color, duration, depthTest);

            // Draw the top hemisphere
            Vector3 topSpherePosition = position + Vector3.up * (height / 2 - radius);
            DebugDrawSphere(topSpherePosition, radius, color, duration, depthTest);

            // Draw the bottom hemisphere
            Vector3 bottomSpherePosition = position - Vector3.up * (height / 2 - radius);
            DebugDrawSphere(bottomSpherePosition, radius, color, duration, depthTest);
        }
    }
}