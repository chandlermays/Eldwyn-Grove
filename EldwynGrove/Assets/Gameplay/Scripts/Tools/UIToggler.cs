using UnityEngine;

namespace EldwynGrove
{
    public class UIToggler : MonoBehaviour
    {
        [SerializeField] private GameObject m_uiPrefab;
        [SerializeField] private KeyCode m_toggleKey;

        /*-----------------------------------------------------
        | --- Start: Called before the first frame update --- |
        -----------------------------------------------------*/
        private void Start()
        {
            if (m_uiPrefab == null)
            {
                Debug.LogError("UI Prefab is not assigned.");
                return;
            }
            m_uiPrefab.SetActive(false);
        }

        /*---------------------------------------
        | --- Update: Called once per frame --- |
        ---------------------------------------*/
        private void Update()
        {
            if (Input.GetKeyDown(m_toggleKey))
            {
                ToggleUI();
            }
        }

        /*--------------------------------------------
        | --- ToggleUI: Toggle the UI visibility --- |
        --------------------------------------------*/
        public void ToggleUI()
        {
            Debug.Log("ToggleUI called");
            m_uiPrefab.SetActive(!m_uiPrefab.activeSelf);
        }
    }
}