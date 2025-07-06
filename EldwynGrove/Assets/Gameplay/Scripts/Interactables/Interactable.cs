using UnityEngine;

namespace EldwynGrove
{
    public abstract class Interactable : MonoBehaviour
    {
        [Header("Interactable UI")]
        [SerializeField] private GameObject m_interactPromptPrefab;
        [SerializeField] private Vector3 m_promptOffset = Vector3.up;

        private GameObject m_promptInstance;
        protected bool m_isPlayerNearby = false;

        private static readonly string kPlayerTag = "Player";

        protected abstract void Interact();
        protected virtual bool CanShowPrompt() => true;

        /*----------------------------------------------------------------
        | --- Awake: Called when the script instance is being loaded --- |
        ----------------------------------------------------------------*/
        protected virtual void Awake()
        {
            if (m_interactPromptPrefab == null)
            {
                Debug.LogError("Interact prompt prefab not assigned on " + gameObject.name);
            }
        }

        /*---------------------------------------
        | --- Update: Called once per frame --- |
        ---------------------------------------*/
        protected virtual void Update()
        {
            if (m_isPlayerNearby && Input.GetKeyDown(KeyCode.E))
            {
                Interact();
            }
        }

        /*---------------------------------------------------------------------------
        | --- OnTriggerEnter2D: Called when another collider enters the trigger --- |
        ---------------------------------------------------------------------------*/
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(kPlayerTag))
            {
                m_isPlayerNearby = true;
                ShowPrompt();
            }
        }

        /*-------------------------------------------------------------------------
        | --- OnTriggerExit2D: Called when another collider exits the trigger --- |
        -------------------------------------------------------------------------*/
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag(kPlayerTag))
            {
                m_isPlayerNearby = false;
                HidePrompt();
            }
        }

        /*-------------------------------------------------------------------------------------
        | --- ShowPrompt: Instantiates the interact prompt prefab at the specified offset --- |
        -------------------------------------------------------------------------------------*/
        private void ShowPrompt()
        {
            if (m_interactPromptPrefab != null && m_promptInstance == null && CanShowPrompt())
            {
                m_promptInstance = Instantiate(m_interactPromptPrefab, transform.position + m_promptOffset, Quaternion.identity, transform);
            }
        }

        /*------------------------------------------------------------------------
        | --- HidePrompt: Destroys the interact prompt instance if it exists --- |
        ------------------------------------------------------------------------*/
        protected void HidePrompt()
        {
            if (m_promptInstance != null)
            {
                Destroy(m_promptInstance);
                m_promptInstance = null;
            }

        }
    }
}