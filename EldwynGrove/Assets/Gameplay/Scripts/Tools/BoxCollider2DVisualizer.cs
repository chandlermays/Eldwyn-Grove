using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BoxCollider2DVisualizer : MonoBehaviour
{
    [SerializeField] private Color m_gizmoColor = Color.red;
    private BoxCollider2D m_boxCollider;

    private void Awake()
    {
        m_boxCollider = GetComponent<BoxCollider2D>();
        Utilities.CheckForNull(m_boxCollider, nameof(m_boxCollider));
    }

    private void OnDrawGizmos()
    {
        if (m_boxCollider == null)
        {
            m_boxCollider = GetComponent<BoxCollider2D>();
        }

        if (m_boxCollider != null)
        {
            Gizmos.color = m_gizmoColor;
            Vector2 size = m_boxCollider.size;
            Vector2 offset = m_boxCollider.offset;
            Vector2 position = (Vector2)transform.position + offset;

            Vector3 topLeft = new Vector3(position.x - size.x / 2, position.y + size.y / 2, 0);
            Vector3 topRight = new Vector3(position.x + size.x / 2, position.y + size.y / 2, 0);
            Vector3 bottomRight = new Vector3(position.x + size.x / 2, position.y - size.y / 2, 0);
            Vector3 bottomLeft = new Vector3(position.x - size.x / 2, position.y - size.y / 2, 0);

            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(topRight, bottomRight);
            Gizmos.DrawLine(bottomRight, bottomLeft);
            Gizmos.DrawLine(bottomLeft, topLeft);
        }
    }
}