using UnityEngine;

namespace EldwynGrove
{
    public class Chest : Interactable
    {
        [SerializeField] private UIToggler m_uiToggler;
        [SerializeField] private Sprite m_openSprite;
        [SerializeField] private Sprite m_closedSprite;

        private SpriteRenderer m_spriteRenderer;
        private bool m_isChestOpen = false;

        protected override void Awake()
        {
            base.Awake();
            m_spriteRenderer = GetComponent<SpriteRenderer>();
            Utilities.CheckForNull(m_spriteRenderer, nameof(m_spriteRenderer));
            Utilities.CheckForNull(m_uiToggler, nameof(m_uiToggler));
        }

        protected override void Interact()
        {
            if (m_isChestOpen)
            {
                CloseChest();
                m_uiToggler.ToggleUI();
            }
            else
            {
                OpenChest();
                m_uiToggler.ToggleUI();
            }
        }

        protected override bool CanShowPrompt()
        {
            return true;
        }

        private void OpenChest()
        {
            m_isChestOpen = true;
            if (m_spriteRenderer != null && m_openSprite != null)
            {
                m_spriteRenderer.sprite = m_openSprite;
            }
        }

        public void CloseChest()
        {
            if (m_isChestOpen)
            {
                m_isChestOpen = false;
                if (m_spriteRenderer != null && m_closedSprite != null)
                {
                    m_spriteRenderer.sprite = m_closedSprite;
                }
            }
        }

    }
}