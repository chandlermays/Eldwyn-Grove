using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
//---------------------------------
// TODO: Try making something like a 'TransitionContext' for the Player's direction to decouple Portal : PlayerController

namespace EldwynGrove
{
    public class Portal : MonoBehaviour
    {
        [Header("Portal Settings")]
        [SerializeField] private SceneField m_sceneField;
        [SerializeField] private Transform m_spawnPoint;

        private static readonly List<Portal> s_portals = new();

        private Transform m_player;
        private const string kPlayerTag = "Player";
        private Vector2 m_playerDirection;

        /*---------------------------------------------------------------------
        | --- OnEnable: Called when the object becomes enabled and active --- |
        ---------------------------------------------------------------------*/
        private void OnEnable()
        {
            s_portals.Add(this);
        }

        /*--------------------------------------------------------
        | --- OnDisable: Called when the object is destroyed --- |
        --------------------------------------------------------*/
        private void OnDestroy()
        {
            s_portals.Remove(this);
        }

        /*------------------------------------------------------------------------------
        | --- OnTriggerEnter: Called when the Collider 'other' enters this trigger --- |
        ------------------------------------------------------------------------------*/
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(kPlayerTag))
            {
                Debug.Log("Player entered the portal trigger.");
                m_player = other.transform;

                // Store the direction
                PlayerController playerController = m_player.GetComponent<PlayerController>();
                m_playerDirection = playerController != null ? playerController.GetDirection() : Vector2.right; // Default direction if not found

                StartCoroutine(SceneTransition());
            }
        }

        /*-------------------------------------------------------------
        | --- SceneTransition: Coroutine to Transition the Scenes --- |
        -------------------------------------------------------------*/
        private IEnumerator SceneTransition()
        {
            DontDestroyOnLoad(gameObject);

            // These are amongst the "PerservedObjects" in the game that get created upon loading scenes
            TransitionFade fade = FindFirstObjectByType<TransitionFade>();
            SaveLoadController saveLoadController = FindFirstObjectByType<SaveLoadController>();

            PlayerController playerController = m_player.GetComponent<PlayerController>();
            playerController.enabled = false;

            yield return fade.FadeOut();

            saveLoadController.Save();

            yield return SceneManager.LoadSceneAsync(m_sceneField.SceneName);

            // Cannot use cached player, have to find it again in the new scene
            PlayerController newPlayerController = GameObject.FindWithTag(kPlayerTag).GetComponent<PlayerController>();
            newPlayerController.enabled = false;

            saveLoadController.Load();

            Portal destination = GetDestination();
            UpdatePlayer(destination);

            saveLoadController.Save();

            yield return fade.Wait();
            yield return fade.FadeIn();

            newPlayerController.enabled = true;
            Destroy(gameObject);
        }

        /*--------------------------------------------------------------------
        | --- GetDestination: Returns the Portal the Player is headed to --- |
        --------------------------------------------------------------------*/
        private Portal GetDestination()
        {
            foreach (Portal portal in s_portals)
            {
                if (portal != this)
                {
                    return portal;
                }
            }

            return null;
        }

        /*-------------------------------------------------------------------------------------
        | --- UpdatePlayer: Update the Player's Position to the Destination's Spawn Point --- |
        -------------------------------------------------------------------------------------*/
        private void UpdatePlayer(Portal destination)
        {
            // Retrieve the player object again in the new scene
            m_player = GameObject.FindWithTag(kPlayerTag).transform;
            if (m_player == null)
            {
                Debug.LogError("Player object not found in the new scene!");
                return;
            }
            m_player.position = destination.m_spawnPoint.position;

            // Restore facing direction
            PlayerController playerController = m_player.GetComponent<PlayerController>();
            MovementComponent movementComponent = m_player.GetComponent<MovementComponent>();
            if (playerController != null && movementComponent != null)
            {
                playerController.SetDirection(m_playerDirection);
                movementComponent.Move(m_playerDirection);
                movementComponent.Stop();
            }
        }
    }
}