using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public int health = 3;

    public void TakeDamage(int damage, Vector2 hitDirection)
    {
        health -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage! Remaining HP: {health}");

        // knockback
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.AddForce(hitDirection * 5f, ForceMode2D.Impulse);

        if (health <= 0)
            Die();
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} died!");
        Destroy(gameObject);
    }
}