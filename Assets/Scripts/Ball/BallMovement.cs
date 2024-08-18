using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallPhysics : MonoBehaviour
{
    public float initialSpeed = 20f;
    public float frictionCoefficient = 0.98f; 
    public float maxSpeed = 400f;
    public float dmg;
    private Rigidbody2D rb;
    private float newSpeed;
    private float currentSpeed;
    private float limitedSpeed;
    public AudioSource bounceSound;
    public AudioSource boostAudioSource;
    public float speedBoost = 16f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Vector2 initialDirection = new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f)).normalized;
        rb.velocity = initialDirection * initialSpeed;
    }

    void Update()
    {
        currentSpeed = rb.velocity.magnitude;
        limitedSpeed = Mathf.Min(currentSpeed, maxSpeed);

        rb.velocity = rb.velocity.normalized * limitedSpeed;
    }

    public void OnHitBall(Vector2 direction, float damage)
    {
        boostAudioSource.Play();
        if (direction == Vector2.zero)
        {
            direction = -rb.velocity.normalized;
        }
        else
        {
            direction = direction.normalized; 
        }


        newSpeed = currentSpeed + speedBoost + (damage + 1);

        rb.velocity = direction * Mathf.Min(newSpeed, maxSpeed);

        Debug.Log("Hit the ball with Direction " + direction + " and Damage " + damage + ". Total Velocity is " + rb.velocity);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        bounceSound.Play();

        Vector2 direction = rb.velocity.normalized;

        rb.velocity = direction * (rb.velocity.magnitude * frictionCoefficient);

        Dummy dummy = collision.gameObject.GetComponent<Dummy>();

        if (dummy != null)
        {
            dmg = rb.velocity.magnitude;
            rb.velocity = direction * (rb.velocity.magnitude * 0.9f);
            dummy.TakeDamage(dmg);
        }
    }

}
