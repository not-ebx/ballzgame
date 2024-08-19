using System;
using Managers;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallPhysics : MonoBehaviour
{
    public float initialSpeed = 20f;
    public float frictionCoefficient = 0.98f; 
    public float maxSpeed = 200f;
    public float dmg;
    private Rigidbody2D rb;
    private float newSpeed;
    public float currentSpeed;
    private float limitedSpeed;
    public AudioSource bounceSound;
    public AudioSource boostAudioSource;
    public float speedBoost = 16f;

    private Animator _anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();

        Vector2 initialDirection = new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f)).normalized;
        rb.velocity = initialDirection * initialSpeed;
    }

    void Update()
    {
        currentSpeed = rb.velocity.magnitude;
        limitedSpeed = Mathf.Min(currentSpeed, maxSpeed);
        rb.velocity = rb.velocity.normalized * limitedSpeed;
        _anim.SetFloat("Speed", currentSpeed);
        
        // Set the rotation of the ball to the direction it is moving
        var angle = Mathf.Atan2(rb.velocity.y, -rb.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public void OnHitBall(PlayerHitbox hitBox, Vector2 direction, float damage, float hitLag)
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

        newSpeed = currentSpeed + (speedBoost*damage);

        var newHitlag = Mathf.Min(hitLag * newSpeed / 10, 2.2f);
        hitBox.Stop(newHitlag);
        var cameraManager = FindObjectOfType<CameraManager>();
        if (cameraManager != null)
        {
            CoroutineManager.Instance.StartManagedCoroutine(cameraManager.Shake(newHitlag, 0.1f, (1 + newHitlag) * 10f));
        }
        
        rb.velocity = direction * Mathf.Min(newSpeed, maxSpeed);

        Debug.Log("Hit the ball with Direction " + direction + " and Damage " + damage + ". Total Velocity is " + rb.velocity + " And hitlag duration is " + newHitlag);
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
