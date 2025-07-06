using UnityEngine;

namespace EldwynGrove
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] private Transform m_target;
        [SerializeField] private BoxCollider2D m_cameraBounds;
        private Camera m_camera;

        private void Awake()
        {
            m_camera = Camera.main; // Cache the main camera
        }

        private void LateUpdate()
        {
            if (m_target == null || m_cameraBounds == null || m_camera == null) return;

            // Get the target position
            Vector3 targetPosition = m_target.position;

            // Get the bounds of the BoxCollider2D
            Bounds bounds = m_cameraBounds.bounds;

            // Calculate the camera's half dimensions in world space
            float cameraHalfHeight = m_camera.orthographicSize;
            float cameraHalfWidth = cameraHalfHeight * m_camera.aspect;

            // Clamp the target position within the bounds, accounting for the camera's size
            float clampedX = Mathf.Clamp(targetPosition.x, bounds.min.x + cameraHalfWidth, bounds.max.x - cameraHalfWidth);
            float clampedY = Mathf.Clamp(targetPosition.y, bounds.min.y + cameraHalfHeight, bounds.max.y - cameraHalfHeight);

            // Set the camera position
            transform.position = new Vector3(clampedX, clampedY, transform.position.z);
        }
    }
}