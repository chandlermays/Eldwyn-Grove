using UnityEngine;
using System;
//---------------------------------
using EldwynGrove.Saving;

namespace EldwynGrove
{
    public class HealthComponent : EntityComponent, ISaveable
    {
        [Header("Health Settings")]
        private float m_health;

        private bool m_isDead = false;
        public event Action OnHealthChanged;            // Event to notify health changes

        public float CurrentHealth => m_health;         // Current health of the entity
        public bool IsDead => m_isDead;                 // Is the entity dead?

        /* --- Components --- */
        private Stats m_stats;

        /* --- Animation Parameter Strings --- */
        private const string kWasHit = "WasHit";
        private const string kDeath = "Death";

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
            m_stats = GetComponent<Stats>();
            m_health = m_stats.GetHealth();
            OnHealthChanged?.Invoke();
        }

        /*-------------------------------------------------
        | --- SetHealth: Set the Health of the Entity --- |
        -------------------------------------------------*/
        public void SetHealth(float health)
        {
            m_health = health;
            OnHealthChanged?.Invoke();
        }

        /*----------------------------------------------------------------------
        | --- TakeDamage: The Behavior of this Entity when it takes damage --- |
        ----------------------------------------------------------------------*/
        public void TakeDamage(GameObject instigator, float damage)
        {
            if (m_isDead) return;

            m_health -= damage;
            Animator.SetTrigger(kWasHit);

            OnHealthChanged?.Invoke();

            if (m_health <= 0)
            {
                OnDeath();
                GrantExperience(instigator);
            }
        }

        /*-----------------------------------------------------------
        | --- OnDeath: The Behavior of this Entity when it dies --- |
        -----------------------------------------------------------*/
        private void OnDeath()
        {
            if (m_isDead) return;

            m_isDead = true;
            Animator.ResetTrigger(kWasHit);
            Animator.SetTrigger(kDeath);
            BoxCollider.enabled = false;
        }

        /*------------------------------------------------------------
        | --- GrantExperience: Give Experience to the Instigator --- |
        ------------------------------------------------------------*/
        private void GrantExperience(GameObject instigator)
        {
            if (!instigator.TryGetComponent<Experience>(out var experience)) return;

            experience.GainExperience(GetComponent<Stats>().GetExperience());
        }

        /*--------------------------------------------------------------
        | --- ReplenishHealth: Fully replenish the Entity's Health --- |
        --------------------------------------------------------------*/
        public void ReplenishHealth()
        {
            m_health = m_stats.GetHealth();
            OnHealthChanged?.Invoke();
        }

        /*----------------------------------------------------------------------
        | --- CaptureState: Save the Current State of the Health Component --- |
        ----------------------------------------------------------------------*/
        public object CaptureState()
        {
            return m_health;
        }

        /*----------------------------------------------------------------------
        | --- RestoreState: Load the Current State of the Health Component --- |
        ----------------------------------------------------------------------*/
        public void RestoreState(object state)
        {
            m_health = (float)state;
            OnHealthChanged?.Invoke();

            if (m_health <= 0)
            {
                OnDeath();
            }
        }
    }
}