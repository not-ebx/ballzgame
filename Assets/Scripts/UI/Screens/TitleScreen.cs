using Script.UI.Components;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Screens
{
    public class TitleScreen : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<ConfirmationDialog, UxmlTraits> { }

        private Button _newGame;
        private Button _continue;
        private Button _quit;

        public TitleScreen()
        {
            // Load the UXML and USS
            var visualTree = Resources.Load<VisualTreeAsset>("UI/Screens/TitleScreen/TitleScreen");
            visualTree.CloneTree(this);

            // Get references to UI elements
            _newGame = this.Q<Button>("new-game");
            _continue = this.Q<Button>("continue");
            _quit = this.Q<Button>("quit");
        }

    }
}