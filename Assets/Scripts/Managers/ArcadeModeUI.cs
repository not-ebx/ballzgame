using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Screens
{
    public class ArcadeModeUI : MonoBehaviour
    {
        private Label _timerLabel;
        private float _timerDuration = 180f; // 2 minutos en segundos
        private float _timeRemaining;

        private void Start()
        {
            // Cargar el UXML y aplicar al UIDocument
            var visualTree = Resources.Load<VisualTreeAsset>("UI/Screens/TimerUI/TimerUI");
            visualTree.CloneTree(GetComponent<UIDocument>().rootVisualElement);

            // Obtener referencia al Label del temporizador
            _timerLabel = GetComponent<UIDocument>().rootVisualElement.Q<Label>("timer");

            // Inicializar el tiempo restante
            _timeRemaining = _timerDuration;
            UpdateTimerText();
        }

        private void Update()
        {
            _timeRemaining -= Time.deltaTime;
            if (_timeRemaining <= 0)
            {
                _timeRemaining = 0;
                UnityEngine.SceneManagement.SceneManager.LoadScene("Victoria");
            }
            UpdateTimerText();
        }

        private void UpdateTimerText()
        {
            int minutes = Mathf.FloorToInt(_timeRemaining / 60);
            int seconds = Mathf.FloorToInt(_timeRemaining % 60);
            _timerLabel.text = string.Format("Timer: {0:00}:{1:00}", minutes, seconds);
        }
    }
}
