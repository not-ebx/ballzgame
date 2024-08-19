using System.Collections;
using UnityEngine;

namespace Player
{
    public class PlayerHitbox : MonoBehaviour
    {
        public Vector2 hitBoxDirection;
        public float damage;
        public float hitLag = 0.0f;
        private bool _isHitting = false;

        // This method makes the world stop for a duration. Used for feedback when hitting the ball.
        public void Stop(float duration)
        {
            if (_isHitting)
                return;
            Time.timeScale = 0.0f;
            StartCoroutine(Wait(duration));
        }

        IEnumerator Wait(float duration)
        {
            _isHitting = true;
            yield return new WaitForSecondsRealtime(duration);
            Time.timeScale = 1.0f;
            _isHitting = false;
            Destroy(this);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Ball"))
            {
                var ball = other.GetComponent<BallPhysics>();
                ball.OnHitBall(this, hitBoxDirection, damage, hitLag);
            }
        }
    }
}