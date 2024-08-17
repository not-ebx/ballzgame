using System;
using Player;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Behaviours
{
    public class InGameUIBehaviour : MonoBehaviour
    {
        private Label _currentStateLabel;
        private Label _currentInputLabel;
        private Label _currentVelocityLabel;
        public PlayerController player;
        
        private void Start()
        {
            // Load the UXML and USS
            var visualTree = Resources.Load<VisualTreeAsset>("UI/Screens/InGameUI/InGameUI");
            visualTree.CloneTree(this.GetComponent<UIDocument>().rootVisualElement);
            
            // Get references to UI elements
            _currentStateLabel = this.GetComponent<UIDocument>().rootVisualElement.Q<Label>("current-state");
            _currentInputLabel = this.GetComponent<UIDocument>().rootVisualElement.Q<Label>("current-input");
            _currentVelocityLabel = this.GetComponent<UIDocument>().rootVisualElement.Q<Label>("current-velocity");
        }

        public void SetCurrentState(string stateName)
        {
            _currentStateLabel.text = "Current State: " + stateName;
        }

        public void SetCurrentInputs()
        {
            _currentInputLabel.text = "Current Inputs: " + player.PlayerInputActions.Player.Move.ReadValue<Vector2>();
        }

        public void SetCurrentVelocity()
        {
            _currentVelocityLabel.text = "Current Velocity: " + player.rb.velocity;
        }

        private void Update()
        {
            SetCurrentState(player.StateMachine.GetCurrentStateName());
            SetCurrentInputs();
            SetCurrentVelocity();
        }
    }
}