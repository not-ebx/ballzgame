using UnityEngine;

public class BallPhysics : MonoBehaviour
{
    public float initialSpeed = 20f;
    public float frictionCoefficient = 0.97f; 
    public float maxSpeed = 400f;
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
            boostAudioSource.Play();
        }

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        bounceSound.Play();

        Vector2 direction = rb.velocity.normalized;

        float angleFromHorizontal = Mathf.Abs(Vector2.Angle(direction, Vector2.right));
        float angleFromVertical = Mathf.Abs(Vector2.Angle(direction, Vector2.up));

        rb.velocity = direction.normalized * (rb.velocity.magnitude * frictionCoefficient);

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }
}
