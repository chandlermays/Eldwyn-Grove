using UnityEngine;

namespace EldwynGrove
{
    public class EnemyController : MonoBehaviour
    {
        [Header("AI Settings")]
        [SerializeField] private float m_wanderRange = 10f;
        [SerializeField] private float m_detectionRadius = 5f;
        [SerializeField] private float m_attackThreshold = 1.5f;
        [SerializeField] private float m_suspicionTime = 3f;
        [SerializeField] private float m_destinationTolerance = 0.1f;

        /* --- Component References --- */
        private Animator m_animator;
        private MovementComponent m_movementComponent;
        private HealthComponent m_healthComponent;
        private CombatComponent m_combatComponent;

        private float m_timeSinceLastSawPlayer = 0f;

        private Vector2 m_initialPosition;
        private Transform m_target;
        private const string kPlayerTag = "Player";

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

            m_movementComponent = GetComponent<MovementComponent>();
            Utilities.CheckForNull(m_movementComponent, nameof(m_movementComponent));

            m_healthComponent = GetComponent<HealthComponent>();
            Utilities.CheckForNull(m_healthComponent, nameof(m_healthComponent));

            m_combatComponent = GetComponent<CombatComponent>();
            Utilities.CheckForNull(m_combatComponent, nameof(m_combatComponent));
        }

        /*-----------------------------------------------------
        | --- Start: Called before the first frame update --- |
        -----------------------------------------------------*/
        private void Start()
        {
            m_initialPosition = transform.position;
            m_target = GameObject.FindWithTag(kPlayerTag).transform;
        }

        /*-----------------------------------------
        | --- Update: Called upon every frame --- |
        -----------------------------------------*/
        private void Update()
        {
            if (m_healthComponent.IsDead)
            {
                m_movementComponent.Stop();
                return;
            }

            if (IsWithinRange(m_detectionRadius))
            {
                m_timeSinceLastSawPlayer = 0f;
                if (IsWithinRange(m_attackThreshold))
                {
                    Attack();
                }
                else
                {
                    MoveToDestination(m_target.position);
                }
            }
            else
            {
                m_timeSinceLastSawPlayer += Time.deltaTime;
                m_movementComponent.Stop();
        
                if (m_timeSinceLastSawPlayer >= m_suspicionTime)
                {
                    MoveToDestination(m_initialPosition);
                }
            }
        }

        /*----------------------------------------------------------------
        | --- Attack: The Behavior of the Agent Attacking the Target --- |
        ----------------------------------------------------------------*/
        private void Attack()
        {
            m_movementComponent.Stop();
            m_combatComponent.Attack();
        }

        /*-------------------------------------------------------------------------------------------------
        | --- IsWithinRange: Returns if the Agent is in Range of a specified Distance from the Target --- |
        -------------------------------------------------------------------------------------------------*/
        private bool IsWithinRange(float distance)
        {
            return Vector2.Distance(transform.position, m_target.position) < distance;
        }

        /*-----------------------------------------------------------------------
        | --- MoveToDestination: Move the Agent towards the Target Position --- |
        -----------------------------------------------------------------------*/
        private void MoveToDestination(Vector2 destination)
        {
            if (Vector2.Distance(transform.position, destination) > m_destinationTolerance)
            {
                Vector2 direction = (destination - (Vector2)transform.position).normalized;
                m_movementComponent.Move(direction);
            }
            else
            {
                m_movementComponent.Stop();
            }
        }

        /*-----------------------------------------------------------------------
        | --- OnDrawGizmosSelected: Draw Gizmos when the object is selected --- |
        -----------------------------------------------------------------------*/
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, m_detectionRadius);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, m_attackThreshold);
        }
    }
}