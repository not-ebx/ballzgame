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

        if (Input.GetKeyDown(KeyCode.F))
        {
            rb.velocity *= speedBoost; 
        }
    }

    public void OnHitBall(Vector2 direction, float damage)
    {
        // If the direction is 0,0, we don't want to multiply by 0 so we just set the direction to the opposite of the current one
        if (direction == Vector2.zero)
        {
            direction = -rb.velocity.normalized;
        }
        
        rb.velocity = (direction * (1 + damage));
        Debug.Log("Hit the ball with Direction " + direction + " and Damage " + damage + ". Total Velocity is " + rb.velocity);
        boostAudioSource.Play();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        bounceSound.Play();
        Vector2 direction = rb.velocity.normalized;

        float angleFromHorizontal = Mathf.Abs(Vector2.Angle(direction, Vector2.right));
        float angleFromVertical = Mathf.Abs(Vector2.Angle(direction, Vector2.up));

        rb.velocity = direction.normalized * (rb.velocity.magnitude * frictionCoefficient);
    }
}
