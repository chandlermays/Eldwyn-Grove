using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace EldwynGrove
{
    public class PauseMenu : MonoBehaviour
    {
        private bool m_isGamePaused = false;

        [SerializeField] private GameObject m_background;
        [SerializeField] private GameObject m_menuPanel;

        /*----------------------------------------------------------
        | --- Start: Called just before the first frame update --- |
        ----------------------------------------------------------*/
        private void Start()
        {
            m_background.SetActive(false);
            m_menuPanel.SetActive(false);
        }

        /*---------------------------------------
        | --- Update: Called once per frame --- |
        ---------------------------------------*/
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (m_isGamePaused)
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
            }
        }

        /*--------------------------------------------------------------------
        | --- PauseGame: Called when the game is paused to show the menu --- |
        --------------------------------------------------------------------*/
        private void PauseGame()
        {
            m_background.SetActive(true);
            m_menuPanel.SetActive(true);
            Time.timeScale = 0f;
            m_isGamePaused = true;
        }

        /*----------------------------------------------------------------------
        | --- ResumeGame: Called when the game is resumed to hide the menu --- |
        ----------------------------------------------------------------------*/
        public void ResumeGame()
        {
            m_background.SetActive(false);
            m_menuPanel.SetActive(false);
            Time.timeScale = 1f;
            m_isGamePaused = false;
        }

        /*-----------------------------------------------------------------
        | --- QuitGame: Called when the player wants to quit the game --- |
        -----------------------------------------------------------------*/
        public void QuitGame()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}