using UnityEngine;
using UnityEngine.UIElements;

namespace Script.UI.Components
{
    public class VisualDialog : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<ConfirmationDialog, UxmlTraits> { }

        private Label _authorName;
        private Label _dialogText;
        private Image _leftPortrait;
        private Image _rightPortrait;
        
        public VisualDialog()
        {
            // Load the UXML and USS
            var visualTree = Resources.Load<VisualTreeAsset>("UI/Components/VisualDialog/VisualDialog");
            visualTree.CloneTree(this);

            // Get references to UI elements
            _authorName = this.Q<Label>("authorName");
            _dialogText = this.Q<Label>("dialogText");
            _leftPortrait = this.Q<Image>("leftPortrait");
            _rightPortrait = this.Q<Image>("rightPortrait");
        }
     
    }
}