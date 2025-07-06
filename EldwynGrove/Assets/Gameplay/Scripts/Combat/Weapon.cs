using System.Collections;
using UnityEngine;

namespace EldwynGrove
{
    public class Weapon : MonoBehaviour
    {
        [Header("Weapon Settings")]
        [SerializeField] private float m_damage = 10f;
        [SerializeField] private float m_knockbackForce = 30f;
        private readonly float m_knockbackDuration = 0.1f;

        private GameObject m_owner;
        private const string kPlayerTag = "Player";
        private const string kEnemyTag = "Enemy";

        /*----------------------------------------------------------------*/
        /*----------------------------
        | --- Method Definitions --- |
        ----------------------------*/

        /*----------------------------------------------------------------
        | --- Awake: Called when the script instance is being loaded --- |
        ----------------------------------------------------------------*/
        private void Awake()
        {
            m_owner = transform.parent.gameObject;
        }

        /*----------------------------------------------------------------------------
        | --- OnTriggerEnter2D: Called when another collider enters this trigger --- |
        ----------------------------------------------------------------------------*/
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject != m_owner)
            {
                // Check if the attacker and m_target are of the same type
                bool isSameType = (m_owner.CompareTag(kPlayerTag) && collision.CompareTag(kPlayerTag)) ||
                                  (m_owner.CompareTag(kEnemyTag) && collision.CompareTag(kEnemyTag));

                if (isSameType) return;

                // Apply damage to the entity
                if (collision.TryGetComponent<HealthComponent>(out var entity))
                {
                    entity.TakeDamage(m_owner, m_damage);
                }

                // Apply knockback
                ApplyKnockback(collision);
            }
        }

        /*----------------------------------------------------------------------------
        | --- ApplyKnockback: Apply a Knockback Force to the Entity on Collision --- |
        ----------------------------------------------------------------------------*/
        private void ApplyKnockback(Collider2D collision)
        {
            if (collision.TryGetComponent<Rigidbody2D>(out var rb2D))
            {
                Vector2 direction = (collision.transform.position - transform.position).normalized;
                rb2D.AddForce(direction * m_knockbackForce, ForceMode2D.Impulse);
                MovementComponent movementComp = collision.GetComponent<MovementComponent>();
                movementComp.StunEntity(true);
                StartCoroutine(RemoveStunAfterDelay(movementComp, m_knockbackDuration));
            }
        }

        /*------------------------------------------------------------
        | --- RemoveStun: Remove the Stun Effect from the Entity --- |
        ------------------------------------------------------------*/
        private IEnumerator RemoveStunAfterDelay(MovementComponent movementComp, float delay)
        {
            yield return new WaitForSeconds(delay);
            movementComp.StunEntity(false);
        }
    }
}