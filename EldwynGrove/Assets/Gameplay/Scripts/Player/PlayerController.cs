using UnityEngine;
using UnityEngine.InputSystem;

namespace EldwynGrove
{
    public class PlayerController : MonoBehaviour, MyInputMapping.IPlayerActions
    {
        /* --- Component References --- */
        private Animator m_animator;
        private Rigidbody2D m_rigidbody2D;
        private MovementComponent m_movementComponent;
        private HealthComponent m_healthComponent;
        private CombatComponent m_combatComponent;
        private DashComponent m_dashComponent;

        private MyInputMapping m_inputMapping;

        private Vector2 m_direction;
        public Vector2 GetDirection() => m_direction;
        public void SetDirection(Vector2 direction) => m_direction = direction;

        /*----------------------------------------------------------------*/
        /*----------------------------
        | --- Method Definitions --- |
        ----------------------------*/

        /*----------------------------------------------------------------
        | --- Awake: Called when the script instance is being loaded --- |
        ----------------------------------------------------------------*/
        private void Awake()
        {
            m_animator = GetComponent<Animator>();
            Utilities.CheckForNull(m_animator, nameof(m_animator));

            m_rigidbody2D = GetComponent<Rigidbody2D>();
            Utilities.CheckForNull(m_rigidbody2D, nameof(m_rigidbody2D));

            m_movementComponent = GetComponent<MovementComponent>();
            Utilities.CheckForNull(m_movementComponent, nameof(m_movementComponent));

            m_healthComponent = GetComponent<HealthComponent>();
            Utilities.CheckForNull(m_healthComponent, nameof(m_healthComponent));

            m_combatComponent = GetComponent<CombatComponent>();
            Utilities.CheckForNull(m_combatComponent, nameof(m_combatComponent));

            m_dashComponent = GetComponent<DashComponent>();
            Utilities.CheckForNull(m_dashComponent, nameof(m_dashComponent));

            m_inputMapping = new MyInputMapping();
        }

        /*----------------------------------------------------------
        | --- OnEnable: Called when the object becomes Enabled --- |
        ----------------------------------------------------------*/
        private void OnEnable()
        {
            m_inputMapping.Player.SetCallbacks(this);
            m_inputMapping.Player.Enable();
        }

        /*------------------------------------------------------------
        | --- OnDisable: Called when the object becomes Disabled --- |
        ------------------------------------------------------------*/
        private void OnDisable()
        {
            m_inputMapping.Player.Disable();
        }

        /*----------------------------------------------------------
        | --- OnMove: Called when the Move Action is performed --- |
        ----------------------------------------------------------*/
        public void OnMove(InputAction.CallbackContext context)
        {
            if (m_healthComponent.IsDead) return;

            if (context.canceled)
            {
                m_movementComponent.Stop();
            }
            else
            {
                Vector2 movement = context.ReadValue<Vector2>();
                if (movement != Vector2.zero)
                {
                    m_direction = movement.normalized;
                }
                m_movementComponent.Move(movement);
            }
        }

        /*--------------------------------------------------------------
        | --- OnAttack: Called when the Attack Action is performed --- |
        --------------------------------------------------------------*/
        public void OnAttack(InputAction.CallbackContext context)
        {
            if (m_healthComponent.IsDead) return;

            if (context.performed)
            {
                m_combatComponent.Attack();
            }
        }

        /*----------------------------------------------------------
        | --- OnDash: Called when the Dash Action is performed --- |
        ----------------------------------------------------------*/
        public void OnDash(InputAction.CallbackContext context)
        {
            if (m_healthComponent.IsDead) return;

            if (context.performed)
            {
                m_dashComponent.Dash(m_direction);
            }
        }
    }
}