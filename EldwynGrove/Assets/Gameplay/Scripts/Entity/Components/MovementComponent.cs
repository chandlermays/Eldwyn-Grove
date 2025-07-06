using UnityEngine;
//---------------------------------
using EldwynGrove.Saving;

namespace EldwynGrove
{
    public class MovementComponent : EntityComponent, ISaveable
    {
        [Header("Movement Settings")]
        [SerializeField] private float m_moveSpeed = 10f;

        public float MoveSpeed => m_moveSpeed;
        public void SetMoveSpeed(float speed) => m_moveSpeed = speed;

        private Vector2 m_movement;
        private bool m_isStunned = false;

        /* --- References --- */
        private CombatComponent m_combatComponent;

        /* --- Animation Parameter Strings --- */
        private const string kMoveX = "MoveX";
        private const string kMoveY = "MoveY";
        private const string kIsMoving = "IsMoving";

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

            m_combatComponent = GetComponent<CombatComponent>();
            if (m_combatComponent == null)
            {
                Debug.LogWarning($"{nameof(CombatComponent)} is not present on {gameObject.name}. This is expected for non-combat entities.");
            }
        }

        /*---------------------------------------------------------
        | --- FixedUpdate: Called every fixed framerate frame --- |
        ---------------------------------------------------------*/
        private void FixedUpdate()
        {
            if (!m_isStunned)
            {
                Rigidbody2D.linearVelocity = m_movement * m_moveSpeed;
            }
        }

        /*--------------------------------------------------------
        | --- Move: Move the Entity in a specified direction --- |
        --------------------------------------------------------*/
        public void Move(Vector2 movement)
        {
            m_movement = movement;
            Animator.SetBool(kIsMoving, true);
            Animator.SetFloat(kMoveX, movement.x);
            Animator.SetFloat(kMoveY, movement.y);
        }

        /*------------------------------------------
        | --- Stop: Stop the Entity's movement --- |
        ------------------------------------------*/
        public void Stop()
        {
            m_movement = Vector2.zero;
            Animator.SetBool(kIsMoving, false);
        }

        /*-------------------------------------------------------------------------
        | --- StunEntity: Apply a Stun Effect that temporarily stops movement --- |
        -------------------------------------------------------------------------*/
        public void StunEntity(bool flag)
        {
            m_isStunned = flag;
            if (flag)
            {
                Stop();
            }
        }

        /*----------------------------------------------------------------
        | --- CaptureState: Captures the current State of the Entity --- |
        ----------------------------------------------------------------*/
        public object CaptureState()
        {
            return new MySerializableVector3(Transform.position);
        }

        /*---------------------------------------------------------------
        | --- RestoreState: Restores the Entity to a previous State --- |
        ---------------------------------------------------------------*/
        public void RestoreState(object state)
        {
            MySerializableVector3 position = (MySerializableVector3)state;
            Transform.position = position.ToVector3();
        }
    }
}