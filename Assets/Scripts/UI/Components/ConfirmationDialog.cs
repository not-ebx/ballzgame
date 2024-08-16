using UnityEngine;
using UnityEngine.UIElements;

namespace Script.UI.Components
{
    public class ConfirmationDialog : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<ConfirmationDialog, UxmlTraits> { }

        private Label confirmationDialogTitle;
        private Label confirmationDialogMessage;
        private Button acceptButton;
        private Button rejectButton;
        
        public ConfirmationDialog()
        {
            // Load the UXML and USS
            var visualTree = Resources.Load<VisualTreeAsset>("UI/Components/ConfirmationDialog/ConfirmationDialog");
            visualTree.CloneTree(this);

            // Get references to UI elements
            confirmationDialogTitle = this.Q<Label>("confirmationDialogTitle");
            confirmationDialogMessage = this.Q<Label>("confirmationDialogMessage");
            acceptButton = this.Q<Button>("acceptButton");
            rejectButton = this.Q<Button>("rejectButton");
        }

        public void Initialize(string title, string message, System.Action onAccept, System.Action onReject)
        {
            confirmationDialogTitle.text = title;
            confirmationDialogMessage.text = message;

            // Set button callbacks
            acceptButton.clicked += () =>
            {
                onAccept?.Invoke();
                Close();
            };
            rejectButton.clicked += () =>
            {
                onReject?.Invoke();
                Close();
            };
        }

        private void Close()
        {
            this.RemoveFromHierarchy();
        }
    }
}