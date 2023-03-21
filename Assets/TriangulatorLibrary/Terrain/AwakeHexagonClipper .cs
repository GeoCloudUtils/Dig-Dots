using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Vector2i = ClipperLib.IntPoint;

public class AwakeHexagonClipper : MonoBehaviour, IClip
{
    public DestructibleTerrain terrain;

    public float diameter = 1.2f;

    public int segmentCount = 6;

    private Vector2 clipPosition;

    public bool CheckBlockOverlapping(Vector2 p, float size)
    {
        Vector2 offsetP = p - clipPosition;

        // distance from the center of the hexagon to the nearest point on the bounding rectangle
        float boundingRectHalfWidth = diameter / 2f;
        float boundingRectHalfHeight = boundingRectHalfWidth * Mathf.Sqrt(3f) / 2f;

        float dx = Mathf.Abs(offsetP.x) - boundingRectHalfWidth - size / 2;
        float dy = Mathf.Abs(offsetP.y) - boundingRectHalfHeight - size / 2;

        // check if the point is within the hexagon
        if (dx <= 0f && dy <= 0f)
        {
            float m = boundingRectHalfHeight / boundingRectHalfWidth;

            // check if the point is within the top or bottom triangle
            if (dy <= -m * dx + boundingRectHalfHeight && dy <= m * dx - boundingRectHalfHeight)
                return true;

            // check if the point is within the left or right triangle
            if (dx <= 0f && dy <= 2f * boundingRectHalfHeight)
                return true;

            if (dx >= 0f && dy <= -2f * boundingRectHalfHeight)
                return true;
        }

        return false;
    }

    public ClipBounds GetBounds()
    {
        return new ClipBounds
        {
            lowerPoint = new Vector2(clipPosition.x - diameter / 2f, clipPosition.y - diameter / 2f * Mathf.Sqrt(3f) / 2f),
            upperPoint = new Vector2(clipPosition.x + diameter / 2f, clipPosition.y + diameter / 2f * Mathf.Sqrt(3f) / 2f)
        };
    }

    public List<Vector2i> GetVertices()
    {
        List<Vector2i> vertices = new List<Vector2i>();
        for (int i = 0; i < segmentCount; i++)
        {
            float angle = Mathf.Deg2Rad * (60f * i);

            Vector2 point = new Vector2(clipPosition.x + diameter * Mathf.Cos(angle), clipPosition.y + diameter * Mathf.Sin(angle));
            Vector2i point_i64 = point.ToVector2i();
            vertices.Add(point_i64);
        }
        return vertices;
    }

    void Awake()
    {
        if (terrain == null)
            terrain = FindObjectOfType<DestructibleTerrain>();
    }

    void OnDrawGizmos()
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWirePolygon(GetVertices().ConvertAll(v => v.ToVector2()).ToArray());
        }
    }

    void Start()
    {
        Vector2 positionWorldSpace = transform.position;
        clipPosition = positionWorldSpace - terrain.GetPositionOffset();

        terrain.ExecuteClip(this);
    }
}
