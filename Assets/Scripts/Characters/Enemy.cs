using UnityEngine;
using System.Collections;

public class Enemy : Character
{
    private Rigidbody2D rb;
    private Transform player;
    private SpriteRenderer spriteRenderer;
    private SimpleEffectsController effects;

    [Header("Enemy Behavior")]
    public float stopDistance = 1.1f;

    [Header("Screen Bounds")]
    public float minX = -9f;
    public float maxX = 9f;
    public float minY = -5f;
    public float maxY = 5f;

    [Header("VFX/SFX")]
    public AudioClip hitClip;
    public AudioClip deathClip;
    public ParticleSystem hitVfxPrefab;
    public ParticleSystem deathVfxPrefab;
    public Color hitFlashColor = new Color(1f, 0.6f, 0.6f, 1f);
    [Range(0.03f, 0.2f)]
    public float hitFlashDuration = 0.08f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        effects = GetComponent<SimpleEffectsController>();
        if (effects == null)
        {
            effects = gameObject.AddComponent<SimpleEffectsController>();
            effects.spatialBlend = 0f;
        }

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        Move();
    }

    public override void Move()
    {
        if (rb == null)
        {
            return;
        }

        if (player == null)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= stopDistance)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;
    }

    void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;
    }

    public override void TakeDamage(int damage)
    {
        health -= damage;
        if (effects != null)
        {
            effects.PlayOneShot(hitClip, 0.7f);
            effects.SpawnEffect(hitVfxPrefab, transform.position, 0.9f);
        }

        if (spriteRenderer != null)
        {
            StopAllCoroutines();
            StartCoroutine(HitFlashRoutine());
        }

        if (health <= 0)
        {
            if (effects != null)
            {
                effects.PlayDetachedAtPoint(deathClip, transform.position, 0.9f);
                effects.SpawnEffect(deathVfxPrefab, transform.position, 1.3f);
            }
            Destroy(gameObject);
        }
    }

    IEnumerator HitFlashRoutine()
    {
        Color startColor = spriteRenderer.color;
        spriteRenderer.color = hitFlashColor;
        yield return new WaitForSeconds(hitFlashDuration);
        if (spriteRenderer != null)
        {
            spriteRenderer.color = startColor;
        }
    }
}
