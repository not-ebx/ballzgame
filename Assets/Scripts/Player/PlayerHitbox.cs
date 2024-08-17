using System;
using UnityEngine;

namespace Player
{
    public class PlayerHitbox : MonoBehaviour
    {
        public Vector2 hitBoxDirection;
        public float damage;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Ball"))
            {
                var ball = other.GetComponent<BallPhysics>();
                ball.OnHitBall(hitBoxDirection, damage);
            }
        }
    }
}