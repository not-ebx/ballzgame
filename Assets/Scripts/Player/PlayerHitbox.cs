using System.Collections;
using Managers;
using UnityEngine;

namespace Player
{
    public class PlayerHitbox : MonoBehaviour
    {
        public Vector2 hitBoxDirection;
        public float damage;
        public float hitLag = 0.0f;
        public bool isHitting = false;
        private bool _isUsed = false;

        // This method makes the world stop for a duration. Used for feedback when hitting the ball.
        public void Stop(float duration)
        {
            if (isHitting)
                return;
            Time.timeScale = 0.0f;
            CoroutineManager.Instance.StartManagedCoroutine(Wait(duration));
        }

        IEnumerator Wait(float duration)
        {
            isHitting = true;
            yield return new WaitForSecondsRealtime(duration);
            Time.timeScale = 1.0f;
            isHitting = false;
            Destroy(this);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Ball") && !_isUsed)
            {
                _isUsed = true;
                var ball = other.GetComponent<BallPhysics>();
                ball.OnHitBall(this, hitBoxDirection, damage, hitLag);
            }
        }
    }
}