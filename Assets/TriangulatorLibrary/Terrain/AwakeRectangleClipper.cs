using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Vector2i = ClipperLib.IntPoint;

public class AwakeRectangleClipper : MonoBehaviour, IClip
{
    public DestructibleTerrain terrain;

    public float width = 2f;

    public float height = 1f;

    private Vector2 clipPosition;

    public bool CheckBlockOverlapping(Vector2 p, float size)
    {
        float dx = Mathf.Abs(clipPosition.x - p.x) - width / 2 - size / 2;
        float dy = Mathf.Abs(clipPosition.y - p.y) - height / 2 - size / 2;

        return dx < 0f && dy < 0f;
    }

    public ClipBounds GetBounds()
    {
        return new ClipBounds
        {
            lowerPoint = new Vector2(clipPosition.x - width / 2, clipPosition.y - height / 2),
            upperPoint = new Vector2(clipPosition.x + width / 2, clipPosition.y + height / 2)
        };
    }

    public List<Vector2i> GetVertices()
    {
        List<Vector2i> vertices = new List<Vector2i>();

        // Top left corner
        vertices.Add((clipPosition - new Vector2(width / 2, height / 2)).ToVector2i());

        // Top right corner
        vertices.Add((clipPosition + new Vector2(width / 2, -height / 2)).ToVector2i());

        // Bottom right corner
        vertices.Add((clipPosition + new Vector2(-width / 2, height / 2)).ToVector2i());

        // Bottom left corner
        vertices.Add((clipPosition + new Vector2(-width / 2, -height / 2)).ToVector2i());

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
            Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0));
        }
    }

    void Start()
    {
        clipPosition = transform.position - terrain.GetPositionOffset();

        terrain.ExecuteClip(this);
    }
}
