using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI.Screens
{
    public class TitleScreen : MonoBehaviour 
    {
        private Button _arcadeMode;
        private Button _freeMode;

        void Start()
        {
            // Load the UXML and USS
            var visualTree = Resources.Load<VisualTreeAsset>("UI/Screens/TitleScreen/TitleScreen");
            if (visualTree == null)
            {
                Debug.LogError("VisualTreeAsset not found at the specified path.");
                return;
            }

            var rootVisualElement = GetComponent<UIDocument>().rootVisualElement;
            visualTree.CloneTree(rootVisualElement);

            // Get references to UI elements
            _arcadeMode = rootVisualElement.Q<Button>("arcade-mode");
            _freeMode = rootVisualElement.Q<Button>("free-mode");

            if (_arcadeMode == null || _freeMode == null)
            {
                Debug.LogError("One or more UI elements are not found in the UXML.");
                return;
            }

            _arcadeMode.clicked += () => StartCoroutine(LoadSceneWithDelay("ArcadeModeScene"));
            _freeMode.clicked += () => StartCoroutine(LoadSceneWithDelay("Chapter-Test"));
        }
        
        private IEnumerator LoadSceneWithDelay(string sceneName)
        {
            // Wait for 1/2 second
            yield return new WaitForSeconds(0.3f);

            // Load the scene
            SceneManager.LoadScene(sceneName);
        }
    }
}