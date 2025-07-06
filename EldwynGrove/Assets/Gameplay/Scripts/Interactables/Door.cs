using UnityEngine;

namespace EldwynGrove
{
    public class Door : Interactable
    {
        [SerializeField] private BoxCollider2D m_boxCollider2D;
        [SerializeField] private Sprite m_openSprite;
        [SerializeField] private Sprite m_closedSprite;

        private SpriteRenderer m_spriteRenderer;
        private bool m_isDoorOpen = false;

        /*----------------------------------------------------------------
        | --- Awake: Called when the script instance is being loaded --- |
        ----------------------------------------------------------------*/
        protected override void Awake()
        {
            base.Awake();
            m_spriteRenderer = GetComponent<SpriteRenderer>();
            if (m_spriteRenderer == null)
            {
                Debug.LogError("SpriteRenderer component is missing on the Door object.");
            }
        }

        /*------------------------------------------------------------------
        | --- Interact: Called when the player interacts with the door --- |
        ------------------------------------------------------------------*/
        protected override void Interact()
        {
            ToggleDoor();
            if (m_isPlayerNearby && m_isDoorOpen)
            {
                HidePrompt();
            }
        }

        /*------------------------------------------------------------------------
        | --- CanShowPrompt: Checks if the prompt can be shown to the player --- |
        ------------------------------------------------------------------------*/
        protected override bool CanShowPrompt()
        {
            return !m_isDoorOpen;
        }

        /*--------------------------------------------------------------------------------------
        | --- ToggleDoor: Toggles the door open/closed and updates the sprite and collider --- |
        --------------------------------------------------------------------------------------*/
        private void ToggleDoor()
        {
            m_isDoorOpen = !m_isDoorOpen;
            m_spriteRenderer.sprite = m_isDoorOpen ? m_openSprite : m_closedSprite;
            m_boxCollider2D.enabled = !m_isDoorOpen;
        }
    }
}