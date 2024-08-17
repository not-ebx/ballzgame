using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallPhysics : MonoBehaviour
{
    public float initialSpeed = 20f; 
    public float frictionCoefficient = 0.90f; 
    private Rigidbody2D rb;
    public AudioSource bounceSound;
    public AudioSource boostAudioSource;
    public float speedBoost = 4f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Vector2 initialDirection = new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f)).normalized;
        rb.velocity = initialDirection * initialSpeed;
    }

    void Update()
    {

    }

    public void OnHitBall(Vector2 direction, float damage)
    {
        if (direction == Vector2.zero)
        {
            direction = -rb.velocity.normalized;
        }
        
        rb.velocity *= (direction * (1 + damage + speedBoost));
        rb.velocity = new Vector2(
            Mathf.Abs(rb.velocity.x) * direction.x * (1 + damage + speedBoost),
            Mathf.Abs(rb.velocity.y) * direction.y * (1 + damage + speedBoost)
        );
        Debug.Log("Hit the ball with Direction " + direction + " and Damage " + damage + ". Total Velocity is " + rb.velocity);
        boostAudioSource.Play();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // If colission is with the player, we dont want to bounce
        if (collision.gameObject.CompareTag("Play"))
        {
            return;
        }
        
        bounceSound.Play();
        Vector2 direction = rb.velocity.normalized;

        float angleFromHorizontal = Mathf.Abs(Vector2.Angle(direction, Vector2.right));
        float angleFromVertical = Mathf.Abs(Vector2.Angle(direction, Vector2.up));

        rb.velocity = direction.normalized * (rb.velocity.magnitude * frictionCoefficient);
    }
}
