using System;
using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    public int damage = 1;
    public float lifetime = 0.34f;   // destroy after short time
    public Vector2 attackDir;       // set by player when spawned
    
    private BoxCollider2D _boxCollider;
    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable dmg = other.GetComponent<IDamageable>();
        if (dmg != null)
        {
            dmg.TakeDamage(damage, attackDir);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + (Vector3)_boxCollider.offset, _boxCollider.size);
    }
}