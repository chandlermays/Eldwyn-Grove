using System.Collections;
using UnityEngine;

namespace EldwynGrove
{
    public class DashComponent : EntityComponent
    {
        [SerializeField] private float m_dashSpeed = 25f;
        [SerializeField] private float m_dashDuration = 0.2f;
        [SerializeField] private float m_dashCooldown = 1.0f;

        private bool m_isDashing = false;
        private bool m_isOnCooldown = false;
        public bool CanDash => !m_isDashing && !m_isOnCooldown;

        private MovementComponent m_movementComponent;

        /*----------------------------------------------------------------*/
        /*----------------------------
        | --- Method Definitions --- |
        ----------------------------*/

        /*----------------------------------------------------------------
        | --- Awake: Called when the script instance is being loaded --- |
        ----------------------------------------------------------------*/
        protected override void Awake()
        {
            base.Awake();
            m_movementComponent = GetComponent<MovementComponent>();
        }

        /*----------------------------------------------------------
        | --- Dash: Initiate a dash in the specified direction --- |
        ----------------------------------------------------------*/
        public void Dash(Vector2 direction)
        {
            if (CanDash)
            {
                StartCoroutine(DashCoroutine(direction));
            }
        }

        /*--------------------------------------------------------------
        | --- DashCoroutine: Coroutine to handle the dash movement --- |
        --------------------------------------------------------------*/
        private IEnumerator DashCoroutine(Vector2 direction)
        {
            m_isDashing = true;
            m_isOnCooldown = true;
            float originalSpeed = m_movementComponent.MoveSpeed;
            m_movementComponent.SetMoveSpeed(m_dashSpeed);
            m_movementComponent.Move(direction);

            yield return new WaitForSeconds(m_dashDuration);

            m_movementComponent.SetMoveSpeed(originalSpeed);
            m_isDashing = false;

            yield return new WaitForSeconds(m_dashCooldown);
            m_isOnCooldown = false;
        }
    }
}