using System.Collections;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    public float health = 200f;
    public Color hitColor = Color.red;
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

    private SpriteRenderer spriteRenderer;
    private Collider2D collider2D;
    private Color originalColor;
    private bool isActive = true;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2D = GetComponent<Collider2D>();
        originalColor = spriteRenderer.color;

        if (spawnTime > 0)
        {
            gameObject.SetActive(false);
            Invoke("ActivateDummy", spawnTime);
        }
    }


   private void ActivateDummy()
    {
        gameObject.SetActive(true);
        isActive = true;
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

        ShowDamageText(finalDamage);

        if (health <= 0)
        {
            StartCoroutine(HandleInvisibility());
            
        }
        else
        {
            hitSound.Play();
            StartCoroutine(ChangeColor(hitDuration));
        }
    }

    private IEnumerator ChangeColor(float duration)
    {
        spriteRenderer.color = hitColor;
        yield return new WaitForSeconds(duration);
        spriteRenderer.color = originalColor;
    }

    private IEnumerator HandleInvisibility()
    {
  
        destroySound.Play();
        ShowExplosion();

        spriteRenderer.enabled = false;
        collider2D.enabled = false;

        yield return new WaitForSeconds(respawnTime);

        spriteRenderer.enabled = true;
        collider2D.enabled = true;
        health = 200f;
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
