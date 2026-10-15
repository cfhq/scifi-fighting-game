using UnityEngine;

public class Player : Character
{
    private Rigidbody2D rb;
    private SimpleEffectsController effects;
    private Vector2 lastMoveDirection = Vector2.right;
    private float nextMoveFxTime;

    [Header("Combat")]
    public float attackRange = 1.2f;
    public int attackDamage = 50;
    public LayerMask enemyLayer;

    [Header("VFX/SFX")]
    public AudioClip attackClip;
    public AudioClip moveClip;
    public AudioClip damageClip;
    public ParticleSystem attackVfxPrefab;
    public ParticleSystem moveVfxPrefab;
    [Range(0.05f, 1f)]
    public float moveFxInterval = 0.2f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        effects = GetComponent<SimpleEffectsController>();
        if (effects == null)
        {
            effects = gameObject.AddComponent<SimpleEffectsController>();
            effects.spatialBlend = 0f;
        }
    }

    void Update()
    {
        Move();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }

    public override void Move()
    {
        if (rb == null)
        {
            return;
        }

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector2 input = new Vector2(x, y).normalized;
        rb.velocity = input * moveSpeed;

        if (input.sqrMagnitude > 0.0001f)
        {
            lastMoveDirection = input;
            if (Time.time >= nextMoveFxTime)
            {
                nextMoveFxTime = Time.time + moveFxInterval;
                if (effects != null)
                {
                    effects.PlayOneShot(moveClip, 0.25f, 1f);
                    effects.SpawnEffect(moveVfxPrefab, transform.position, 0.8f);
                }
            }
        }
    }

    public override void TakeDamage(int damage)
    {
        health -= damage;
        if (effects != null)
        {
            effects.PlayOneShot(damageClip, 0.8f);
            effects.SpawnEffect(null, transform.position, 1f);
        }
    }

    void Attack()
    {
        Vector3 attackFxPosition = transform.position + (Vector3)(lastMoveDirection * Mathf.Max(0.1f, attackRange * 0.6f));
        if (effects != null)
        {
            effects.PlayOneShot(attackClip, 0.9f);
            effects.SpawnEffect(attackVfxPrefab, attackFxPosition, 1f);
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            attackRange,
            enemyLayer
        );

        foreach (Collider2D hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
