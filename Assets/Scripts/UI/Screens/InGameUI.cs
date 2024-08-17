using Player;
using Script.UI.Components;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Screens
{
    public class InGameUI : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<ConfirmationDialog, UxmlTraits> { }

        private Label _currentStateLabel;
        
        public InGameUI()
        {
            // Load the UXML and USS
            var visualTree = Resources.Load<VisualTreeAsset>("UI/Screens/InGameUI/InGameUI");
            visualTree.CloneTree(this);

            // Get references to UI elements
            _currentStateLabel = this.Q<Label>("current-state");
        }
        
        public void SetCurrentState(string stateName)
        {
            _currentStateLabel.text = stateName;
        }
    }
    
}