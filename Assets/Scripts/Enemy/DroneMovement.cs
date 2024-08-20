using System.Collections;
using UnityEngine;
using Player;

public class DroneMovement : MonoBehaviour
{
    public float speed = 2f;
    public bool startMovingRight = true;
    public float stunDuration = 2f;
    public float invulnerabilityTime = 3f;
    public AudioClip stunSound; 
    private Animator animator; 
    private Vector2 direction;
    private Rigidbody2D rb;
    private bool isStunning = false;
    private bool isInvulnerable = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        direction = startMovingRight ? Vector2.right : Vector2.left;
        if (startMovingRight)
        {
            Flip();
        }
    }

    void Update()
    {
      
            rb.velocity = direction * speed;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            direction = -direction;
            Flip(); 
        }

        if ((collision.gameObject.CompareTag("Play") || collision.gameObject.name == "Spike") && !isInvulnerable)
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null && !isStunning)
            {
                direction = -direction;
                Flip();
                StartCoroutine(StunPlayer(playerController));
                PlayStunSound();
                PlayStunAnimation(playerController);
            }
        }
    }

    private IEnumerator StunPlayer(PlayerController playerController)
    {
        isStunning = true;
        playerController.StateMachine.ChangeState(playerController.StateContainer.PlayerStunnedState);
        playerController.enabled = false; // Deshabilitar controles del jugador

        yield return new WaitForSeconds(stunDuration);

        playerController.enabled = true; // Habilitar controles del jugador
        isStunning = false;

        isInvulnerable = true; // Activar invulnerabilidad temporal
        yield return new WaitForSeconds(invulnerabilityTime);
        isInvulnerable = false;
    }

    private void PlayStunSound()
    {
        if (stunSound != null)
        {
            AudioSource.PlayClipAtPoint(stunSound, transform.position);
        }
    }

    private void PlayStunAnimation(PlayerController playerController)
    {
        if (playerController.anim != null)
        {
            playerController.anim.Play("damage");
        }
    }

    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
