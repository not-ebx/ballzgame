using System.Collections;
using Player;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    public float health = 1200f;
    private float maxHealth = 0f;
    public Color hitColor = Color.red;
    public Color reflectColor = Color.blue;
    public float dummyValue = 50f;
    public float hitDuration = 0.2f;
    public AudioSource hitSound;
    public Vector3 damageTextOffset = new Vector3(0, 1.5f, 0);
    public Vector3 explosionOffset = new Vector3(4, -1, 0);
    private int roundedDamage;
    public AudioSource destroySound; 
    public GameObject damageTextPrefab;
    public float respawnTime = 5f;
    public float spawnTime = 0f;
    public float despawnTime = 0f;
    public GameObject explosionPrefab;

    private float currentHealth;
    private SpriteRenderer spriteRenderer;
    private Collider2D _c2d;
    private EdgeCollider2D _cedge;
    private Color originalColor;
    private Animator _anim;
    private Vector3 originalPosition;
    private bool isRespawning = false;
    public float yOffset = 0f;
    public float animationDuration = 0.3f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        _c2d = GetComponent<Collider2D>();
        _cedge = GetComponent<EdgeCollider2D>();
        _anim = GetComponent<Animator>();
        originalColor = spriteRenderer.color;
        maxHealth = health;
        _anim.SetFloat("health", maxHealth);
        _anim.SetBool("isRespawning", isRespawning);
        originalPosition = transform.position;

        if (spawnTime > 0)
        {
            gameObject.SetActive(false);
            Invoke("ActivateDummy", spawnTime);
        }
        if (yOffset > 0){
            StartCoroutine(AnimatePosition());
        }
    }

    private void ActivateDummy()
    {
        gameObject.SetActive(true);
    }

    private IEnumerator AnimatePosition()
    {
        Vector3 originalPosition = transform.position;

        while (true)
        {
            transform.position = new Vector3(originalPosition.x, originalPosition.y + yOffset, originalPosition.z);
            yield return new WaitForSeconds(animationDuration);

            transform.position = originalPosition;
            yield return new WaitForSeconds(animationDuration);
        }
    }

    void Update()
    {
        if (despawnTime > 0)
        {
            Destroy(gameObject, despawnTime);
        }
    }

    public void TakeDamage(float dmg)
    {
        float finalDamage = dmg;
        health -= finalDamage;

        _anim.SetFloat("health", health);

        ShowDamageText(finalDamage);

        if (health <= 0)
        {
            var player = GameObject.Find("Player");
            if (player && player.GetComponent<PlayerController>())
            {
                var playerController = player.GetComponent<PlayerController>();
                playerController.Score += (int)dummyValue;
            }
            StartCoroutine(HandleInvisibility());
        }
        else
        {
            hitSound.Play();
            StartCoroutine(ChangeColor(hitDuration, hitColor));
        }
    }

    public void HandleReflection()
    {
        StartCoroutine(ChangeColor(hitDuration, reflectColor));
    }

    private IEnumerator ChangeColor(float duration, Color newColor)
    {
        spriteRenderer.color = newColor;
        yield return new WaitForSeconds(duration);
        spriteRenderer.color = originalColor;
    }

    private IEnumerator HandleInvisibility()
    {
        Debug.Log("HandleInvisibility started");
        destroySound.Play();
        ShowExplosion();

        spriteRenderer.enabled = false;
        _c2d.enabled = false;
        Collider2D[] allColliders = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D collider in allColliders)
        {
            collider.enabled = false;
        }
        isRespawning = true;
        _anim.SetBool("isRespawning", isRespawning);
        health = maxHealth;
        _anim.SetFloat("health", maxHealth);
        Debug.Log("Waiting for respawn...");
        yield return new WaitForSeconds(respawnTime);
        Debug.Log("Respawn time finished");

        transform.position = originalPosition;

        spriteRenderer.enabled = true;
        _c2d.enabled = true;
        isRespawning = false;
        _anim.SetBool("isRespawning", isRespawning);

        foreach (Collider2D collider in allColliders)
        {
            collider.enabled = true;
        }

        yield return new WaitForEndOfFrame();
        transform.position = originalPosition;

        Debug.Log("Dummy respawned at: " + transform.position);
    }

    private void ShowDamageText(float dmg)
    {
        if (damageTextPrefab != null)
        {
            roundedDamage = Mathf.RoundToInt(dmg);
            Vector3 spawnPosition = transform.position + damageTextOffset;

            GameObject damageText = Instantiate(damageTextPrefab, spawnPosition, Quaternion.identity);
            damageText.GetComponent<TextMesh>().text = roundedDamage.ToString();

            Destroy(damageText, 0.3f);
        }
    }

    private void ShowExplosion()
    {
        if (explosionPrefab != null)
        {
            Vector3 spawnPosition  =  transform.position;
            GameObject explosion = Instantiate(explosionPrefab, spawnPosition, Quaternion.identity);
        }
    }
}