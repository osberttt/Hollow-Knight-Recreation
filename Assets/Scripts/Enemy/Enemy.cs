using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("References")] 
    public Transform wallCheck;
    
    [Header("Settings")]
    public int health = 3;
    public float speed = 2f;
    public float knockbackSpeed = 5f;
    public float knockbackTime = 0.5f;
    public float wallCheckRadius = 0.1f;
    public LayerMask wallLayer;
    public float deathForce = 5f;
    public Sprite deathSprite;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private int direction = 1;
    private Vector2 hitDir;

    public StateMachine StateMachine { get; private set; }
    public WalkState WalkState { get; private set; }
    public KnockBackState KnockBackState { get; private set; }
    public DieState DieState {get; private set;}
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        
        StateMachine = new StateMachine();
        WalkState = new WalkState(this);
        KnockBackState = new KnockBackState(this);
        DieState = new DieState(this);
    }
    
    private void Start()
    {
        StateMachine.ChangeState(WalkState);
    }
    
    private void Update()
    {
        StateMachine.Update();
    }

    private void FixedUpdate()
    {
        StateMachine.FixedUpdate();
    }

    public void Move()
    {
        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
    }
    
    public void KnockBack()
    {
        rb.linearVelocity = new Vector2(hitDir.x * knockbackSpeed, rb.linearVelocity.y);
    }

    public void ChangeDirection()
    {
        direction *= -1;
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * direction, transform.localScale.y, transform.localScale.z);
    }

    public void ChangeSpriteColor(Color color)
    {
        sr.color = color;
    }
    public void TakeDamage(int damage, Vector2 hitDirection)
    {
        health -= damage;
        hitDir = hitDirection;
        
        StateMachine.ChangeState(KnockBackState);

        if (health <= 0)
        {
            StateMachine.ChangeState(DieState);
        }
    }

    public void Die()
    {
        sr.sprite = deathSprite;
        rb.constraints = RigidbodyConstraints2D.None;
        var deathDir = new Vector2(hitDir.x, 0.2f);
        rb.AddForce(deathDir * deathForce, ForceMode2D.Impulse);
    }
    
    void OnDrawGizmos()
    {
        if (wallCheck != null)
        {
            Gizmos.DrawWireSphere(wallCheck.position, wallCheckRadius);
        }
    }
}