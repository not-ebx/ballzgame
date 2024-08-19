using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public enum AttackType
    {
        AerialAttack,
        GroundAttack
    }
    
    public static class BatHitboxManager 
    {
        public static GameObject CreateHitBox(GameObject owner, Vector2 hitBoxDirection, float damage, float duration, AttackType attType)
        {
            var hitbox = new GameObject("HitBox_"+attType);
            hitbox.transform.parent = owner.transform;
            hitbox.transform.localPosition = new Vector3(0, 0, 0);
            var hitboxCollider = hitbox.AddComponent<BoxCollider2D>();
            hitboxCollider.isTrigger = true;
            
            var hitboxScript = hitbox.AddComponent<PlayerHitbox>();
            hitboxScript.damage = damage;
            hitboxScript.hitBoxDirection = hitBoxDirection;

            if (attType == AttackType.AerialAttack)
            {
                hitboxCollider.size = new Vector2(1.55f, 1.8f);
                hitboxCollider.offset = new Vector2(-0.1f * hitBoxDirection.x, 0.98f);
                hitboxScript.hitLag = 0.01f;
            }
            else
            {
                hitboxCollider.size = new Vector2(1.65f, 1.7f);
                hitboxCollider.offset = new Vector2(0.03f * hitBoxDirection.x, 1.3f);
                hitboxScript.hitLag = 0.02f;
            }
            
            Debug.Log("Created Hitbox for " + attType + " with duration " + duration);
            return hitbox;
        }

        private static IEnumerator DestroyHitBoxAfterTime(GameObject hitbox, float duration)
        {
            yield return new WaitForSeconds(duration);
            Object.Destroy(hitbox);
            Debug.Log("Destroyed");
        }
        
    }
}