using StateMachine;
using UnityEngine;

namespace Player.States
{
    public class PlayerStunnedState : PlayerState
    {
        private float _stunDuration = 2f;
        private float _stunTime;
        
        private AudioClip _stunnedSound;
        private AudioSource _audioSource;
        
        public PlayerStunnedState(PlayerController pController) : base(pController)
        {
            _stunnedSound = Resources.Load<AudioClip>("stunned");
            _audioSource = pController.gameObject.AddComponent<AudioSource>();
        }

        public override void Enter()
        {
            base.Enter();
            PController.rb.velocity = Vector2.zero; 
            PController.anim.Play("StunnedAnimation"); 
            PlayStunnedSound();
            _stunTime = Time.time;
        }

        public override void Update()
        {
            base.Update();
            if (Time.time - _stunTime >= _stunDuration)
            {
                PController.StateMachine.ChangeState(PController.StateContainer.PlayerGroundState); // Return to ground state or any other appropriate state
            }
        }

        public override void Exit()
        {
            base.Exit();
            // Clean up if necessary
        }

        private void PlayStunnedSound()
        {
            if (_stunnedSound != null)
            {
                _audioSource.clip = _stunnedSound;
                _audioSource.volume = 0.5f;
                _audioSource.pitch = 1f;
                _audioSource.Play();
            }
        }
    }
}
