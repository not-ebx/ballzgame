using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Screens
{
    public class VictoryScreen : MonoBehaviour
    {
        private Label _scoreLabel;

        private void Start()
        {
            var visualTree = Resources.Load<VisualTreeAsset>("UI/Screens/VictoryScreen/VictoryScreenUI");
            visualTree.CloneTree(GetComponent<UIDocument>().rootVisualElement);

            _scoreLabel = GetComponent<UIDocument>().rootVisualElement.Q<Label>("scoreLabel");
            DisplayFinalScore();
        }

        private void DisplayFinalScore()
        {
            _scoreLabel.text = "Final Score: " + GameData.FinalScore;
        }
    }
}
