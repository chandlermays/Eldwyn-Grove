using UnityEngine;

namespace EldwynGrove
{
    public class SlimeComponent : EntityComponent
    {
        [Header("Slime Split Settings")]
        [SerializeField] private GameObject m_smallerSlimePrefab;
        [SerializeField] private int m_childrenCount = 2;
        [SerializeField] private float m_spawnRadius = 0.5f;
        [SerializeField] private float m_maxSplitHealthPercentage = 0.75f;      // -> Split if above this health percentage
        [SerializeField] private float m_minSplitHealthPercentage = 0.2f;       // -> Don't split if below this health percentage
        [SerializeField] private float m_splitForce = 3f;

        [Header("Child Slime Properties")]
        [SerializeField] private float m_childHealthPercentage = 0.5f;          // -> Health relative to parent
        [SerializeField] private float m_childSizeScale = 0.6f;                 // -> Scale relative to parent

        /* --- Components --- */
        private HealthComponent m_healthComponent;
        private Stats m_stats;

        private bool m_canSplit = true;
        private bool m_hasSplit = false;

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
            m_healthComponent = GetComponent<HealthComponent>();
            Utilities.CheckForNull(m_healthComponent, nameof(m_healthComponent));

            m_stats = GetComponent<Stats>();
            Utilities.CheckForNull(m_stats, nameof(m_stats));
        }

        /*-----------------------------------------------------
        | --- Start: Called before the first frame update --- |
        -----------------------------------------------------*/
        private void Start()
        {
            // Subscribe to the OnHealthChanged event
            m_healthComponent.OnHealthChanged += CheckForSplit;
        }

        /*---------------------------------------------------------
        | --- OnDestroy: Cleanup when the object is destroyed --- |
        ---------------------------------------------------------*/
        private void OnDestroy()
        {
            // Unsubscribe from the event when destroyed
            if (m_healthComponent != null)
            {
                m_healthComponent.OnHealthChanged -= CheckForSplit;
            }
        }

        /*--------------------------------------------------------------
        | --- CheckForSplit: Check if we should split after damage --- |
        --------------------------------------------------------------*/
        private void CheckForSplit()
        {
            // Only proceed if health has changed and we're not dead
            if (m_healthComponent.IsDead || !m_canSplit || m_hasSplit) return;

            // Calculate health percentage
            float healthPercentage = m_healthComponent.CurrentHealth / m_stats.GetHealth();

            // Only split if health is below some threshold (showing damage but not dead)
            // and above minimum threshold (avoid splitting when almost dead)
            if (healthPercentage < m_maxSplitHealthPercentage && healthPercentage > m_minSplitHealthPercentage)
            {
                SplitSlime();
            }
        }

        /*-------------------------------------------------------
        | --- SplitSlime: Split the slime into smaller ones --- |
        -------------------------------------------------------*/
        private void SplitSlime()
        {
            // Mark that we've split, so we don't split again
            m_hasSplit = true;

            // Spawn the smaller slimes in a circle pattern
            for (int i = 0; i < m_childrenCount; i++)
            {
                // Calculate spawn position in a circle around current position
                float angle = (360f / m_childrenCount) * i;
                Vector2 spawnDirection = Quaternion.Euler(0, 0, angle) * Vector2.right;
                Vector2 spawnPosition = (Vector2)Transform.position + spawnDirection * m_spawnRadius;

                // Instantiate the smaller slime
                GameObject childSlime = Instantiate(m_smallerSlimePrefab, spawnPosition, Quaternion.identity);

                // Configure the child slime
                ConfigureChildSlime(childSlime, spawnDirection);
            }
        }

        /*-----------------------------------------------------------------
        | --- ConfigureChildSlime: Configure the spawned child slimes --- |
        -----------------------------------------------------------------*/
        private void ConfigureChildSlime(GameObject childSlime, Vector2 direction)
        {
            // Scale the child slime
            childSlime.transform.localScale = Transform.localScale * m_childSizeScale;

            // Set child health
            if (childSlime.TryGetComponent<HealthComponent>(out var childHealth))
            {
                float childHealthValue = m_healthComponent.CurrentHealth * m_childHealthPercentage;
                childHealth.SetHealth(childHealthValue);
            }

            // Disable splitting for child slimes
            if (childSlime.TryGetComponent<SlimeComponent>(out var slimeComponent))
            {
                slimeComponent.SetCanSplit(false);
            }

            // Add a force to push the slimes outward
            if (childSlime.TryGetComponent<Rigidbody2D>(out var rb))
            {
                rb.AddForce(direction * m_splitForce, ForceMode2D.Impulse);
            }
        }

        /*----------------------------------------------------------
        | --- SetCanSplit: Enable or disable splitting ability --- |
        ----------------------------------------------------------*/
        public void SetCanSplit(bool canSplit)
        {
            m_canSplit = canSplit;
        }
    }
}