using UnityEngine;

namespace EldwynGrove
{
    public class CombatComponent : EntityComponent
    {
        [Header("Combat Settings")]
        [SerializeField] private Weapon m_equippedWeapon;
        [SerializeField] private float m_attackCooldown = 1f;
        private float m_attackTimer = 0f;

        /* --- Animation Parameter Strings --- */
        private const string kAttack = "Attack";

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
        }

        /*-----------------------------------------
        | --- Update: Called upon every frame --- |
        -----------------------------------------*/
        private void Update()
        {
            if (m_attackTimer > 0)
            {
                m_attackTimer -= Time.deltaTime;
            }
        }

        /*----------------------------------------------------------------
        | --- Attack: The Behavior of an Entity performing an Attack --- |
        ----------------------------------------------------------------*/
        public void Attack()
        {
            if (m_equippedWeapon != null && m_attackTimer <= 0)
            {
                Animator.SetTrigger(kAttack);
                m_attackTimer = m_attackCooldown;
            }
        }
    }
}