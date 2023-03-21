using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Vector2i = ClipperLib.IntPoint;

public class AwakeTriangleClipper : MonoBehaviour, IClip
{
    public DestructibleTerrain terrain;

    public float baseLength = 2f;

    public float height = 2f;

    private Vector2 clipPosition;

    public bool CheckBlockOverlapping(Vector2 p, float size)
    {
        Vector2 offsetP = p - clipPosition;

        // half width of the bounding rectangle
        float boundingRectHalfWidth = baseLength / 2f;
        // distance from the center of the triangle to the bottom point of the bounding rectangle
        float boundingRectBottomHalfHeight = height / 2f;

        float dx = Mathf.Abs(offsetP.x) - boundingRectHalfWidth - size / 2;
        float dy = Mathf.Abs(offsetP.y) - boundingRectBottomHalfHeight - size / 2;

        // check if the point is within the triangle
        if (dx <= 0f && dy <= 0f)
        {
            float m = boundingRectBottomHalfHeight / boundingRectHalfWidth;

            // check if the point is above the bottom line
            if (dy <= -m * dx + boundingRectBottomHalfHeight)
                return true;
        }

        return false;
    }

    public ClipBounds GetBounds()
    {
        return new ClipBounds
        {
            lowerPoint = new Vector2(clipPosition.x - baseLength / 2f, clipPosition.y - height / 2f),
            upperPoint = new Vector2(clipPosition.x + baseLength / 2f, clipPosition.y + height / 2f)
        };
    }

    public List<Vector2i> GetVertices()
    {
        List<Vector2i> vertices = new List<Vector2i>();

        // bottom point of the triangle
        Vector2 point = new Vector2(clipPosition.x, clipPosition.y - height / 2f);
        Vector2i point_i64 = point.ToVector2i();
        vertices.Add(point_i64);

        // left point of the triangle
        point = new Vector2(clipPosition.x - baseLength / 2f, clipPosition.y + height / 2f);
        point_i64 = point.ToVector2i();
        vertices.Add(point_i64);

        // right point of the triangle
        point = new Vector2(clipPosition.x + baseLength / 2f, clipPosition.y + height / 2f);
        point_i64 = point.ToVector2i();
        vertices.Add(point_i64);

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
