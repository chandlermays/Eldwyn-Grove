using UnityEngine;
//---------------------------------
using EldwynGrove.Saving;

namespace EldwynGrove
{
    public class SaveLoadController : MonoBehaviour
    {
        private const string kDefaultSaveFile = "EldwynGroveSaveFile";

        /* --- Key Bindings --- */
        [SerializeField] private KeyCode kSaveKey = KeyCode.F5;
        [SerializeField] private KeyCode kLoadKey = KeyCode.F9;

        /*-----------------------------------------------------
        | --- Start: Called before the first frame update --- |
        -----------------------------------------------------*/
        private void Start()
        {
            TransitionFade transitionFade = FindFirstObjectByType<TransitionFade>();

            transitionFade.ResetFade();
            StartCoroutine(GetComponent<SaveSystem>().LoadLastScene(kDefaultSaveFile));
            transitionFade.FadeIn();
        }

        /*-----------------------------------------
        | --- Update: Called upon every frame --- |
        -----------------------------------------*/
        private void Update()
        {
            if (Input.GetKeyDown(kSaveKey))
            {
                Save();
            }
            if (Input.GetKeyDown(kLoadKey))
            {
                Load();
            }
        }

        /*--------------------------------------------------------
        | --- Save: Perform the Action of Saving to the File --- |
        --------------------------------------------------------*/
        public void Save()
        {
            GetComponent<SaveSystem>().Save(kDefaultSaveFile);
        }

        /*---------------------------------------------------------
        | --- Load: Perform the Action of Loading from a File --- |
        ---------------------------------------------------------*/
        public void Load()
        {
            GetComponent<SaveSystem>().Load(kDefaultSaveFile);
        }
    }
}